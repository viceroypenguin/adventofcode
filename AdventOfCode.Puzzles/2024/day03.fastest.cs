using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 03, CodeType.Fastest)]
public partial class Day_03_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;

		var span = input.Span;
		var nextIdx = span.IndexOfAny((byte)'d', (byte)'m');
		var enabled = true;

		while (nextIdx >= 0)
		{
			span = span[nextIdx..];

			if (span[..4].SequenceEqual("do()"u8))
			{
				span = span[4..];
				enabled = true;
			}
			else if (span[..7].SequenceEqual("don't()"u8))
			{
				span = span[7..];
				enabled = false;
			}
			else if (span[..4].SequenceEqual("mul("u8))
			{
				var idx = 4;
				var result = ParseMul(span, ref idx);
				span = span[idx..];

				part1 += result;
				if (enabled)
					part2 += result;
			}
			else
			{
				span = span[1..];
			}

			nextIdx = span.IndexOfAny((byte)'d', (byte)'m');
		}

		return (part1.ToString(), part2.ToString());
	}

	private static int ParseMul(ReadOnlySpan<byte> span, ref int i)
	{
		var num1 = ParseNumber(span, ref i);
		if (span[i - 1] != ',')
			return 0;

		var num2 = ParseNumber(span, ref i);
		if (span[i - 1] != ')')
			return 0;

		return num1 * num2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int ParseNumber(ReadOnlySpan<byte> input, ref int i)
	{
		var a = 0;
		var c = 0;
		while (i < input.Length && (c = input[i++]) is >= (byte)'0' and <= (byte)'9')
			a = (a * 10) + (c - '0');
		return a;
	}
}
