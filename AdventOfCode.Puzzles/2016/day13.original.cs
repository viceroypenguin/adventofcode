using System.Numerics;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 13, CodeType.Original)]
public class Day_13_Original : IPuzzle
{
	private static bool IsWall(int x, int y, int number)
	{
		var num = (x * x) + (3 * x) + (2 * x * y) + y + (y * y) + number;
		var bitCount = BitOperations.PopCount((uint)num);
		return (bitCount % 2) == 1;
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var number = Convert.ToInt32(input.Lines[0]);

		var partA = SuperEnumerable.GetShortestPathCost<(int x, int y), int>(
			(1, 1),
			(p, c) => p.GetCartesianNeighbors()
				.Where(p => !IsWall(p.x, p.y, number))
				.Select(p => (p, c + 1)),
			(31, 39));

		var count = -1;
		var flag = false;
		_ = SuperEnumerable.GetShortestPathCost<(int x, int y), int>(
			(1, 1),
			(p, c) =>
			{
				flag = c > 50;
				count++;
				return p.GetCartesianNeighbors()
					.Where(p => !IsWall(p.x, p.y, number))
					.Select(p => (p, c + 1));
			},
			_ => flag);

		return (partA.ToString(), count.ToString());
	}
}
