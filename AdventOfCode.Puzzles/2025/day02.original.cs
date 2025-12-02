using System.Numerics;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 02, CodeType.Original)]
public partial class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var ranges = RangeRegex.Matches(input.Text)
			.Select(x => (lo: BigInteger.Parse(x.Groups["lo"].Value), hi: BigInteger.Parse(x.Groups["hi"].Value)))
			.ToList();

		var part1 = ranges
			.SelectMany(x => SuperEnumerable.Generate(x.lo, x => x + 1).TakeWhile(y => y <= x.hi))
			.Select(x => x.ToString())
			.Where(x => x[..(x.Length / 2)] == x[(x.Length / 2)..])
			.Select(BigInteger.Parse)
			.Aggregate(BigInteger.Zero, (a, b) => a + b);

		var part2 = ranges
			.SelectMany(x => SuperEnumerable.Generate(x.lo, x => x + 1).TakeWhile(y => y <= x.hi))
			.Where(x => InvalidPart2IdRegex.IsMatch(x.ToString()))
			.Aggregate(BigInteger.Zero, (a, b) => a + b);

		return (part1.ToString(), part2.ToString());
	}

	[GeneratedRegex(@"(?<lo>\d+)-(?<hi>\d+)")]
	private static partial Regex RangeRegex { get; }

	[GeneratedRegex(@"^(\d+)(\1)+$")]
	private static partial Regex InvalidPart2IdRegex { get; }
}
