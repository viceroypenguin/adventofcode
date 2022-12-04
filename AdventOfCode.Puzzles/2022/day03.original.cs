namespace AdventOfCode;

[Puzzle(2022, 3, CodeType.Original)]
public class Day_03_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = input.Lines
			.Select(x => x.Batch(x.Length / 2))
			.Select(x => x.First().Intersect(x.Last()))
			.SelectMany(x => x)
			.Select(x => char.IsLower(x) ? x - 'a' + 1 : x - 'A' + 27)
			.Sum()
			.ToString();

		var part2 = input.Lines
			.Batch(3)
			.Select(x => x[0].Intersect(x[1]).Intersect(x[2]))
			.SelectMany(x => x)
			.Select(x => char.IsLower(x) ? x - 'a' + 1 : x - 'A' + 27)
			.Sum()
			.ToString();

		return (part1, part2);
	}
}
