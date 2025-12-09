namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 09, CodeType.Original)]
public partial class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var coordinates = input.Lines
			.Select(l => CoordinateRegex.Match(l))
			.Select(m => (
				x: int.Parse(m.Groups["x"].Value),
				y: int.Parse(m.Groups["y"].Value)
			))
			.ToList();

		var boxes = coordinates
			.SelectMany(
				a => coordinates.Where(b => a.x != b.x || a.y != b.y),
				(a, b) => (a, b, area: (long)(Math.Abs(a.x - b.x) + 1) * (Math.Abs(a.y - b.y) + 1))
			)
			.OrderByDescending(x => x.area)
			.TakeEvery(2)
			.ToList();

		var part1 = boxes[0].area;

		var part2 = boxes
			.First(b => IsValid(b, coordinates))
			.area;

		return (part1.ToString(), part2.ToString());
	}

	private static bool IsValid(
		((int x, int y) a, (int x, int y) b, long area) box,
		List<(int x, int y)> coordinates
	)
	{
		var (a, b, _) = box;

		foreach (var (p, q) in coordinates.Lead(1, coordinates[0], (p, q) => (p, q)))
		{
			if (p.x == q.x)
			{
				if (
					p.x.Between(Math.Min(a.x, b.x) + 1, Math.Max(a.x, b.x) - 1)
					&& Math.Min(a.y, b.y) < Math.Max(p.y, q.y)
					&& Math.Max(a.y, b.y) > Math.Min(p.y, q.y)
				)
				{
					return false;
				}
			}
			else
			{
				if (
					p.y.Between(Math.Min(a.y, b.y) + 1, Math.Max(a.y, b.y) - 1)
					&& Math.Min(a.x, b.x) < Math.Max(p.x, q.x)
					&& Math.Max(a.x, b.x) > Math.Min(p.x, q.x)
				)
				{
					return false;
				}
			}
		}

		return true;
	}

	[GeneratedRegex(@"^(?<x>\d+),(?<y>\d+)$")]
	private static partial Regex CoordinateRegex { get; }
}
