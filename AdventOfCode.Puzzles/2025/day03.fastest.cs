using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 03, CodeType.Fastest)]
public partial class Day_03_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0L;
		var part2 = 0L;

		var span = input.Span;
		foreach (var line in input.Span.EnumerateLines())
		{
			if (line.IsEmpty)
				break;

			part1 += GetMaxNumberOfLength(line, 2);
			part2 += GetMaxNumberOfLength(line, 12);
		}

		return (part1.ToString(), part2.ToString());
	}

	[SkipLocalsInit]
	private static long GetMaxNumberOfLength(ReadOnlySpan<byte> line, int length)
	{
		Span<byte> stack = stackalloc byte[16];
		var index = 0;

		for (var i = 0; i < line.Length; i++)
		{
			var ch = line[i];

			while (
				index > 0
				&& stack[index - 1] < ch
				&& index + (line.Length - i) > length
			)
			{
				index--;
			}

			if (index < length)
				stack[index++] = ch;
		}

		return long.Parse(stack[..index]);
	}
}
