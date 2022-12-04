namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var elves = input.Lines
			.Split(string.Empty)
			.Select(g => g.Select(int.Parse).Sum())
			.ToList();

		var part1 = elves.Max().ToString();
		var part2 = elves.PartialSort(3, OrderByDirection.Descending)
			.Sum()
			.ToString();

		return (part1, part2);
	}
}
