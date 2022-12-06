namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 5, CodeType.Original)]
public class Day_05_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var ids = input.Lines
			.Select(s => s
				.Select(c => c == 'B' || c == 'R')
				.Select((b, idx) => (b, idx))
				.Aggregate(0, (i, b) => i | ((b.b ? 1 : 0) << (s.Length - b.idx - 1))))
			.OrderBy(x => x)
			.ToArray();

		var part1 = ids.Last().ToString();

		var potentials = ids
			.Window(2)
			.Where(a => a[1] - a[0] == 2)
			.ToArray();

		var part2 = (potentials[0][0] + 1).ToString();

		return (part1, part2);
	}
}
