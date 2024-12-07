using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 05, CodeType.Fastest)]
public partial class Day_05_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<ulong> orderingRules = stackalloc ulong[2 * 100];
		var span = input.Span;

		while (span[0] != '\n')
		{
			var before = ((span[0] - '0') * 10) + (span[1] - '0');
			var after = ((span[3] - '0') * 10) + (span[4] - '0');

			orderingRules.SetOrder(before, after);

			span = span[6..];
		}

		var part1 = 0;
		var part2 = 0;
		Span<int> pages = stackalloc int[24];
		Span<ulong> pagesBitArray = stackalloc ulong[2];

		span = span[1..];
		while (span.Length > 0)
		{
			pagesBitArray.Clear();
			var numberOfPages = 0;
			while (true)
			{
				var num = pages[numberOfPages++] = ((span[0] - '0') * 10) + (span[1] - '0');
				pagesBitArray[num / 64] |= 1ul << (num % 64);

				var ch = span[2];
				span = span[3..];

				if (ch == '\n')
					break;
			}

			var flag = true;
			var medianValue = 0;
			for (var i = 0; i < numberOfPages; i++)
			{
				var after = pages[i];

				// count how many items _should_ be before this current number
				var pagesBefore =
					BitOperations.PopCount(pagesBitArray[0] & orderingRules[after * 2])
					+ BitOperations.PopCount(pagesBitArray[1] & orderingRules[after * 2 + 1]);

				flag = flag && pagesBefore == i;
				if (pagesBefore == numberOfPages / 2)
					medianValue = after;
			}

			if (flag)
				part1 += medianValue;
			else
				part2 += medianValue;
		}

		return (part1.ToString(), part2.ToString());
	}
}

file static class Extensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetOrder(this Span<ulong> orderingRules, int before, int after)
	{
		var bitLocation = (after * 2) + (before / 64);
		var index = before % 64;
		orderingRules[bitLocation] |= 1ul << index;
	}
}
