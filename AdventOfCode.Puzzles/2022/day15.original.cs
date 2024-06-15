namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 15, CodeType.Original)]
public partial class Day_15_Original : IPuzzle
{
	[GeneratedRegex("Sensor at x=(?<sx>-?\\d+), y=(?<sy>-?\\d+): closest beacon is at x=(?<bx>-?\\d+), y=(?<by>-?\\d+)")]
	private static partial Regex SensorRegex();

	private record struct Sensor(
		int Sx, int Sy,
		int Bx, int By,
		int Dist
	);

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
				Dist: Math.Abs(s.sx - s.bx) + Math.Abs(s.sy - s.by)))
			.ToList();

		var part1 = DoPart1(data).ToString();

		var l1 = new Line(new(0, 0), new(5, 5));
		var l2 = new Line(new(5, 0), new(0, 5));

		var (x, y) = DoPart2(data);
		var part2 = ((x * 4_000_000L) + y).ToString();

		return (part1, part2);
	}

	private static int DoPart1(List<Sensor> sensors)
	{
		const int Row = 2_000_000;
		var ranges = sensors
			.Select(s => (s.Sx, distToY: s.Dist - Math.Abs(Row - s.Sy)))
			.Where(s => s.distToY >= 0)
			.Select(s => (minX: s.Sx - s.distToY, maxX: s.Sx + s.distToY))
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
			.Where(s => s.By == Row)
			.Select(s => s.Bx)
			.Distinct()
			.ToList();

		return notPossible
			.Select(r => (r.min, max: r.max - beacons.Count(b => b.Between(r.min, r.max))))
			.Sum(r => r.max - r.min + 1);
	}

	private const int MapSize = 4_000_000;
	private static (int x, int y) DoPart2(List<Sensor> sensors)
	{
		var llines = sensors
			// for each sensor, get the lines that describe the external border
			.SelectMany(GetLLines)
			.ToList();
		var rlines = sensors
			.SelectMany(GetRLines)
			.ToList();

		return llines.Cartesian(rlines, (l, r) => l.Intersects(r))
			.Where(p => p != null)
			.Select(p => p!.Value)
			.Where(p => p.x.Between(0, MapSize) && p.y.Between(0, MapSize))
			.Where(p => !sensors.Any(s => Math.Abs(s.Sx - p.x) + Math.Abs(s.Sy - p.y) <= s.Dist))
			.First();
	}

	private record struct Point(int X, int Y);
	private record struct Line(Point A, Point B)
	{
		public readonly (int x, int y)? Intersects(Line o)
		{
			var xdiff = (A.X - B.X, o.A.X - o.B.X);
			var ydiff = (A.Y - B.Y, o.A.Y - o.B.Y);

			static long Determinant((long a, long b) x, (long a, long b) y) =>
				(x.a * y.b) - (x.b * y.a);

			var div = Determinant(xdiff, ydiff);
			// lines don't intersect
			if (div == 0) return default;

			var d = (
				Determinant((A.X, B.X), (A.Y, B.Y)),
				Determinant((o.A.X, o.B.X), (o.A.Y, o.B.Y))
			);

			var x = Determinant(d, xdiff) / div;
			var y = Determinant(d, ydiff) / div;

			if (Math.Abs(x - A.X) != Math.Abs(y - A.Y)
				|| Math.Abs(x - o.A.X) != Math.Abs(y - o.A.Y))
			{
				return default;
			}

			return ((int)x, (int)y);
		}
	}

	private static IEnumerable<Line> GetLLines(Sensor s)
	{
		var (sx, sy, _, _, dist) = s;

		yield return new(new(sx - (dist + 1), sy), new(sx, sy - (dist + 1)));
		yield return new(new(sx, sy + dist + 1), new(sx + dist + 1, sy));
	}

	private static IEnumerable<Line> GetRLines(Sensor s)
	{
		var (sx, sy, _, _, dist) = s;

		yield return new(new(sx - (dist + 1), sy), new(sx, sy + dist + 1));
		yield return new(new(sx, sy - (dist + 1)), new(sx + dist + 1, sy));
	}
}

