﻿namespace AdventOfCode.Puzzles._2022;

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


		var l1 = new Line(new(0, 0), new(5, 5));
		var l2 = new Line(new(5, 0), new(0, 5));

		var point = DoPart2(data);
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

	const int MapSize = 4_000_000;
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
			.Where(p => !sensors.Any(s => Math.Abs(s.sx - p.x) + Math.Abs(s.sy - p.y) <= s.dist))
			.First();
	}

	private record struct Point(int x, int y);
	private record struct Line(Point a, Point b)
	{
		public (int x, int y)? Intersects(Line o)
		{
			var xdiff = (a.x - b.x, o.a.x - o.b.x);
			var ydiff = (a.y - b.y, o.a.y - o.b.y);

			static long Determinant((long a, long b) x, (long a, long b) y) =>
				x.a * y.b - x.b * y.a;

			var div = Determinant(xdiff, ydiff);
			// lines don't intersect
			if (div == 0) return default;

			var d = (
				Determinant((a.x, b.x), (a.y, b.y)),
				Determinant((o.a.x, o.b.x), (o.a.y, o.b.y)));
			var x = Determinant(d, xdiff) / div;
			var y = Determinant(d, ydiff) / div;

			if (Math.Abs(x - a.x) != Math.Abs(y - a.y)
				|| Math.Abs(x - o.a.x) != Math.Abs(y - o.a.y))
				return default;

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

