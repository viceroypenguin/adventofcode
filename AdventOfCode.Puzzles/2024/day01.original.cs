namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 01, CodeType.Original)]
public partial class Day_01_Original : IPuzzle
{
	[GeneratedRegex(@"(\d+)\s+(\d+)")]
	private static partial Regex NumbersRegex { get; }

	public (string, string) Solve(PuzzleInput input)
	{
		var list = input.Lines
			.Select(l => NumbersRegex.Match(l))
			.Select(m => new
			{
				int1 = int.Parse(m.Groups[1].Value),
				int2 = int.Parse(m.Groups[2].Value),
			})
			.ToList();

		var list1 = list.Select(l => l.int1).Order().ToList();
		var list2 = list.Select(l => l.int2).Order().ToList();

		var part1 = list1.Zip(list2, (a, b) => Math.Abs(a - b)).Sum().ToString();

		var count = list2.CountBy(x => x).ToDictionary(x => x.Key, x => x.Value);

		var part2 = list.Sum(l => l.int1 * count.GetValueOrDefault(l.int1)).ToString();

		return (part1, part2);
	}
}
