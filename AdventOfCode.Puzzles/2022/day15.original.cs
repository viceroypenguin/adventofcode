namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 15, CodeType.Original)]
public partial class Day_15_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var regex = new Regex(@"Sensor at x=(?<sx>-?\d+), y=(?<sy>-?\d+): closest beacon is at x=(?<bx>-?\d+), y=(?<by>-?\d+)");
		var data = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => (
				sx: int.Parse(m.Groups["sx"].Value),
				sy: int.Parse(m.Groups["sy"].Value),
				bx: int.Parse(m.Groups["bx"].Value),
				by: int.Parse(m.Groups["by"].Value)))
			.Select(s => (
				s.sx, s.sy, s.bx, s.by,
				dist: Math.Abs(s.sx - s.bx) + Math.Abs(s.sy - s.by)))
			.ToList();

		const int Row = 2_000_000;
		var minX = data
			.Select(s => s.sx - (s.dist - Math.Abs(Row - s.sy)))
			.Min();
		var maxX = data
			.Select(s => s.sx + (s.dist - Math.Abs(Row - s.sy)))
			.Max();

		var points = Enumerable.Range(minX, maxX - minX + 1)
			.Where(x => !data.Any(s => (s.bx, s.by) == (x, Row)))
			.Where(x => data.Any(s => Math.Abs(x - s.sx) + Math.Abs(Row - s.sy) <= s.dist))
			.Count();

		var part1 = points.ToString();

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
}

