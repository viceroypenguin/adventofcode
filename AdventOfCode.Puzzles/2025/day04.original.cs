namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 04, CodeType.Original)]
public partial class Day_04_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		int RemovePoints()
		{
			var positions = map
				.GetMapPoints()
				.Where(
					p =>
						p.item == '@'
						&& p.p.GetCartesianAdjacent(map)
							.Count(q => map[q.y][q.x] == '@') < 4
				)
				.ToList();

			foreach (var ((x, y), _) in positions)
				map[y][x] = (byte)'.';

			return positions.Count;
		}

		var part1 = RemovePoints();
		var part2 = part1;

		while (RemovePoints() is > 0 and var cnt)
			part2 += cnt;

		return (part1.ToString(), part2.ToString());
	}
}
