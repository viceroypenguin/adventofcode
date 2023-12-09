namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 09, CodeType.Original)]
public partial class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var longs = input.Lines
			.Select(l => l.Split().Select(long.Parse).ToList());

		var part1 = longs
			.Select(l => l[^1] + GetNextValue(l))
			.Sum();

		var part2 = longs
			.Select(l => l[0] - GetPreviousValue(l))
			.Sum();

		return (part1.ToString(), part2.ToString());
	}

	private static long GetNextValue(List<long> ints)
	{
		var differences = ints.Window(2)
			.Select(w => w[1] - w[0])
			.ToList();
		if (differences.All(d => d == 0))
			return 0;

		var next = GetNextValue(differences);
		return differences[^1] + next;
	}

	private static long GetPreviousValue(List<long> ints)
	{
		var differences = ints.Window(2)
			.Select(w => w[1] - w[0])
			.ToList();
		if (differences.All(d => d == 0))
			return 0;

		var prev = GetPreviousValue(differences);
		return differences[0] - prev;
	}
}
