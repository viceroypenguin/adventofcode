namespace AdventOfCode;

public class Day_2021_15_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 15;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input.GetIntMap();
		var sideLength = map.Length;
		var destination = (x: sideLength - 1, y: sideLength - 1);
		var (_, _, risk) = Helpers.Dijkstra(
			(0, 0),
			p => p.GetCartesianNeighbors(map)
				.Select(q => (q, map[q.y][q.x])),
			(_, p) => p == destination);
		PartA = risk.ToString();

		destination = (sideLength * 5 - 1, sideLength * 5 - 1);
		int getRisk(int x, int y)
		{
			var increase = y / sideLength + x / sideLength;
			(x, y) = (x % sideLength, y % sideLength);
			return (((map[y][x] - 1) + increase) % 9) + 1;
		}

		(_, _, risk) = Helpers.Dijkstra(
			(0, 0),
			p => p.GetCartesianNeighbors()
				.Where(q => q.y >= 0 && q.y <= destination.y
					&& q.x >= 0 && q.x <= destination.x)
				.Select(q => (q, getRisk(q.x, q.y))),
			(_, p) => p == destination);
		PartB = risk.ToString();
	}
}
