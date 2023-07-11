namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 24, CodeType.Original)]
public class Day_24_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();
		var distanceMap = BuildDistanceMap(map);

		return (
			DoPartA(distanceMap).ToString(),
			DoPartB(distanceMap).ToString());
	}

	private static Dictionary<int, List<(int to, int cost)>> BuildDistanceMap(byte[][] map)
	{
		var numberPoints = map.GetMapPoints()
			.Where(x => x.item is >= (byte)'0' and <= (byte)'9')
			.ToDictionary(x => x.item, x => x.p);

		var distanceMap = new Dictionary<int, List<(int to, int cost)>>();

		foreach (var (item, p) in numberPoints)
		{
			var costs = SuperEnumerable.GetShortestPaths<(int, int), int>(
				p,
				(p, c) => p.GetCartesianNeighbors()
					.Where(q => map[q.y][q.x] != '#')
					.Select(q => (q, c + 1)));

			foreach (var (dest, q) in numberPoints)
				distanceMap.GetOrAdd(item - '0', _ => new()).Add((dest - '0', costs[q].cost));
		}

		return distanceMap;
	}

	private static int DoPartA(Dictionary<int, List<(int to, int cost)>> map)
	{
		var visitedAll = map.Keys
			.Aggregate(0, (v, k) => v | (1 << k));
		return SuperEnumerable
			.GetShortestPathCost<(int pos, int visited), int>(
				(pos: 0, visited: 1 << 0),
				(s, c) => map[s.pos]
					.Where(x => (s.visited & (1 << x.to)) == 0)
					.Select(x => ((x.to, s.visited | (1 << x.to)), c + x.cost)),
				s => s.visited == visitedAll);
	}

	private static int DoPartB(Dictionary<int, List<(int to, int cost)>> map)
	{
		var visitedAll = map.Keys
			.Aggregate(0, (v, k) => v | (1 << k));
		return SuperEnumerable
			.GetShortestPathCost<(int pos, int visited), int>(
				(pos: 0, visited: 1 << 0),
				(s, c) => s.visited == visitedAll
					? map[s.pos]
						.Where(x => x.to == 0)
						.Select(x => ((x.to, s.visited | (1 << map.Keys.Count)), c + x.cost))
					: map[s.pos]
						.Where(x => (s.visited & (1 << x.to)) == 0)
						.Select(x => ((x.to, s.visited | (1 << x.to)), c + x.cost)),
				s => s.visited == (visitedAll | (1 << map.Keys.Count)));
	}
}
