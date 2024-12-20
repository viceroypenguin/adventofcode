namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 20, CodeType.Original)]
public partial class Day_20_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		var start = map.GetMapPoints().First(p => p.item == 'S').p;
		var end = map.GetMapPoints().First(p => p.item == 'E').p;

		var paths = SuperEnumerable.GetShortestPaths<(int x, int y), int>(
			start,
			(p, c) => p.GetCartesianNeighbors()
				.Where(q => map[q.y][q.x] != '#')
				.Select(q => (q, c + 1))
		);

		var part1 = paths.Keys
			.SelectMany(p => MapExtensions.Neighbors
				.Select(d =>
					(
						p,
						q: (x: p.x + d.x + d.x, y: p.y + d.y + d.y)
					)
				)
				.Where(x =>
					x.q.x.Between(0, map[0].Length - 1)
					&& x.q.y.Between(0, map.Length - 1)
					&& map[x.q.y][x.q.x] != '#'
				)
			)
			.Count(x => paths[x.q].cost - paths[x.p].cost - 2 >= 100);

		var part2 = 0L;
		foreach (var (x, y) in paths.Keys)
		{
			var startCost = paths[(x, y)].cost;

			for (var dy = -20; dy <= +20; dy++)
			{
				var mindx = Math.Abs(dy) - 20;
				var maxdx = -mindx;
				for (var dx = mindx; dx <= maxdx; dx++)
				{
					if (!paths.TryGetValue((x + dx, y + dy), out var value))
						continue;

					var deltaCost = value.cost - startCost;
					deltaCost -= Math.Abs(dy) + Math.Abs(dx);
					if (deltaCost >= 100)
						part2++;
				}
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
