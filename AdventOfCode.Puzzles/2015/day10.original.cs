namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 10, CodeType.Original)]
public class Day_10_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var line = input.Lines[0];

		line = SuperEnumerable
			.Generate(
				line,
				Transform)
			.ElementAt(40);
		var partA = line.Length;

		line = SuperEnumerable
			.Generate(
				line,
				Transform)
			.ElementAt(10);
		var partB = line.Length;

		return (partA.ToString(), partB.ToString());

		static string Transform(string str) =>
			string.Join(
				"",
				str
					.RunLengthEncode()
					.Select(rle => $"{rle.count}{rle.value}"));
	}
}
