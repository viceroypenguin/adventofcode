namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var changes = input.Lines
			.Select(l => Convert.ToInt32(l))
			.ToList();

		var part1 = changes.Sum().ToString();

		var part2 = changes.Repeat()
			.Scan((acc, next) => acc + next)
			.Duplicates()
			.First()
			.ToString();

		return (part1, part2);
	}
}
