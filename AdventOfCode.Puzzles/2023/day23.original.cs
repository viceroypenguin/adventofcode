using System.Collections.Immutable;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 23, CodeType.Original)]
public partial class Day_23_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();
		var end = (map[0].Length - 2, map.Length - 1);

		var graph = BuildGraph(map, end, ignoreSlopes: false);
		var part1 = GetLongestPath(graph, (1, 0), end, 0, []);

		graph = BuildGraph(map, end, ignoreSlopes: true);
		var part2 = GetLongestPath(graph, (1, 0), end, 0, []);

		return (part1.ToString(), part2.ToString());
	}

	private static Dictionary<(int x, int y), List<((int x, int y) p, int c)>> BuildGraph(
		byte[][] map, (int x, int y) end, bool ignoreSlopes)
	{
		var graph = new Dictionary<(int x, int y), List<((int x, int y) p, int c)>>();
		var queue = new Queue<((int x, int y) prev, (int x, int y) p)>();
		queue.Enqueue(((1, 0), (1, 0)));

		while (queue.TryDequeue(out var q))
		{
			var (prev, p) = q;
			var from = prev;
			var dist = 0;

			while (true)
			{
				if (p == end)
				{
					var list = graph.GetOrAdd(from, _ => []);

					if (!list.Any(q => q.p == p))
						list.Add((p, dist));

					break;
				}

				dist++;
				var neighbors = p.GetCartesianNeighbors(map)
					.Where(q => q != prev)
					.Where(q => map[q.y][q.x] != '#')
					.ToList();

				if (neighbors.Count == 0)
				{
					break;
				}
				else if (neighbors.Count > 1)
				{
					var list = graph.GetOrAdd(from, _ => []);

					if (!list.Any(q => q.p == p))
					{
						list.Add((p, dist));

						foreach (var (x, y) in neighbors)
						{
							if (!ignoreSlopes)
							{
								var b = map[y][x];
								var delta = (x - p.x, y - p.y);

								var slope = (delta, b) switch
								{
									((0, 1), (byte)'^') => true,
									((0, -1), (byte)'v') => true,
									((1, 0), (byte)'<') => true,
									((-1, 0), (byte)'>') => true,
									_ => false,
								};

								if (slope)
									continue;
							}

							queue.Enqueue((p, (x, y)));
						}
					}

					break;
				}
				else
				{
					(prev, p) = (p, neighbors[0]);
				}
			}
		}

		return graph;
	}

	private static int GetLongestPath(
		Dictionary<(int x, int y), List<((int x, int y) p, int c)>> graph,
		(int x, int y) p,
		(int x, int y) end,
		int cost,
		ImmutableHashSet<(int x, int y)> seen)
	{
		if (seen.Contains(p))
			return -1;

		if (p == end)
			return cost;

		var max = -1;
		seen = seen.Add(p);
		foreach (var (q, c) in graph.GetValueOrDefault(p) ?? [])
			max = Math.Max(max, GetLongestPath(graph, q, end, cost + c, seen));
		return max;
	}
}
