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

		var map = input.GetMap();

		// visit every point
		var lowPoints = map.GetMapPoints()
			// check if point is surrounded by points strictly greater
			.Where(z => z.p.GetCartesianNeighbors(map)
				.All(q => z.item < map[q.y][q.x]))
			// save this list, we'll need it later
			.ToList();

		// take the value at each point, add one, and sum them
		PartA = lowPoints
			.Select(p => (p.item - (byte)'0') + 1)
			.Sum()
			.ToString();

		PartB = lowPoints
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
	}
}
