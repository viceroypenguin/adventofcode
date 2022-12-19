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
			.ScanEx((acc, next) => acc + next)
			.First(f =>
			{
				if (seen.Contains(f))
					return true;
				seen.Add(f);
				return false;
			})
			.ToString();

		return (part1, part2);
	}
}
