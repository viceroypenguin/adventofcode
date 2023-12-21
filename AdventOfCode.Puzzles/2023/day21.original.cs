using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 21, CodeType.Original)]
public partial class Day_21_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();
		var (p, _) = map.GetMapPoints()
			.First(x => x.item == 'S');

		Debug.Assert(map.Length == map[0].Length);
		Debug.Assert(p.x == p.y);

		// all even steps within 64 away
		var part1 = GetPathCosts(map, p, 64)[0];

		// _ _ _ c 1 6 _ _ _
		// _ _ c b e 5 6 _ _
		// _ c b e o e 5 6 _
		// c b e o e o e 5 6
		// 4 e o e S e o e 2
		// a 9 e o e o e 7 8
		// _ a 9 e o e 7 8 _
		// _ _ a 9 e 7 8 _ _
		// _ _ _ a 3 8 _ _ _
		// dist = 4; o = 9; e = 16; 
		// dist = 202,300; o = 202,299^2; e = 202,300^2

		const long TargetDistance = 26501365L;
		// total squares away
		var distance = (TargetDistance - p.x) / map.Length;
		// less one - these are all 100% covered
		var diff1 = distance - 1;

		// map #o/#e
		var costs = GetPathCosts(map, p, 200);

		// all interior squares fully covered; since fully covered,
		// don't care about actual distance, only care about even/odd squares
		// with map being odd squares wide, each square flips the even/odd designation
		var odds = diff1 * diff1 * costs[1];
		var evens = distance * distance * costs[0];
		var part2 = odds + evens;

		// for 1-4: because startingpoint is an odd, count itself and evens from there
		var mapRemainder = TargetDistance - (diff1 * map.Length) - p.x - 1;

		// map #1
		costs = GetPathCosts(map, (p.x, map.Length - 1), (int)mapRemainder);
		part2 += costs[0];

		// map #2
		costs = GetPathCosts(map, (0, p.y), (int)mapRemainder);
		part2 += costs[0];

		// map #3
		costs = GetPathCosts(map, (p.x, 0), (int)mapRemainder);
		part2 += costs[0];

		// map #4
		costs = GetPathCosts(map, (map.Length - 1, p.y), (int)mapRemainder);
		part2 += costs[0];

		// for 5,7,9,b: because startingpoint is an even, count odds
		mapRemainder = TargetDistance - (diff1 * map.Length) - 1;

		// map #5
		costs = GetPathCosts(map, (0, map.Length - 1), (int)mapRemainder);
		part2 += costs[1] * diff1;

		// map #7
		costs = GetPathCosts(map, (0, 0), (int)mapRemainder);
		part2 += costs[1] * diff1;

		// map #9
		costs = GetPathCosts(map, (map.Length - 1, 0), (int)mapRemainder);
		part2 += costs[1] * diff1;

		// map #b
		costs = GetPathCosts(map, (map.Length - 1, map.Length - 1), (int)mapRemainder);
		part2 += costs[1] * diff1;

		// for 6,8,a,c: because startingpoint is an odd, count evens
		mapRemainder = TargetDistance - (distance * map.Length) - 1;

		// map #6
		costs = GetPathCosts(map, (0, map.Length - 1), (int)mapRemainder);
		part2 += costs[0] * distance;

		// map #8
		costs = GetPathCosts(map, (0, 0), (int)mapRemainder);
		part2 += costs[0] * distance;

		// map #a
		costs = GetPathCosts(map, (map.Length - 1, 0), (int)mapRemainder);
		part2 += costs[0] * distance;

		// map #c
		costs = GetPathCosts(map, (map.Length - 1, map.Length - 1), (int)mapRemainder);
		part2 += costs[0] * distance;

		return (part1.ToString(), part2.ToString());
	}

	private static Dictionary<int, int> GetPathCosts(byte[][] map, (int x, int y) start, int maxDistance)
	{
		var costs = SuperEnumerable
			.GetShortestPaths<(int x, int y), int>(
				start,
				(p, c) => GetNeighbors(map, p, c, maxDistance));
		return costs
			.GroupBy(x => x.Value.cost % 2)
			.ToDictionary(x => x.Key, x => x.Count());
	}

	private static IEnumerable<((int x, int y) p, int cost)> GetNeighbors(byte[][] map, (int x, int y) p, int cost, int maxDistance)
	{
		if (cost == maxDistance)
			return [];

		return p.GetCartesianNeighbors(map)
			.Where(q => map[q.y][q.x] != '#')
			.Select(q => (q, cost + 1));
	}
}
