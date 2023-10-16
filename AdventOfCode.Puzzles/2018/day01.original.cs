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

		var seen = new HashSet<int>();
		var part2 = changes.Repeat()
			.Scan((acc, next) => acc + next)
			.First(f => !seen.Add(f))
			.ToString();

		return (part1, part2);
	}
}
