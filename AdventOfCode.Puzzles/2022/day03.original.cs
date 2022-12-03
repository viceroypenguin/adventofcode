namespace AdventOfCode;

[Puzzle(2022, 3, CodeType.Original)]
public class Day_03_Original : IPuzzle<string[]>
{
	public string[] Parse(PuzzleInput input) => input.Lines;

	public string Part1(string[] input) =>
		input.Select(x => x.Batch(x.Length / 2))
			.Select(x => x.First().Intersect(x.Last()))
			.SelectMany(x => x)
			.Select(x => char.IsLower(x) ? x - 'a' + 1 : x - 'A' + 27)
			.Sum()
			.ToString();

	public string Part2(string[] input) => 
		input.Batch(3)
			.Select(x => x[0].Intersect(x[1]).Intersect(x[2]))
			.SelectMany(x => x)
			.Select(x => char.IsLower(x) ? x - 'a' + 1 : x - 'A' + 27)
			.Sum()
			.ToString();
}
