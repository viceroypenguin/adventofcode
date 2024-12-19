namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 19, CodeType.Original)]
public partial class Day_19_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var splits = input.Lines.Split(string.Empty).ToList();
		var available = splits[0][0].Split(", ").ToList();

		var possibles = splits[1]
			.Select(p => CountPossible(available, p, []))
			.ToList();

		var part1 = possibles.Count(p => p > 0);
		var part2 = possibles.Sum();

		return (part1.ToString(), part2.ToString());
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
