namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 3, CodeType.Original)]
public class Day_03_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Lines
			.Select(s => s.Select(c => c == '#').ToArray())
			.ToArray();

		long GetTreesOnSlope(int vx, int vy)
		{
			var count = 0;
			for (int x = 0, y = 0; x < map.Length; x += vx, y += vy)
				if (map[x][y % map[x].Length])
					count++;
			return count;
		}

		var part1 = GetTreesOnSlope(1, 3).ToString();

		var part2 = (
			GetTreesOnSlope(1, 1) *
			GetTreesOnSlope(1, 3) *
			GetTreesOnSlope(1, 5) *
			GetTreesOnSlope(1, 7) *
			GetTreesOnSlope(2, 1)).ToString();

		return (part1, part2);
	}
}
