namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 05, CodeType.Fastest)]
public sealed partial class Day_05_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;
		span = span.Slice("seeds: "u8.Length);

		var seedLine = span.Slice(0, span.IndexOf((byte)'\n') + 1);
		span = span.Slice(seedLine.Length + 1);

		var numSeeds = seedLine.Count((byte)' ') + 1;
		Span<long> seeds = stackalloc long[numSeeds];
		var i = 0;
		while (seedLine.Length > 0)
		{
			var (num, n) = seedLine.AtoL();
			seedLine = seedLine.Slice(n + 1);
			seeds[i++] = num;
		}

		var numMapRanges = span.Count((byte)'\n') - 13;
		Span<(long from, long to, long adjust)> mapRanges =
			stackalloc (long from, long to, long adjust)[numMapRanges];
		Span<int> mapRangeIndices = stackalloc int[8];
		mapRangeIndices[7] = numMapRanges;

		var idx = 0;
		for (i = 0; i < 7; i++)
		{
			mapRangeIndices[i] = idx;

			span = span.Slice(span.IndexOf((byte)'\n') + 1);
			while (span.Length > 0 && span[0] != '\n')
			{
				var (dest, n) = span.AtoL();
				span = span.Slice(n + 1);

				(var from, n) = span.AtoL();
				span = span.Slice(n + 1);

				(var to, n) = span.AtoL();
				span = span.Slice(n + 1);

				mapRanges[idx++] = (from, from + to - 1, dest - from);
			}

			mapRanges[mapRangeIndices[i]..idx].Sort();

			if (span.Length > 0)
				span = span.Slice(1);
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

		var part2 = long.MaxValue;

		Span<(long from, long to)> seedRanges = stackalloc (long from, long to)[64];
		Span<(long from, long to)> nextSeedRanges = stackalloc (long from, long to)[64];

		for (var j = 0; j < seeds.Length; j += 2)
		{
			var seedRangeCount = 0;
			seedRanges[seedRangeCount++] = (seeds[j], seeds[j + 1] + seeds[j] - 1);

			for (i = 0; i < 7; i++)
			{
				var nextCount = 0;

				foreach (var r in seedRanges[..seedRangeCount])
				{
					var (from, to) = r;
					foreach (var mapRange in mapRanges[mapRangeIndices[i]..mapRangeIndices[i + 1]])
					{
						if (from > mapRange.to)
							continue;

						if (to < mapRange.from)
						{
							nextSeedRanges[nextCount++] = (from, to);
							(from, to) = (0, 0);
							break;
						}

						if (from < mapRange.from)
						{
							nextSeedRanges[nextCount++] = (
								from + mapRange.adjust, mapRange.from - 1 + mapRange.adjust);
							from = mapRange.from;
						}

						var end = Math.Min(to, mapRange.to);
						nextSeedRanges[nextCount++] = (
							from + mapRange.adjust, end + mapRange.adjust);

						if (to > mapRange.to)
						{
							from = mapRange.to + 1;
						}
						else
						{
							(from, to) = (0, 0);
							break;
						}
					}

					if (from != 0 && to != 0)
						nextSeedRanges[nextCount++] = (from, to);
				}

				nextSeedRanges[..nextCount].Sort();

				var tmp = seedRanges;
				seedRanges = nextSeedRanges;
				nextSeedRanges = tmp;

				seedRangeCount = nextCount;
			}

			var s = seedRanges[0].from;
			if (s < part2)
				part2 = s;
		}

		return (part1.ToString(), part2.ToString());
	}

	private static (long from, long to, long adjust) BinarySearchMapRanges(
		ReadOnlySpan<(long from, long to, long adjust)> ranges,
		long value)
	{
		var lo = 0;
		var hi = ranges.Length - 1;
		while (lo <= hi)
		{
			var mid = lo + ((hi - lo) / 2);

			var (from, to, _) = ranges[mid];
			if (value.Between(from, to))
				return ranges[mid];

			if (to < value)
				lo = mid + 1;
			else
				hi = mid - 1;
		}

		return (0, long.MaxValue, 0);
	}
}
