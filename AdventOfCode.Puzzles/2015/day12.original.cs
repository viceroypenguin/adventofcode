namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 12, CodeType.Original)]
public partial class Day_12_Original : IPuzzle
{
	[GeneratedRegex("[,:[](-?\\d+)")]
	private static partial Regex NumbersRegex();

	[GeneratedRegex("{[^{}]*(((?<before>{)[^{}]*)+((?<-before>})[^{}]*)+)*(?(before)(?!))[^{}]*:\"red\"[^{}]*(((?<before>{)[^{}]*)+((?<-before>})[^{}]*)+)*(?(before)(?!))[^{}]*}")]
	private static partial Regex JsonRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var str = input.Text;

		var regex = NumbersRegex();
		var partA =
			regex.Matches(str)
				.OfType<Match>()
				.Select(c => c.Groups[1])
				.Select(c => c.Value)
				.Select(c => Convert.ToInt32(c))
				.Sum();

		var redsRegex = JsonRegex();
		str = redsRegex.Replace(str, "");

		var partB =
			regex.Matches(str)
				.OfType<Match>()
				.Select(c => c.Groups[1])
				.Select(c => c.Value)
				.Select(c => Convert.ToInt32(c))
				.Sum();

		return (partA.ToString(), partB.ToString());
	}
}
