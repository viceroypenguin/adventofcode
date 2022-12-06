namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 6, CodeType.Original)]
public partial class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var firstWindow = input.Text
			.Window(4)
			.Index()
			.First(x => x.item.Distinct().Count() == 4)
			.index;
		var part1 = (firstWindow + 4).ToString();

		firstWindow = input.Text
			.Window(14)
			.Index()
			.First(x => x.item.Distinct().Count() == 14)
			.index;
		var part2 = (firstWindow + 14).ToString();
		return (part1, part2);
	}
}
