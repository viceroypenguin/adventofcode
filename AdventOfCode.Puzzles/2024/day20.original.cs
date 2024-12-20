using System.IO;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 20, CodeType.Original)]
public partial class Day_20_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		var start = map.GetMapPoints().First(p => p.item == 'S').p;

		var paths = SuperEnumerable.GetShortestPaths<(int x, int y), int>(
			start,
			(p, c) => p.GetCartesianNeighbors()
				.Where(q => map[q.y][q.x] != '#')
				.Select(q => (q, c + 1))
		);

		var part1 = GetCheatCount(paths, 2);
		var part2 = GetCheatCount(paths, 20);

		return (part1.ToString(), part2.ToString());
	}

	private static int GetCheatCount(IReadOnlyDictionary<(int x, int y), ((int x, int y) previousState, int cost)> paths, int maxLength)
	{
		var count = 0;
		foreach (var (x, y) in paths.Keys)
		{
			var startCost = paths[(x, y)].cost;

			for (var dy = -maxLength; dy <= +maxLength; dy++)
			{
				var mindx = Math.Abs(dy) - maxLength;
				var maxdx = -mindx;
				for (var dx = mindx; dx <= maxdx; dx++)
				{
					if (!paths.TryGetValue((x + dx, y + dy), out var value))
						continue;

					var deltaCost = value.cost - startCost;
					deltaCost -= Math.Abs(dy) + Math.Abs(dx);
					if (deltaCost >= 100)
						count++;
				}
			}
		}

		return count;
	}
}
