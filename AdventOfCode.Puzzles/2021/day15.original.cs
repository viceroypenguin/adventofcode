namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 15, CodeType.Original)]
public class Day_15_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetIntMap();
		var sideLength = map.Length;
		var destination = (x: sideLength - 1, y: sideLength - 1);
		var risk = SuperEnumerable.GetShortestPathCost<(int, int), int>(
			(0, 0),
			(p, c) => p.GetCartesianNeighbors(map)
				.Select(q => (q, c + map[q.y][q.x])),
			destination);
		var part1 = risk.ToString();

		destination = ((sideLength * 5) - 1, (sideLength * 5) - 1);
		int GetRisk(int x, int y)
		{
			var increase = (y / sideLength) + (x / sideLength);
			(x, y) = (x % sideLength, y % sideLength);
			return ((map[y][x] - 1 + increase) % 9) + 1;
		}

		risk = SuperEnumerable.GetShortestPathCost<(int, int), int>(
			(0, 0),
			(p, c) => p.GetCartesianNeighbors()
				.Where(q => q.y >= 0 && q.y <= destination.y
					&& q.x >= 0 && q.x <= destination.x)
				.Select(q => (q, c + GetRisk(q.x, q.y))),
			destination);
		var part2 = risk.ToString();

		return (part1, part2);
	}
}
