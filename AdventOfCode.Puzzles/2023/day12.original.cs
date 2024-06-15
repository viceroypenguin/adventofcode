namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 12, CodeType.Original)]
public partial class Day_12_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Lines
			.Select(l => l.Split())
			.Select(l => (report: l[0], conditions: l[1].Split(',').Select(int.Parse).ToArray()))
			.ToList();

		var part1 = data
			.Select(l => GetArrangementsCount(l.report, l.conditions))
			.Sum();

		var part2 = data
			.Select(l => GetArrangementsCount(
				string.Join('?', Enumerable.Repeat(l.report, 5)),
				l.conditions.Repeat(5).ToArray()))
			.Sum();

		return (part1.ToString(), part2.ToString());
	}

	private static long GetArrangementsCount(ReadOnlySpan<char> springs, int[] conditions)
	{
		return GetArrangementsCount(springs, 0, conditions, 0, []);
	}

	private static long GetArrangementsCount(ReadOnlySpan<char> array, int mIndex, Span<int> counts, int cIndex, Dictionary<(int, int), long> memo)
	{
		if (memo.TryGetValue((mIndex, cIndex), out var cnt))
		{
			return cnt;
		}

		if (mIndex >= array.Length)
			return counts.Length == 0 ? 1 : 0;

		else if (counts.Length == 0)
			return memo[(mIndex, cIndex)] = array[mIndex..].IndexOf('#') >= 0 ? 0 : 1;

		for (int mi = mIndex, ci = cIndex; mi < array.Length; mi++)
		{
			// blanks are good; we don't care about them
			var n = array[mi..].IndexOfAnyExcept('.');
			if (n >= 0)
				mi += n;

			if (array[mi] == '#')
			{
				// found another set, but no counts for it. gtfo
				if (counts.Length == 0)
					return memo[(mIndex, cIndex)] = 0;

				// not enough remaining chars to possibly match
				if (counts[0] > array.Length - mi)
					return memo[(mIndex, cIndex)] = 0;

				n = array[mi..].IndexOfAnyExcept('#');

				// perfect match, go to the next condition
				if (n == counts[0]
					|| (n == -1 && array[mi..].Length == counts[0]))
				{
					// skip the following char; either a `.` or a `?`,
					// but `?` must be `.` to satisfy so skip anyway
					mi += counts[0];
					counts = counts[1..];
					ci++;
					continue;
				}

				// too long for current count; back out and try again
				if (n > counts[0])
					return memo[(mIndex, cIndex)] = 0;

				// try going forward - if any `.`, then gtfo
				var variance = counts[0] - n;
				var tmp = array.Slice(mi + n, variance);
				if (tmp.IndexOf('.') >= 0)
					return memo[(mIndex, cIndex)] = 0;

				// made it far enough, but next char can't be '#' or count is invalid
				if (mi + n + variance < array.Length
					&& array[mi + n + variance] == '#')
				{
					return memo[(mIndex, cIndex)] = 0;
				}

				// only way for the current arrangement to work, so calculate what's left and gtfo
				var count = GetArrangementsCount(array, mi + n + variance + 1, counts[1..], ci + 1, memo);
				return memo[(mIndex, cIndex)] = count;
			}

			// got ourselves a `?` try both ways and see what works...
			else
			{
				// start with a blank
				var count = GetArrangementsCount(array, mi + 1, counts, ci, memo);

				if (counts.Length == 0)
					return memo[(mIndex, cIndex)] = count;

				if (mi + counts[0] > array.Length)
					return memo[(mIndex, cIndex)] = count;

				// then try a `#`
				var tmp = array.Slice(mi, counts[0]);
				// ok, can't do `#`, so return the current possibility
				if (tmp.IndexOf('.') >= 0)
					return memo[(mIndex, cIndex)] = count;

				// made it far enough, but next char can't be '#' or count is invalid
				if (mi + counts[0] < array.Length
					&& array[mi + counts[0]] == '#')
				{
					return memo[(mIndex, cIndex)] = count;
				}

				// add the next count
				count += GetArrangementsCount(array, mi + counts[0] + 1, counts[1..], ci + 1, memo);

				// return what we got
				return memo[(mIndex, cIndex)] = count;
			}
		}

		// ended string, but still a counts left. gtfo
		if (counts.Length != 0)
			return 0;

		// successful match
		return 1;
	}
}
