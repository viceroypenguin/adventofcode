namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 04, CodeType.Original)]
public partial class Day_04_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var games = input.Lines
			.Select(l => l.Split('|', ':'))
			.Select((s, i) => s[2]
				.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(int.Parse)
				.Intersect(
					s[1]
						.Split(' ', StringSplitOptions.RemoveEmptyEntries)
						.Select(int.Parse))
				.Count())
			.ToList();

		var part1 = games
			.Select(x => (long)Math.Pow(2, x - 1))
			.Sum();

		var counts = games.Select(x => 1).ToList();
		for (var i = 0; i < games.Count; i++)
		{
			for (int j = i + 1, n = 0; j < counts.Count && n < games[i]; j++, n++)
				counts[j] += counts[i];
		}

		var part2 = counts.Sum();

		return (part1.ToString(), part2.ToString());
	}
}
