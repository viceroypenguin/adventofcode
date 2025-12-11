namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 11, CodeType.Original)]
public partial class Day_11_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var tree = input.Lines
			.Select(l => InputRegex.Match(l))
			.Select(m => (
				From: m.Groups["from"].Value,
				To: m.Groups["to"].Captures.Select(c => c.Value).ToList()
			))
			.SelectMany(x => x.To.Select(t => (From: x.From, To: t)))
			.ToLookup(x => x.To, x => x.From);

		var memoize = new Dictionary<string, long>();

		long GetCount(string to)
		{
			if (to == "you")
				return 1;

			if (memoize.TryGetValue(to, out long count))
				return count;

			return memoize[to] = tree[to]
				.Sum(x => GetCount(x));
		}

		var part1 = GetCount("out");

		var memoize2 = new Dictionary<(string, bool, bool), long>();

		long GetCount2(string to, bool hasDac, bool hasFft)
		{
			if (to == "svr")
				return hasFft && hasDac ? 1 : 0;

			if (memoize2.TryGetValue((to, hasDac, hasFft), out long count))
				return count;

			return memoize2[(to, hasDac, hasFft)] = tree[to]
				.Sum(x => GetCount2(x, hasDac || to == "dac", hasFft || to == "fft"));
		}

		var part2 = GetCount2("out", hasDac: false, hasFft: false);

		return (part1.ToString(), part2.ToString());
	}

	[GeneratedRegex(@"^(?<from>\w+):( (?<to>\w+))+$", RegexOptions.ExplicitCapture)]
	private static partial Regex InputRegex { get; }
}
