using System.Numerics;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 02, CodeType.Original)]
public partial class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var ranges = RangeRegex.Matches(input.Text)
			.Select(x => (lo: long.Parse(x.Groups["lo"].Value), hi: long.Parse(x.Groups["hi"].Value)))
			.ToList();

		var part1 = ranges
			.SelectMany(x => SuperEnumerable.Generate(x.lo, x => x + 1).TakeWhile(y => y <= x.hi))
			.Where(x => InvalidPart1IdRegex.IsMatch(x.ToString()))
			.Sum();

		var part2 = ranges
			.SelectMany(x => SuperEnumerable.Generate(x.lo, x => x + 1).TakeWhile(y => y <= x.hi))
			.Where(x => InvalidPart2IdRegex.IsMatch(x.ToString()))
			.Sum();

		return (part1.ToString(), part2.ToString());
	}

	[GeneratedRegex(@"(?<lo>\d+)-(?<hi>\d+)")]
	private static partial Regex RangeRegex { get; }

	[GeneratedRegex(@"^(\d+)(\1)$")]
	private static partial Regex InvalidPart1IdRegex { get; }

	[GeneratedRegex(@"^(\d+)(\1)+$")]
	private static partial Regex InvalidPart2IdRegex { get; }
}
