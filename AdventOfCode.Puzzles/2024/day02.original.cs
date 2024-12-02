namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 02, CodeType.Original)]
public partial class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var reports = input.Lines
			.Select(l => l.Split().Select(int.Parse).ToArray())
			.ToList();

		var part1 = reports.Count(IsSafe).ToString();
		var part2 = reports.Count(IsSafe2).ToString();
		return (part1, part2);
	}

	private static bool IsSafe(int[] levels)
	{
		static bool IsValid(int a, int b) => a - b is >= 1 and <= 3;

		return
			levels.Window(2).All(w => IsValid(w[0], w[1]))
			|| levels.Window(2).All(w => IsValid(w[1], w[0]));
	}

	private static bool IsSafe2(int[] levels)
	{
		for (var i = 0; i < levels.Length; i++)
		{
			if (IsSafe([.. levels[..i], .. levels[(i + 1)..]]))
				return true;
		}

		return false;
	}
}
