namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 12, CodeType.Original)]
public partial class Day_12_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		var start = (x: 0, y: 0);
		var end = start;
		for (int y = 0; y < map.Length; y++)
			for (int x = 0; x < map[y].Length; x++)
				if (map[y][x] == (byte)'S')
				{
					start = (x, y);
				}
				else if (map[y][x] == (byte)'E')
				{
					end = (x, y);
				}

		map[start.y][start.x] = (byte)'a';
		map[end.y][end.x] = (byte)'z';

		var cost = SuperEnumerable.GetShortestPathCost<(int x, int y), int>(
			start,
			(p, c) => p.GetCartesianNeighbors(map)
				.Where(q => map[q.y][q.x] - map[p.y][p.x] <= 1)
				.Select(q => (q, c + 1)),
			end);

		var part1 = cost.ToString();

		var part2 = int.MaxValue;
		for (int y = 0; y < map.Length; y++)
			for (int x = 0; x < map[y].Length; x++)
				if (map[y][x] == (byte)'a')
				{
					try
					{
						var curCost = SuperEnumerable.GetShortestPathCost<(int x, int y), int>(
							(x, y),
							(p, c) => p.GetCartesianNeighbors(map)
								.Where(q => map[q.y][q.x] - map[p.y][p.x] <= 1)
								.Select(q => (q, c + 1)),
							end);
						if (curCost < part2)
							part2 = curCost;
					}
					// don't care if we can't reach end
					catch { }
				}

		return (part1, part2.ToString());
	}
}
