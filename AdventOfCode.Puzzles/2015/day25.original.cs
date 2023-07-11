namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 25, CodeType.Original)]
public partial class Day_25_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		static ulong Step(ulong x) => x * 252533 % 33554393;

		var matches = NumberRegex().Matches(input.Text);
		var row = Convert.ToInt32(matches[0].Value);
		var col = Convert.ToInt32(matches[1].Value);

		static int TotalNums(int n) => n * (n - 1) / 2;

		var stepCount = TotalNums(row + col) - (row - 1);

		var num = 20151125UL;
		foreach (var x in Enumerable.Range(0, stepCount - 1))
			num = Step(num);

		return (num.ToString(), string.Empty);
	}

	[GeneratedRegex("\\d+")]
	private static partial Regex NumberRegex();
}
