namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 12, CodeType.Original)]
public partial class Day_12_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		var seen = new HashSet<(int x, int y)>();
		var part1 = 0L;
		var part2 = 0L;

		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				var c = map[y][x];
				if (seen.Contains((x, y)))
					continue;

				var region = new HashSet<(int x, int y)>();

				map.FloodFill(
					(x, y),
					(_, i) => c == i,
					(p, _) =>
					{
						region.Add(p);
						seen.Add(p);
					}
				);

				var area = region.Count;
				var perimeter = region
					.SelectMany(p =>
						p.GetCartesianNeighbors()
							.Where(q => !region.Contains(q))
					)
					.Count();
				part1 += area * perimeter;

				var sides = region
					.SelectMany(p => new[]
					{
						(p: (x: p.x, y: p.y - 1), d: 1),
						(p: (x: p.x + 1, y: p.y), d: 2),
						(p: (x: p.x, y: p.y + 1), d: 3),
						(p: (x: p.x - 1, y: p.y), d: 0),
					})
					.Where(x => !region.Contains(x.p))
					.OrderBy(x => x.d switch
					{
						1 => (x.d, x.p.y, x.p.x),
						2 => (x.d, x.p.x, x.p.y),
						3 => (x.d, x.p.y, -x.p.x),
						_ => (x.d, x.p.x, -x.p.y),
					})
					.Segment(
						(cur, prev, _) =>
							cur.d != prev.d
							|| cur.d switch
							{
								0 => cur.p != (prev.p.x, prev.p.y - 1),
								1 => cur.p != (prev.p.x + 1, prev.p.y),
								2 => cur.p != (prev.p.x, prev.p.y + 1),
								_ => cur.p != (prev.p.x - 1, prev.p.y),
							}
					)
					.Count();

				part2 += area * sides;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
