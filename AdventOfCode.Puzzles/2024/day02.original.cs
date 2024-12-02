namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 02, CodeType.Original)]
public partial class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = input.Lines
			.Select(l => l.Split().Select(s => int.Parse(s)).ToArray())
			.Count(IsSafe)
			.ToString();

		var part2 = input.Lines
			.Select(l => l.Split().Select(s => int.Parse(s)).ToArray())
			.Count(IsSafe2)
			.ToString();

		return (part1, part2);
	}

	private static bool IsSafe(int[] levels)
	{
		if (levels.Window(2).All(w => w[0] > w[1] && w[0] <= w[1] + 3))
			return true;
		if (levels.Window(2).All(w => w[0] < w[1] && w[0] >= w[1] - 3))
			return true;

		return false;
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
