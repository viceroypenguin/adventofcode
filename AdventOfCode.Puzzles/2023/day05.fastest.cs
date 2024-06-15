using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 05, CodeType.Fastest)]
public sealed partial class Day_05_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;
		span = span["seeds: "u8.Length..];

		var seedLine = span[..(span.IndexOf((byte)'\n') + 1)];
		span = span[(seedLine.Length + 1)..];

		var numSeeds = seedLine.Count((byte)' ') + 1;
		Span<long> seeds = stackalloc long[numSeeds];
		var i = 0;
		while (seedLine.Length > 0)
		{
			var (num, n) = seedLine.AtoL();
			seedLine = seedLine[(n + 1)..];
			seeds[i++] = num;
		}

		var numMapRanges = span.Count((byte)'\n') - 13;
		Span<(long from, long length, long adjust)> mapRanges =
			stackalloc (long from, long length, long adjust)[numMapRanges];
		Span<int> mapRangeIndices = stackalloc int[8];
		mapRangeIndices[7] = numMapRanges;

		var idx = 0;
		for (i = 0; i < 7; i++)
		{
			mapRangeIndices[i] = idx;

			span = span[(span.IndexOf((byte)'\n') + 1)..];
			while (span.Length > 0 && span[0] != '\n')
			{
				var (dest, n) = span.AtoL();
				span = span[(n + 1)..];

				(var from, n) = span.AtoL();
				span = span[(n + 1)..];

				(var length, n) = span.AtoL();
				span = span[(n + 1)..];

				mapRanges[idx++] = (from, from + length - 1, dest - from);
			}

			mapRanges[mapRangeIndices[i]..idx].Sort();

			if (span.Length > 0)
				span = span[1..];
		}

		var part1 = long.MaxValue;
		foreach (var seed in seeds)
		{
			var s = seed;
			for (i = 0; i < 7; i++)
			{
				for (var j = mapRangeIndices[i]; j < mapRangeIndices[i + 1]; j++)
				{
					var (from, to, adjust) = mapRanges[j];
					if (s.Between(from, to))
					{
						s += adjust;
						break;
					}
				}
			}

			if (s < part1)
				part1 = s;
		}

		Span<(long from, long length)> seedRanges = MemoryMarshal.Cast<long, (long, long)>(seeds);
		seedRanges.Sort();

		Span<(long location, long from, long to, int level)> stack =
			stackalloc (long location, long from, long to, int level)[64];

		for (i = 0; i < mapRanges.Length; i++)
		{
			var (from, to, adjust) = mapRanges[i];
			mapRanges[i] = (from + adjust, to + adjust, -adjust);
		}

		for (i = 0; i < 7; i++)
			mapRanges[mapRangeIndices[i]..mapRangeIndices[i + 1]].Sort();

		var stackIdx = 0;
		foreach (var (from, to, adjust) in mapRanges[mapRangeIndices[6]..mapRangeIndices[7]])
			stack[stackIdx++] = (from, from + adjust, to + adjust, 5);
		stack[..stackIdx].Sort();
		stack[..stackIdx].Reverse();

		var part2 = long.MaxValue;
		while (part2 == long.MaxValue)
		{
			var (location, from, to, level) = stack[--stackIdx];

			if (level == -1)
			{
				foreach (var (sFrom, sLength) in seedRanges)
				{
					if (sFrom > to)
						break;
					if (from > sFrom + sLength - 1)
						continue;

					part2 = sFrom < from
						? location
						: location + (sFrom - from);
				}
			}
			else
			{
				var start = stackIdx;

				foreach (var (mFrom, mTo, mAdjust) in mapRanges[mapRangeIndices[level]..mapRangeIndices[level + 1]])
				{
					if (mFrom > to)
					{
						stack[stackIdx++] = (location, from, to, level - 1);
						level = int.MinValue;
						break;
					}

					if (from > mTo)
						continue;

					if (from < mFrom)
					{
						stack[stackIdx++] = (location, from, mFrom - 1, level - 1);
						location += mFrom - from;
						from = mFrom;
					}

					var end = Math.Min(to, mTo);
					stack[stackIdx++] = (location, from + mAdjust, end + mAdjust, level - 1);

					if (end == to)
					{
						level = int.MinValue;
						break;
					}

					location += mTo + 1 - from;
					from = mTo + 1;
				}

				if (level != int.MinValue)
					stack[stackIdx++] = (location, from, to, level - 1);

				stack[start..stackIdx].Reverse();
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
