using System.Collections.Immutable;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 22, CodeType.Original)]
public class Day_22_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var stats = input.Lines;

		var hitPoints = Convert.ToInt32(stats[0].Split().Last());
		var damage = Convert.ToInt32(stats[1].Split().Last());

		return (
			HandleTurn(false, new CharacterSet(hitPoints, damage)).minMana.ToString(),
			HandleTurn(true, new CharacterSet(hitPoints, damage)).minMana.ToString());
	}

	private enum TurnAction
	{
		BossAttack,
		MagicMissile,
		Drain,
		Shield,
		Poison,
		Recharge,
	}

	private static (int minMana, ImmutableStack<TurnAction> actions) HandleTurn(bool partB, CharacterSet set) =>
		HandleTurn(partB, set, (int.MaxValue, ImmutableStack<TurnAction>.Empty), (0, ImmutableStack<TurnAction>.Empty));

	private static (int minMana, ImmutableStack<TurnAction> actions) HandleTurn(bool partB, CharacterSet set, (int minMana, ImmutableStack<TurnAction> actions) minActions, (int minMana, ImmutableStack<TurnAction> actions) actionsSoFar)
	{
		set.DoNextTurn(partB);

		if (!set.Player.IsAlive)
			return (int.MaxValue, ImmutableStack<TurnAction>.Empty);
		if (!set.Boss.IsAlive)
			return actionsSoFar;

		set.ProcessEffects();
		if (!set.Player.IsAlive)
			return (int.MaxValue, ImmutableStack<TurnAction>.Empty);
		if (!set.Boss.IsAlive)
			return actionsSoFar;

		switch (set.WhoseTurn)
		{
			case WhoseTurn.BossTurn:
				set.Boss.Attack(set.Player);
				return HandleTurn(partB, set, minActions, (actionsSoFar.minMana, actionsSoFar.actions.Push(TurnAction.BossAttack)));

			case WhoseTurn.PlayerTurn:
				foreach (var effect in _Effects)
				{
					if (set.Player.Mana < effect.Mana)
						continue;

					if (set.Player.Effects.Select(e => e.Effect).Contains(effect))
						continue;

					var testSet = set.Clone();
					testSet.Player.Cast(effect, testSet.Boss);
					var testCost = actionsSoFar.minMana + effect.Mana;
					if (testCost > minActions.minMana)
						continue;
					var playCost = HandleTurn(partB, testSet, minActions, (testCost, actionsSoFar.actions.Push(effect.TurnAction)));
					if (playCost.minMana < minActions.minMana)
						minActions = playCost;
				}
				return minActions;

			default:
				throw new InvalidOperationException("Should Never Happen.");
		}
	}

	public enum WhoseTurn
	{
		PlayerTurn,
		BossTurn,
	}

	private sealed class CharacterSet
	{
		public Character Boss { get; private set; }
		public Character Player { get; private set; }
		public WhoseTurn WhoseTurn { get; private set; }

		public CharacterSet(int hitPoints, int damage)
		{
			Boss = new Character(hitPoints, damage, 0, 0);
			Player = new Character(50, 0, 0, 500);

			// set to boss first, so first turn will switch to player
			WhoseTurn = WhoseTurn.BossTurn;
		}

		private CharacterSet(CharacterSet previous)
		{
			Boss = previous.Boss.Clone();
			Player = previous.Player.Clone();
		}

		public void DoNextTurn(bool partB)
		{
			WhoseTurn = WhoseTurn == WhoseTurn.PlayerTurn ? WhoseTurn.BossTurn : WhoseTurn.PlayerTurn;

			Player.ResetTurn();
			Boss.ResetTurn();

			if (partB && WhoseTurn == WhoseTurn.PlayerTurn)
				Player.HitPoints--;
		}

		public void ProcessEffects()
		{
			foreach (var effect in Player.Effects)
			{
				effect.Effect.DoEffect(Boss, Player);
				effect.Timer--;
			}
			_ = Player.Effects.RemoveAll(e => e.Timer <= 0);

			foreach (var effect in Boss.Effects)
			{
				effect.Effect.DoEffect(Player, Boss);
				effect.Timer--;
			}
			_ = Boss.Effects.RemoveAll(e => e.Timer <= 0);
		}

		public CharacterSet Clone() => new(this);
	}

	private sealed class Character
	{
		public int HitPoints { get; set; }
		public int Damage { get; set; }

		public int Armor => BaseArmor + EffectArmor;
		public int BaseArmor { get; set; }
		public int EffectArmor { get; set; }

		public int Mana { get; set; }
		public List<ActiveEffect> Effects { get; private set; }

		public bool IsAlive => HitPoints > 0;

		public Character(int hitPoints, int damage, int baseArmor, int mana)
		{
			HitPoints = hitPoints;
			Damage = damage;
			BaseArmor = baseArmor;
			Mana = mana;

			Effects = new List<ActiveEffect>();
			EffectArmor = 0;
		}

		private Character() { }

		public Character Clone()
		{
			var c = new Character
			{
				HitPoints = HitPoints,
				Damage = Damage,
				BaseArmor = BaseArmor,
				EffectArmor = EffectArmor,
				Mana = Mana,
				Effects = Effects
					.Select(e => new ActiveEffect()
					{
						Effect = e.Effect,
						Timer = e.Timer,
					})
					.ToList(),
			};

			return c;
		}

		public void ResetTurn()
		{
			EffectArmor = 0;
		}

		public void Attack(Character defender)
		{
			var damage = Math.Max(Damage - defender.Armor, 1);
			defender.HitPoints -= damage;
		}

		public void Cast(Effect effect, Character defender)
		{
			if (effect.IsImmediate)
				effect.DoEffect(defender, this);

			if (effect.Length > 0)
			{
				Effects.Add(new ActiveEffect()
				{
					Effect = effect,
					Timer = effect.Length,
				});
			}

			Mana -= effect.Mana;
		}
	}

	private sealed class ActiveEffect
	{
		public Effect Effect { get; set; }
		public int Timer { get; set; }
	}

	private abstract class Effect
	{
		public abstract int Mana { get; }
		public abstract int Length { get; }
		public abstract bool IsImmediate { get; }
		public abstract TurnAction TurnAction { get; }

		public abstract void DoEffect(Character defender, Character attacker);
	}

	private static readonly IReadOnlyList<Effect> _Effects = new Effect[]
	{
		new MagicMissile(),
		new Drain(),
		new Shield(),
		new Poison(),
		new Recharge(),
	};

	private sealed class MagicMissile : Effect
	{
		public override int Mana => 53;
		public override int Length => 0;
		public override bool IsImmediate => true;
		public override TurnAction TurnAction => TurnAction.MagicMissile;

		public override void DoEffect(Character boss, Character player)
		{
			boss.HitPoints -= 4;
		}
	}

	private sealed class Drain : Effect
	{
		public override int Mana => 73;
		public override int Length => 0;
		public override bool IsImmediate => true;
		public override TurnAction TurnAction => TurnAction.Drain;

		public override void DoEffect(Character boss, Character player)
		{
			boss.HitPoints -= 2;
			player.HitPoints += 2;
		}
	}

	private sealed class Shield : Effect
	{
		public override int Mana => 113;
		public override int Length => 6;
		public override bool IsImmediate => false;
		public override TurnAction TurnAction => TurnAction.Shield;

		public override void DoEffect(Character boss, Character player)
		{
			player.EffectArmor += 7;
		}
	}

	private sealed class Poison : Effect
	{
		public override int Mana => 173;
		public override int Length => 6;
		public override bool IsImmediate => false;
		public override TurnAction TurnAction => TurnAction.Poison;

		public override void DoEffect(Character boss, Character player)
		{
			boss.HitPoints -= 3;
		}
	}

	private sealed class Recharge : Effect
	{
		public override int Mana => 229;
		public override int Length => 5;
		public override bool IsImmediate => false;
		public override TurnAction TurnAction => TurnAction.Recharge;

		public override void DoEffect(Character boss, Character player)
		{
			player.Mana += 101;
		}
	}
}
