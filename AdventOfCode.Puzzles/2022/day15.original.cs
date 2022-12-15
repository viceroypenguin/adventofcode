namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 15, CodeType.Original)]
public partial class Day_15_Original : IPuzzle
{
	[GeneratedRegex("Sensor at x=(?<sx>-?\\d+), y=(?<sy>-?\\d+): closest beacon is at x=(?<bx>-?\\d+), y=(?<by>-?\\d+)")]
	private static partial Regex SensorRegex();

	private record struct Sensor(
		int sx, int sy,
		int bx, int by,
		int dist);
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var regex = SensorRegex();
		var data = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => (
				sx: int.Parse(m.Groups["sx"].Value),
				sy: int.Parse(m.Groups["sy"].Value),
				bx: int.Parse(m.Groups["bx"].Value),
				by: int.Parse(m.Groups["by"].Value)))
			.Select(s => new Sensor(
				s.sx, s.sy, s.bx, s.by,
				dist: Math.Abs(s.sx - s.bx) + Math.Abs(s.sy - s.by)))
			.ToList();

		var part1 = DoPart1(data).ToString();

		const int MapSize = 4_000_000;
		var point = Enumerable.Range(0, MapSize + 1)
			.Select(x => (x, data: data
				.Select(s => (s.sx, s.sy, distToX: s.dist - Math.Abs(x - s.sx)))
				.Where(s => s.distToX >= 0)
				.Select(s => (s.sx, s.sy, minY: s.sy - s.distToX, maxY: s.sy + s.distToX))
				.OrderBy(s => s.minY)
				.ToList()))
			.SelectMany(x =>
			{
				var range = (min: 0, max: 0);
				foreach (var (_, _, minY, maxY) in x.data)
				{
					if (minY <= range.max + 1)
						range = (range.min, Math.Max(maxY, range.max));
					else
						return new[] { (x.x, y: range.max + 1), };
				}
				return Array.Empty<(int x, int y)>();
			})
			.First();

		var part2 = (point.x * 4_000_000L + point.y).ToString();
		return (part1, part2);
	}

	private static int DoPart1(List<Sensor> sensors)
	{
		const int Row = 2_000_000;
		var ranges = sensors
			.Select(s => (s.sx, distToY: s.dist - Math.Abs(Row - s.sy)))
			.Where(s => s.distToY >= 0)
			.Select(s => (minX: s.sx - s.distToY, maxX: s.sx + s.distToY))
			.OrderBy(s => s.minX);

		var notPossible = new List<(int min, int max)>();
		foreach (var (minX, maxX) in ranges)
		{
			if (notPossible.Count == 0
				|| minX > notPossible[^1].max + 1)
			{
				notPossible.Add((minX, maxX));
			}
			else
			{
				notPossible[^1] = (
					notPossible[^1].min,
					Math.Max(maxX, notPossible[^1].max));
			}
		}

		var beacons = sensors
			.Where(s => s.by == Row)
			.Select(s => s.bx)
			.Distinct()
			.ToList();

		return notPossible
			.Select(r => (r.min, max: r.max - beacons.Count(b => b.Between(r.min, r.max))))
			.Sum(r => r.max - r.min + 1);
	}
}

