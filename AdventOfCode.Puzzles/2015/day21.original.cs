namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 21, CodeType.Original)]
public partial class Day_21_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var weapons = ParseItems(
@"Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0");

		var armor = ParseItems(
@"Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5").Concat(new Item[] { null });

		var rings = ParseItems(
@"Damage +1    25     1       0
Damage +2    50     2       0
Damage +3   100     3       0
Defense +1   20     0       1
Defense +2   40     0       2
Defense +3   80     0       3").Concat(new Item[] { null });

		var stats = input.Lines;

		var boss = new Character
		{
			HitPoints = Convert.ToInt32(stats[0].Split().Last()),
			Damage = Convert.ToInt32(stats[1].Split().Last()),
			Armor = Convert.ToInt32(stats[2].Split().Last()),
		};

		var candidates =
			from w in weapons
			from a in armor
			from r1 in rings
			from r2 in rings
			where r1 != r2 || (r1 == null && r2 == null)
			let player = BuildPlayerCharacter(w, a, r1, r2)
			where CanPlayerWin(player, boss)
			orderby player.Cost
			select player;
		var partA = candidates.First().Cost;

		candidates =
			from w in weapons
			from a in armor
			from r1 in rings
			from r2 in rings
			where r1 != r2 || (r1 == null && r2 == null)
			let player = BuildPlayerCharacter(w, a, r1, r2)
			where !CanPlayerWin(player, boss)
			orderby player.Cost descending
			select player;
		var partB = candidates.First().Cost;

		return (partA.ToString(), partB.ToString());
	}

	private sealed class Character
	{
		public int HitPoints { get; set; }
		public int Damage { get; set; }
		public int Armor { get; set; }
		public int Cost { get; set; }

		public Item WeaponItem { get; set; }
		public Item ArmorItem { get; set; }
		public Item Ring1Item { get; set; }
		public Item Ring2Item { get; set; }
	}

	private sealed class Item
	{
		public string Name { get; set; }
		public int Cost { get; set; }
		public int Damage { get; set; }
		public int Armor { get; set; }
	}

	[GeneratedRegex("(.+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)")]
	private static partial Regex ParseRegex();

	private static IList<Item> ParseItems(string input) => 
		input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
			.Select(s => ParseRegex().Match(s))
			.Select(m => new Item()
			{
				Name = m.Groups[1].Value,
				Cost = Convert.ToInt32(m.Groups[2].Value),
				Damage = Convert.ToInt32(m.Groups[3].Value),
				Armor = Convert.ToInt32(m.Groups[4].Value),
			})
			.ToList();

	private static bool CanPlayerWin(Character player, Character boss)
	{
		var playerHitPointsPerTurn = Math.Max(boss.Damage - player.Armor, 1);
		var bossHitPointsPerTurn = Math.Max(player.Damage - boss.Armor, 1);

		var playerTurns = player.HitPoints / playerHitPointsPerTurn;
		var bossTurns = boss.HitPoints / bossHitPointsPerTurn;

		return bossTurns <= playerTurns;
	}

	private static Character BuildPlayerCharacter(Item weapon, Item armor, Item ring1, Item ring2) =>
		new()
		{
			HitPoints = 100,
			Armor = weapon.Armor + (armor?.Armor ?? 0) + (ring1?.Armor ?? 0) + (ring2?.Armor ?? 0),
			Damage = weapon.Damage + (armor?.Damage ?? 0) + (ring1?.Damage ?? 0) + (ring2?.Damage ?? 0),
			Cost = weapon.Cost + (armor?.Cost ?? 0) + (ring1?.Cost ?? 0) + (ring2?.Cost ?? 0),
			WeaponItem = weapon,
			ArmorItem = armor,
			Ring1Item = ring1,
			Ring2Item = ring2,
		};
}
