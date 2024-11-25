namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 08, CodeType.Original)]
public partial class Day_08_Original : IPuzzle
{
	private static int GetDecodedLength(string str) =>
		DecodeRegex()
			.Match(str)
			.Groups["char"]
			.Captures
			.Count;

	public (string, string) Solve(PuzzleInput input)
	{
		var lines = input.Lines;
		var inputLength = lines.Select(s => s.Length).Sum();

		var decodedLength = lines.Select(GetDecodedLength).Sum();
		var partA = inputLength - decodedLength;

		var encodedVariance = lines
			.Select(s => s.Count(c => c is '\\' or '\"') + 2)
			.Sum();
		var partB = encodedVariance;

		return (partA.ToString(), partB.ToString());
	}

	[GeneratedRegex(@"""(?<char>\\x.{2}|\\\\|\\\""|\w)*""", RegexOptions.ExplicitCapture)]
	private static partial Regex DecodeRegex();
}
