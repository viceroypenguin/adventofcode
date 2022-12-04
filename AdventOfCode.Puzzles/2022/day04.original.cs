using System.Text.RegularExpressions;

namespace AdventOfCode;

[Puzzle(2022, 4, CodeType.Original)]
public partial class Day_04_Original : IPuzzle
{
	[GeneratedRegex("(\\d+)-(\\d+),(\\d+)-(\\d+)")]
	private static partial Regex RangeRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = RangeRegex();
		var ranges = input.Lines
			.Select(x => regex.Match(x))
			.Select(m => (
				int.Parse(m.Groups[1].Value),
				int.Parse(m.Groups[2].Value),
				int.Parse(m.Groups[3].Value),
				int.Parse(m.Groups[4].Value)))
			.Select(x => (
				Enumerable.Range(x.Item1, x.Item2 - x.Item1 + 1).ToList(),
				Enumerable.Range(x.Item3, x.Item4 - x.Item3 + 1).ToList()));

		var part1 = ranges
			.Where(x =>
			{
				var intersect = x.Item1.Intersect(x.Item2).ToList();
				return intersect.CollectionEqual(x.Item1)
					|| intersect.CollectionEqual(x.Item2);
			})
			.Count()
			.ToString();

		var part2 = ranges
			.Where(x => x.Item1.Intersect(x.Item2).Any())
			.Count()
			.ToString();

		return (part1, part2);
	}
}
