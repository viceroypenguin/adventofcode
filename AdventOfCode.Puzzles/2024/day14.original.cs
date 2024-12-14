namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 14, CodeType.Original)]
public partial class Day_14_Original : IPuzzle
{
	[GeneratedRegex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)")]
	private static partial Regex RobotRegex { get; }

	public (string, string) Solve(PuzzleInput input)
	{
		var robots = input.Lines
			.Select(l => RobotRegex.Match(l))
			.Select(m =>
				(
					px: int.Parse(m.Groups[1].Value),
					py: int.Parse(m.Groups[2].Value),
					vx: int.Parse(m.Groups[3].Value) + 101,
					vy: int.Parse(m.Groups[4].Value) + 103
				)
			)
			.ToArray();

		var part1 = robots
			.Select(r =>
				(
					x: (r.px + (r.vx * 100)) % 101,
					y: (r.py + (r.vy * 100)) % 103
				)
			)
			.GroupBy(
				r => (r.x, r.y) switch
				{
					( < 50, < 51) => 1,
					( > 50, < 51) => 2,
					( < 50, > 51) => 3,
					( > 50, > 51) => 4,
					_ => 5,
				},
				(k, g) => (k, c: g.Count())
			)
			.Where(x => x.k < 5)
			.Aggregate(1L, (a, b) => a * b.c);

		var part2 = 0;
		while (true)
		{
			if (robots.CountBy(r => (r.px, r.py)).All(k => k.Value == 1))
				break;

			part2++;
			for (var i = 0; i < robots.Length; i++)
			{
				ref var r = ref robots[i];
				r = (px: (r.px + r.vx) % 101, py: (r.py + r.vy) % 103, r.vx, r.vy);
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
