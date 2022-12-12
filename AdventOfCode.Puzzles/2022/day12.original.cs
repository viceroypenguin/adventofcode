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

		var costs = SuperEnumerable.GetShortestPaths<(int x, int y), int>(
			end,
			(p, c) => p.GetCartesianNeighbors(map)
				.Where(q => map[p.y][p.x] - map[q.y][q.x] <= 1)
				.Select(q => (q, c + 1)));

		var part1 = costs[start].cost.ToString();

		var part2 = map.GetMapPoints()
			.Where(x => x.item == (byte)'a')
			.Select(x => costs.TryGetValue(x.p, out var cost) ? cost.cost : int.MaxValue)
			.Min()
			.ToString();

		return (part1, part2);
	}
}
