namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 06, CodeType.Original)]
public partial class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var times = input.Lines[0][12..].Split([" "], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
		var distances = input.Lines[1][12..].Split([" "], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

		var part1 = times.Zip(distances)
			.Select(x =>
				Enumerable.Range(1, x.First)
					.Select(i => i * (x.First - i))
					.Count(i => i > x.Second))
			.Aggregate(1L, (a, b) => a * b);

		var time = int.Parse(string.Concat(input.Lines[0][12..].Split([" "], StringSplitOptions.RemoveEmptyEntries)));
		var distance = long.Parse(string.Concat(input.Lines[1][12..].Split([" "], StringSplitOptions.RemoveEmptyEntries)));

		var part2 = Enumerable.Range(1, time)
			.Select(i => (long)i * (time - i))
			.Count(i => i > distance);

		return (part1.ToString(), part2.ToString());
	}
}
