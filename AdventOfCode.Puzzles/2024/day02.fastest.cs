using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 02, CodeType.Fastest)]
public partial class Day_02_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;

		Span<int> numbers = new int[16];
		Span<int> scratch = new int[16];

		foreach (var line in input.Span[..^1].EnumerateLines())
		{
			var idx = 0;
			var n = 0;
			foreach (var c in line)
			{
				if (c is (byte)' ')
				{
					numbers[idx++] = n;
					n = 0;
				}
				else
				{
					n = (n * 10) + (c - '0');
				}
			}

			numbers[idx++] = n;
			var levels = numbers[..idx];

			var isSafe1 = IsSafe1(levels);
			part1 += isSafe1;
			part2 += isSafe1 == 1 ? 1 : IsSafe2(levels, scratch);
		}

		return (part1.ToString(), part2.ToString());
	}

	private static int IsSafe1(Span<int> levels)
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool IsValid(int a, int b) => a - b is >= 1 and <= 3;

		if (levels[1] >= levels[0])
		{
			for (var i = 0; i < levels.Length - 1; i++)
			{
				if (!IsValid(levels[i + 1], levels[i]))
					return 0;
			}
		}
		else
		{
			for (var i = 0; i < levels.Length - 1; i++)
			{
				if (!IsValid(levels[i], levels[i + 1]))
					return 0;
			}
		}

		return 1;
	}

	private static int IsSafe2(Span<int> levels, Span<int> scratch)
	{
		var newLevels = scratch[..(levels.Length - 1)];

		for (var i = 0; i < levels.Length; i++)
		{
			levels[..i].CopyTo(newLevels);
			levels[(i + 1)..].CopyTo(newLevels[i..]);

			if (IsSafe1(newLevels) == 1)
				return 1;
		}

		return 0;
	}
}
