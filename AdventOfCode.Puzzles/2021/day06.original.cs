namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 6, CodeType.Original)]
public class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var fish = input.Text
			.Split(',')
			.Select(int.Parse)
			// don't keep track of each fish individually
			// only keep track of how many of each age
			.GroupBy(x => x)
			.ToDictionary(g => g.Key, g => (long)g.Count());

	// handles a single day cycle
	// each day decrements except for special cases
	static Dictionary<int, long> DayCycle(Dictionary<int, long> fish) =>
		new()
		{
			// all of the 0 ages go to 8 as new fish
			[8] = fish.GetValueOrDefault(0),
			[7] = fish.GetValueOrDefault(8),
			// 0 ages go to 6, along with 7 ages
			[6] = fish.GetValueOrDefault(0) + fish.GetValueOrDefault(7),
			[5] = fish.GetValueOrDefault(6),
			[4] = fish.GetValueOrDefault(5),
			[3] = fish.GetValueOrDefault(4),
			[2] = fish.GetValueOrDefault(3),
			[1] = fish.GetValueOrDefault(2),
			[0] = fish.GetValueOrDefault(1),
		};

		// run first 80 days
		for (int i = 0; i < 80; i++)
			fish = DayCycle(fish);
		var part1 = fish.Values.Sum().ToString();

		// run 80-256 days
		for (int i = 80; i < 256; i++)
			fish = DayCycle(fish);
		var part2 = fish.Values.Sum().ToString();

		return (part1, part2);
	}
}
