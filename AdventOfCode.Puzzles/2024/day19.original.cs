namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 19, CodeType.Original)]
public partial class Day_19_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var splits = input.Lines.Split(string.Empty).ToList();
		var available = splits[0][0].Split(", ").ToList();

		var part1 = splits[1]
			.Count(p => IsPossible(available, p, []));

		var part2 = splits[1]
			.Sum(p => CountPossible(available, p, []));

		return (part1.ToString(), part2.ToString());
	}

	private static bool IsPossible(List<string> available, ReadOnlySpan<char> span, Dictionary<string, bool> cache)
	{
		if (span is "")
			return true;

		if (cache.GetAlternateLookup<ReadOnlySpan<char>>().TryGetValue(span, out var result))
			return result;

		foreach (var a in available)
		{
			if (span.StartsWith(a) && IsPossible(available, span[a.Length..], cache))
				return cache[span.ToString()] = true;
		}

		return cache[span.ToString()] = false;
	}

	private static long CountPossible(List<string> available, ReadOnlySpan<char> span, Dictionary<string, long> cache)
	{
		if (span is "")
			return 1;

		if (cache.GetAlternateLookup<ReadOnlySpan<char>>().TryGetValue(span, out var result))
			return result;

		var sum = 0L;
		foreach (var a in available)
		{
			if (span.StartsWith(a))
				sum += CountPossible(available, span[a.Length..], cache);
		}

		return cache[span.ToString()] = sum;
	}
}
