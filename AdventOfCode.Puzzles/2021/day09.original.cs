namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 9, CodeType.Original)]
public class Day_2021_09_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		// visit every point
		var lowPoints = map.GetMapPoints()
			// check if point is surrounded by points strictly greater
			.Where(z => z.p.GetCartesianNeighbors(map)
				.All(q => z.item < map[q.y][q.x]))
			// save this list, we'll need it later
			.ToList();

		// take the value at each point, add one, and sum them
		var part1 = lowPoints
			.Select(p => (p.item - (byte)'0') + 1)
			.Sum()
			.ToString();

		var part2 = lowPoints
			// for each point, do a BFS search to flood-fill the basin
			.Select(p =>
			{
				// keep track of how big the basin is
				var s = 0;

				// fill the basin
				map.FloodFill(
					p.p,
					(q, i) => i != '9',
					(_, _) => s++);

				// we know the size of the basin, return it
				return s;
			})
			.ToList()
			// get the largest numbers
			.OrderByDescending(x => x)
			// take the top three of them
			.Take(3)
			// calculate the product of these numbers
			.Aggregate(1L, (a, b) => a * b)
			.ToString();

		return (part1, part2);
	}
}
