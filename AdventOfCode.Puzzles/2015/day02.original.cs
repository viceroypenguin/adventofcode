namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 02, CodeType.Original)]
public partial class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var boxes = input.Lines
			.Select(l => BoxRegex().Match(l))
			.Select(m =>
				new[]
				{
					Convert.ToInt32(m.Groups["l"].Value),
					Convert.ToInt32(m.Groups["w"].Value),
					Convert.ToInt32(m.Groups["h"].Value),
				}
				.OrderBy(l => l)
				.ToList())
			.ToList();

		var totalWrappingPaper =
			boxes
				.Select(b => new[] { b[0] * b[1], b[0] * b[2], b[1] * b[2], }.OrderBy(a => a).ToArray())
				.Select(a => (3 * a[0]) + (2 * a[1]) + (2 * a[2]))
				.Sum();

		var totalRibbonLength =
			boxes
				.Select(b => (b[0] * b[1] * b[2]) + (2 * b[0]) + (2 * b[1]))
				.Sum();

		return (
			totalWrappingPaper.ToString(),
			totalRibbonLength.ToString());
	}

	[GeneratedRegex("(?<l>\\d+)x(?<w>\\d+)x(?<h>\\d+)", RegexOptions.Compiled)]
	private static partial Regex BoxRegex();
}
