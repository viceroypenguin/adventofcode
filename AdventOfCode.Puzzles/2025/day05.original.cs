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
			.Select(m => (lo: long.Parse(m.Groups[1].Value), hi: long.Parse(m.Groups[2].Value)))
			.ToList();

		var part1 = parts[1]
			.Select(long.Parse)
			.Count(i => idRanges.Exists(r => i.Between(r.lo, r.hi)));

		var (part2, _) = idRanges
			.Order()
			.Aggregate(
				(sum: 0L, max: long.MinValue),
				(x, y) =>
				{
					var ((sum, max), (lo, hi)) = (x, y);

					if (max >= lo)
						lo = max + 1;

					if (max >= hi)
						hi = max;

					sum += hi - lo + 1;
					return (sum, hi);
				}
			);

		return (part1.ToString(), part2.ToString());
	}
}
