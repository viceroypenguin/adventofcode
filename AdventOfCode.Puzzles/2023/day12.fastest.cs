namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 12, CodeType.Fastest)]
public sealed partial class Day_12_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<long> memoization = stackalloc long[4096];
		Span<byte> springs = stackalloc byte[128];
		Span<int> groups = stackalloc int[32];

		var part1 = 0L;
		var part2 = 0L;
		foreach (var l in input.Span.EnumerateLines())
		{
			if (l.Length == 0)
				break;

			var span = l;

			var n = span.IndexOf((byte)' ');
			var lSprings = span[..n];

			span = span[n..];
			var i = 0;
			while (span.Length > 1)
			{
				span = span[1..];
				(groups[i++], n) = span.AtoI();
				span = span[n..];
			}

			var lGroups = groups[..i];

			memoization.Fill(-1);
			part1 += ProcessLine(lSprings, lGroups, memoization);

			springs.Fill((byte)'?');
			n = lSprings.Length + 1; // leave the `?` gap between sets
			lSprings.CopyTo(springs);
			lSprings.CopyTo(springs[n..]);
			lSprings.CopyTo(springs[(n * 2)..]);
			lSprings.CopyTo(springs[(n * 3)..]);
			lSprings.CopyTo(springs[(n * 4)..]);

			lGroups.CopyTo(groups[i..]);
			lGroups.CopyTo(groups[(i * 2)..]);
			lGroups.CopyTo(groups[(i * 3)..]);
			lGroups.CopyTo(groups[(i * 4)..]);

			memoization.Fill(-1);
			part2 += ProcessLine(springs[..((n * 5) - 1)], groups[..(i * 5)], memoization);
		}

		return (part1.ToString(), part2.ToString());
	}

	private static long ProcessLine(ReadOnlySpan<byte> springs, ReadOnlySpan<int> groups, Span<long> memoization)
	{
		var memoIdx = (springs.Length * 32) + groups.Length;
		if (memoization[memoIdx] >= 0)
			return memoization[memoIdx];

		if (springs.Length == 0)
		{
			return memoization[memoIdx] =
				groups.Length == 0 ? 1 : 0;
		}

		if (groups.Length == 0)
		{
			return memoization[memoIdx] =
				springs.IndexOf((byte)'#') >= 0 ? 0 : 1;
		}

		while (springs.Length > 0)
		{
			var n = springs.IndexOfAnyExcept((byte)'.');
			if (n > 0)
			{
				springs = springs[n..];
				continue;
			}

			var count = 0L;
			if (springs[0] == '?')
			{
				count += ProcessLine(springs[1..], groups, memoization);
			}

			// either a `#` (so return 0) or assume all remaining `?` are `.`
			if (groups.Length == 0
				// not enough room for group of `#` in what's left
				|| groups[0] > springs.Length
				// `.` breaking up the group
				|| springs[..groups[0]].IndexOf((byte)'.') >= 0
				// no `.` between this group and the next
				|| (springs.Length > groups[0] && springs[groups[0]] == '#'))
			{
				return memoization[memoIdx] = count;
			}

			count += ProcessLine(
				springs[Math.Min(springs.Length, groups[0] + 1)..],
				groups[1..],
				memoization
			);

			return memoization[memoIdx] = count;
		}

		return groups.Length == 0 ? 1 : 0;
	}
}
