namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 2, CodeType.Original)]
public partial class Day_02_Original : IPuzzle
{
	[GeneratedRegex("(?<min>\\d+)-(?<max>\\d+) (?<char>\\w): (?<pass>\\w+)")]
	private static partial Regex PasswordRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = PasswordRegex();
		var matches = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => new
			{
				min = int.Parse(m.Groups["min"].Value),
				max = int.Parse(m.Groups["max"].Value),
				chr = m.Groups["char"].Value[0],
				pass = m.Groups["pass"].Value,
			})
			.ToArray();

		var part1 = matches
			.Where(x => x.pass.Where(c => c == x.chr).CountBetween(x.min, x.max))
			.Count()
			.ToString();

		var part2 = matches
			.Where(x => x.pass[x.min - 1] == x.chr ^ x.pass[x.max - 1] == x.chr)
			.Count()
			.ToString();

		return (part1, part2);
	}
}
