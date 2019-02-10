using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2015_22_Original : Day
	{
		public override int Year => 2015;
		public override int DayNumber => 22;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var stats = input.GetLines();

			var hitPoints = Convert.ToInt32(stats[0].Split().Last());
			var damage = Convert.ToInt32(stats[1].Split().Last());

			Dump('A', HandleTurn(false, new CharacterSet(hitPoints, damage)).Item1);
			Dump('B', HandleTurn(true, new CharacterSet(hitPoints, damage)).Item1);
		}

		enum TurnAction
		{
			BossAttack,
			MagicMissile,
			Drain,
			Shield,
			Poison,
			Recharge,
		}

		Tuple<int, ImmutableStack<TurnAction>> HandleTurn(bool partB, CharacterSet set)
		{
			return _HandleTurn(partB, set, Tuple.Create(int.MaxValue, ImmutableStack<TurnAction>.Empty), Tuple.Create(0, ImmutableStack<TurnAction>.Empty));
		}

		Tuple<int, ImmutableStack<TurnAction>> _HandleTurn(bool partB, CharacterSet set, Tuple<int, ImmutableStack<TurnAction>> minActions, Tuple<int, ImmutableStack<TurnAction>> actionsSoFar)
		{
			set.DoNextTurn(partB);

			if (!set.Player.IsAlive)
				return Tuple.Create(int.MaxValue, ImmutableStack<TurnAction>.Empty);
			if (!set.Boss.IsAlive)
				return actionsSoFar;

			set.ProcessEffects();
			if (!set.Player.IsAlive)
				return Tuple.Create(int.MaxValue, ImmutableStack<TurnAction>.Empty);
			if (!set.Boss.IsAlive)
				return actionsSoFar;

			switch (set.WhoseTurn)
			{
				case WhoseTurn.BossTurn:
					set.Boss.Attack(set.Player);
					return _HandleTurn(partB, set, minActions, Tuple.Create(actionsSoFar.Item1, actionsSoFar.Item2.Push(TurnAction.BossAttack)));

				case WhoseTurn.PlayerTurn:
					foreach (var effect in _Effects)
					{
						if (set.Player.Mana < effect.Mana)
							continue;

						if (set.Player.Effects.Select(e => e.Effect).Contains(effect))
							continue;

						var testSet = set.Clone();
						testSet.Player.Cast(effect, testSet.Boss);
						var testCost = actionsSoFar.Item1 + effect.Mana;
						if (testCost > minActions.Item1)
							continue;
						var playCost = _HandleTurn(partB, testSet, minActions, Tuple.Create(testCost, actionsSoFar.Item2.Push(effect.TurnAction)));
						if (playCost.Item1 < minActions.Item1)
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

		class CharacterSet
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
				Player.Effects.RemoveAll(e => e.Timer <= 0);

				foreach (var effect in Boss.Effects)
				{
					effect.Effect.DoEffect(Player, Boss);
					effect.Timer--;
				}
				Boss.Effects.RemoveAll(e => e.Timer <= 0);
			}

			public CharacterSet Clone() => new CharacterSet(this);
		}

		class Character
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
				var c = new Character();

				c.HitPoints = this.HitPoints;
				c.Damage = this.Damage;
				c.BaseArmor = this.BaseArmor;
				c.EffectArmor = this.EffectArmor;
				c.Mana = this.Mana;
				c.Effects = new List<ActiveEffect>(this.Effects.Select(e => new ActiveEffect()
				{
					Effect = e.Effect,
					Timer = e.Timer,
				}));

				return c;
			}

			public void ResetTurn()
			{
				this.EffectArmor = 0;
			}

			public void Attack(Character defender)
			{
				var damage = Math.Max(this.Damage - defender.Armor, 1);
				defender.HitPoints -= damage;
			}

			public void Cast(Effect effect, Character defender)
			{
				if (effect.IsImmediate)
					effect.DoEffect(defender, this);

				if (effect.Length > 0)
					this.Effects.Add(new ActiveEffect()
					{
						Effect = effect,
						Timer = effect.Length,
					});

				this.Mana -= effect.Mana;
			}
		}

		class ActiveEffect
		{
			public Effect Effect { get; set; }
			public int Timer { get; set; }
		}

		abstract class Effect
		{
			public abstract int Mana { get; }
			public abstract int Length { get; }
			public abstract bool IsImmediate { get; }
			public abstract TurnAction TurnAction { get; }

			public abstract void DoEffect(Character defender, Character attacker);
		}

		static List<Effect> _Effects = new List<Effect>()
{
	new MagicMissile(),
	new Drain(),
	new Shield(),
	new Poison(),
	new Recharge(),
};

		class MagicMissile : Effect
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

		class Drain : Effect
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

		class Shield : Effect
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

		class Poison : Effect
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

		class Recharge : Effect
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
}
