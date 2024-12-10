namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 10, CodeType.Original)]
public partial class Day_10_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetIntMap();

		var zeros = map.GetMapPoints()
			.Where(p => p.item == 0)
			.ToList();

		var part1 = 0;
		var part2 = 0;

		var distinct = new HashSet<(int, int)>();
		foreach (var (p, item) in zeros)
		{
			distinct.Clear();

			part2 += SuperEnumerable
				.TraverseBreadthFirst(
					p,
					p => p.GetCartesianNeighbors(map)
						.Where(q => map[q.y][q.x] == map[p.y][p.x] + 1)
				)
				.Where(p => map[p.y][p.x] == 9)
				.Do(p => distinct.Add(p))
				.Count();

			part1 += distinct.Count;
		}

		return (part1.ToString(), part2.ToString());
	}
}
