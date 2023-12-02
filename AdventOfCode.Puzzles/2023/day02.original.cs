namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 02, CodeType.Original)]
public partial class Day_02_Original : IPuzzle
{
	[GeneratedRegex(@"^Game (?<gameid>\d+): ((?<sets>[^;]*)(; )?)*$", RegexOptions.ExplicitCapture)]
	private static partial Regex GameRegex();
	[GeneratedRegex(@"(\d+) (\w+)")]
	private static partial Regex SetRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var games = input.Lines
			.Select(l => GameRegex().Match(l))
			.Select(m => new
			{
				id = int.Parse(m.Groups["gameid"].Value),
				sets = m.Groups["sets"]
					.Captures
					.Select(c => SetRegex().Matches(c.Value)
						.OfType<Match>()
						.Select(m => (num: int.Parse(m.Groups[1].Value), Color: m.Groups[2].Value))
						.GroupBy(
							x => x.Color,
							(k, g) => (color: k, num: g.Sum(x => x.num)))
						.ToDictionary(x => x.color, x => x.num))
					.ToList(),
			})
			.ToList();

		var part1 = games
			.Where(g => g.sets.All(s =>
				s.GetValueOrDefault("red") <= 12
				&& s.GetValueOrDefault("green") <= 13
				&& s.GetValueOrDefault("blue") <= 14))
			.Sum(g => g.id)
			.ToString();

		var part2 = games
			.Select(g => g.sets.Max(s => s.GetValueOrDefault("red", 1)) *
				g.sets.Max(s => s.GetValueOrDefault("green", 1)) *
				g.sets.Max(s => s.GetValueOrDefault("blue", 1)))
			.Sum()
			.ToString();

		return (part1, part2);
	}
}
