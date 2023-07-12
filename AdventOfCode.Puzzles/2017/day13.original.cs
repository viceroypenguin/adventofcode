namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 13, CodeType.Original)]
public partial class Day_13_Original : IPuzzle
{
	[GeneratedRegex("^(?<depth>\\d+): (?<range>\\d+)$", RegexOptions.Compiled)]
	private static partial Regex TargetRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = TargetRegex();
		var depths = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => new
			{
				depth = Convert.ToInt32(m.Groups["depth"].Value),
				range = Convert.ToInt32(m.Groups["range"].Value),
			})
			.ToList();

		var partA =
			depths
				.Where(f => f.depth % ((f.range - 1) * 2) == 0)
				.Select(f => f.depth * f.range)
				.Sum();

		var partB =
			Enumerable.Range(0, int.MaxValue)
				.Select(i =>
				{
					var any = depths
						.Any(f => (f.depth + i) % ((f.range - 1) * 2) == 0);
					return (i, any);
				})
				.First(x => !x.any)
				.i;

		return (partA.ToString(), partB.ToString());
	}
}
