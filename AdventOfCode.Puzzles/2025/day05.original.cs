namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 05, CodeType.Original)]
public partial class Day_05_Original : IPuzzle
{
	[GeneratedRegex(@"(?<lo>\d+)-(?<hi>\d+)")]
	private static partial Regex RangeRegex { get; }

	public (string, string) Solve(PuzzleInput input)
	{
		var parts = input.Lines.Split(string.Empty).ToArray();

		var idRanges = parts[0]
			.Select(l => RangeRegex.Match(l))
			.Select(m => (lo: long.Parse(m.Groups["lo"].Value), hi: long.Parse(m.Groups["hi"].Value)))
			.Order()
			.Merge()
			.ToList();

		var part1 = parts[1]
			.Select(long.Parse)
			.Count(
				i => idRanges.BinarySearch(
					(i, long.MinValue),
					Comparer<(long Lo, long Hi)>.Create(CompareIdToRange)
				) >= 0
			);

		var part2 = idRanges.Sum(x => x.Hi - x.Lo + 1);

		return (part1.ToString(), part2.ToString());
	}

	private static int CompareIdToRange((long Lo, long Hi) x, (long Lo, long Hi) y)
	{
		var id = y.Lo;
		if (id < x.Lo)
			return 1;
		if (id > x.Hi)
			return -1;
		return 0;
	}
}

file static class Extensions
{
	public static IEnumerable<(long Lo, long Hi)> Merge(
		this IEnumerable<(long Lo, long Hi)> source
	)
	{
		using var enumerator = source.GetEnumerator();

		if (!enumerator.MoveNext())
			yield break;

		var (lo, hi) = enumerator.Current;

		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Lo > hi + 1)
			{
				yield return (lo, hi);
				(lo, hi) = enumerator.Current;
				continue;
			}

			hi = Math.Max(hi, enumerator.Current.Hi);
		}

		yield return (lo, hi);
	}
}
