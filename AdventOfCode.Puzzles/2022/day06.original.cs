namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 6, CodeType.Original)]
public partial class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input) =>
		(
			GetIndex(input.Text, 4).ToString(),
			GetIndex(input.Text, 14).ToString()
		);

	private static int GetIndex(string text, int numDistinct) =>
		text.Window(numDistinct)
			.Index()
			.First(x => x.Item.Distinct().Count() == numDistinct)
			.Index + numDistinct;
}
