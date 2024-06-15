namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 24, CodeType.Original)]
public partial class Day_24_Original : IPuzzle
{
	private sealed class Group
	{
		public int Id { get; init; }
		public int LiveUnits { get; set; }
		public int HitPoints { get; init; }
		public string DamageType { get; init; }
		public int AttackDamage { get; init; }
		public int Initiative { get; init; }
		public IReadOnlyList<string> Weaknesses { get; init; }
		public IReadOnlyList<string> Immunities { get; init; }

		public int EffectivePower => LiveUnits * AttackDamage;
	}

	private sealed class Army
	{
		public string Name { get; init; }
		public List<Group> Groups { get; init; } = [];
	}

	[GeneratedRegex(@"^(?<u>\d+) units each with (?<hp>\d+) hit points (\(((; )?immune to ((, )?(?<immune>\w+))+|(; )?weak to ((, )?(?<weak>\w+))+)+\) )?with an attack that does (?<dmg>\d+) (?<type>\w+) damage at initiative (?<init>\d+)$", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
	private static partial Regex UnitRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Lines
			.Segment(l => l.EndsWith(':'));

		var regex = UnitRegex();

		var armies = data
			.Select((g, j) =>
			{
				var name = g[0].TrimEnd(':');

				var groups = g.Skip(1)
					.Where(s => !string.IsNullOrWhiteSpace(s))
					.Select(s => regex.Match(s))
					.Select((m, i) => new Group
					{
						Id = (j << 8) | (i + 1),
						LiveUnits = Convert.ToInt32(m.Groups["u"].Value),
						HitPoints = Convert.ToInt32(m.Groups["hp"].Value),
						AttackDamage = Convert.ToInt32(m.Groups["dmg"].Value),
						DamageType = m.Groups["type"].Value,
						Initiative = Convert.ToInt32(m.Groups["init"].Value),
						Weaknesses = m.Groups["weak"].Captures.OfType<Capture>().Select(c => c.Value).ToList(),
						Immunities = m.Groups["immune"].Captures.OfType<Capture>().Select(c => c.Value).ToList(),
					})
					.ToList();

				return new Army
				{
					Name = name,
					Groups = groups,
				};
			})
			.ToList();

		var part1 = DoBattle(armies, 0).units.ToString();

		// start at 34... 33 has an infinite loop; should fix later...
		var part2 =
			Enumerable.Range(34, 1_000_000)
				.Select(i => DoBattle(armies, i))
				.First(i => i.army == 0)
				.units
				.ToString();

		return (part1, part2);
	}

	private static (int army, int units) DoBattle(List<Army> armies, int boost)
	{
		armies = armies
			.Select((a, i) => new Army
			{
				Name = a.Name,
				Groups = a.Groups
					.Select(g => new Group
					{
						Id = g.Id,
						LiveUnits = g.LiveUnits,
						HitPoints = g.HitPoints,
						AttackDamage = i == 0 ? g.AttackDamage + boost : g.AttackDamage,
						DamageType = g.DamageType,
						Initiative = g.Initiative,
						Weaknesses = g.Weaknesses,
						Immunities = g.Immunities,
					})
					.ToList(),
			})
			.ToList();

		var groupsById = armies.SelectMany(a => a.Groups)
			.ToDictionary(g => g.Id);

		while (armies.All(a => a.Groups.Any(g => g.LiveUnits > 0)))
		{
			// target round
			var targets = new List<(int attacker, int defender, int ap)>();
			for (var i = 0; i < 2; i++)
			{
				var army = armies[i];
				var enemy = armies[1 - i];
				foreach (var g in army.Groups
					.OrderByDescending(g => g.EffectivePower)
					.ThenByDescending(g => g.Initiative))
				{
					var target = enemy.Groups
						.Where(g2 => !targets.Select(t => t.defender).Contains(g2.Id))
						.Select(g2 =>
						{
							var ap = g.AttackDamage;
							if (g2.Immunities.Contains(g.DamageType))
								ap = 0;
							if (g2.Weaknesses.Contains(g.DamageType))
								ap *= 2;
							return (g2, ap);
						})
						.Where(x => x.ap != 0)
						.OrderByDescending(x => x.ap)
						.ThenByDescending(x => x.g2.EffectivePower)
						.ThenByDescending(x => x.g2.Initiative)
						.FirstOrDefault();
					if (target != default)
						targets.Add((g.Id, target.g2.Id, target.ap));
				}
			}

			// attack round
			foreach (var (attackerId, defenderId, ap) in targets
				.OrderByDescending(t => groupsById[t.attacker].Initiative))
			{
				var attacker = groupsById[attackerId];
				var defender = groupsById[defenderId];

				var damage = ap * attacker.LiveUnits;
				var units = Math.Min(damage / defender.HitPoints, defender.LiveUnits);
				defender.LiveUnits -= units;
			}

			// clean up
			foreach (var a in armies)
				_ = a.Groups.RemoveAll(g => g.LiveUnits <= 0);
		}

		return armies
			.Select((a, i) => (idx: i, cnt: a.Groups.Sum(g => g.LiveUnits)))
			.First(x => x.cnt != 0);
	}
}
