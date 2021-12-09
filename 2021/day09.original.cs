using System.Collections;

namespace AdventOfCode;

public class Day_2021_09_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 9;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input.GetLines();

		// the directions we can go
		var dirs = new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0), };
		// is this position inside the map?
		bool CheckInMap((int x, int y) p) =>
			p.y >= 0 && p.y < map.Length
			&& p.x >= 0 && p.x < map[p.y].Length;

		// visit every point
		var lowPoints = Enumerable.Range(0, map.Length)
			.SelectMany(y => Enumerable.Range(0, map[y].Length).Select(x => (x, y)))
			// check if point is surrounded by points strictly greater
			.Where(p => dirs
				// look in every direction
				.Select(d => (x: d.x + p.x, y: d.y + p.y))
				// only check points in map
				.Where(CheckInMap)
				// all remaining points must be strictly greater
				.All(q => map[p.y][p.x] < map[q.y][q.x]))
			// save this list, we'll need it later
			.ToList();

		// take the value at each point, add one, and sum them
		PartA = lowPoints
			.Select(p => (map[p.y][p.x] - (byte)'0') + 1)
			.Sum()
			.ToString();

		PartB = lowPoints
			// for each point, do a BFS search to flood-fill the basin
			.Select(p =>
			{
				// keep track of how big the basin is
				var s = 0;
				// keep track of where we've been
				var seen = new HashSet<(int x, int y)>();

				// get the neighboring points...
				IEnumerable<(int x, int y)> traverse((int x, int y) q)
				{
					var (x, y) = q;

					// on another type of border
					if (map[y][x] == '9')
						// don't go anywhere
						return Array.Empty<(int x, int y)>();

					// we've been here before
					if (seen.Contains((x, y)))
						// don't go anywhere
						return Array.Empty<(int x, int y)>();

					// now we've been here for the first time
					// remember this and increase size of the basin
					seen.Add((x, y));
					s++;

					// go in all four directions
					return dirs
						.Select(d => (x: x + d.x, y: y + d.y))
						// that are still inside the map
						.Where(CheckInMap);
				}

				// execute a BFS based on traverse method
				MoreEnumerable
					.TraverseBreadthFirst(
						p,
						traverse)
					.Consume();

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
	}
}
