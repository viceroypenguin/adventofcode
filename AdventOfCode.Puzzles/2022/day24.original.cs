namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 24, CodeType.Original)]
public class Day_24_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();
		var lastBlizzard = map.GetMapPoints()
			.Where(p => (char)p.item is '<' or '>' or '^' or 'v')
			.ToList();

		var blizzards = new List<HashSet<(int x, int y)>>
		{
			lastBlizzard.Select(p => p.p).ToHashSet(),
		};
		HashSet<(int x, int y)> GetBlizzardAtTime(int t)
		{
			if (t < blizzards.Count)
				return blizzards[t];

			lastBlizzard = lastBlizzard
				.Select(_ =>
				{
					var ((x, y), dir) = _;
					var n = dir switch
					{
						(byte)'>' => (x: x + 1 == map[0].Length - 1 ? 1 : x + 1, y),
						(byte)'<' => (x: x - 1 == 0 ? map[0].Length - 2 : x - 1, y),
						(byte)'^' => (x, y - 1 == 0 ? map.Length - 2 : y - 1),
						(byte)'v' => (x, y + 1 == map.Length - 1 ? 1 : y + 1),
					};
					return (n, dir);
				})
				.ToList();

			var ret = lastBlizzard.Select(x => x.p).ToHashSet();
			blizzards.Add(ret);
			return ret;
		}

		IEnumerable<((int x, int y, int t), int t)> GetNeighbors((int x, int y, int t) p, int t)
		{
			t = p.t + 1;
			var b = GetBlizzardAtTime(t);
			return (p.x, p.y).GetCartesianNeighbors()
				.Append((p.x, p.y))
				.Where(q =>
					!b.Contains(q)
					&& q.y.Between(0, map.Length - 1)
					&& map[q.y][q.x] != '#')
				.Select(q => ((q.x, q.y, t), t));
		}

		var cost = SuperEnumerable.GetShortestPathCost<(int x, int y, int t), int>(
			(1, 0, 0),
			GetNeighbors,
			s => s.x == map[0].Length - 2 && s.y == map.Length - 1);

		var part1 = cost.ToString();

		cost = SuperEnumerable.GetShortestPathCost<(int x, int y, int t), int>(
			(map[0].Length - 2, map.Length - 1, cost),
			GetNeighbors,
			s => s.x == 1 && s.y == 0);

		cost = SuperEnumerable.GetShortestPathCost<(int x, int y, int t), int>(
			(1, 0, cost),
			GetNeighbors,
			s => s.x == map[0].Length - 2 && s.y == map.Length - 1);

		var part2 = cost.ToString();
		return (part1, part2);
	}
}
