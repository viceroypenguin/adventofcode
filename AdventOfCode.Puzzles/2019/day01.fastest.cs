namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 01, CodeType.Fastest)]
public class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		int part1Sum = 0, part2Sum = 0, n = 0;
		foreach (var c in input.Bytes)
		{
			if (c == '\n')
			{
				var fuel = n / 3 - 2;
				part1Sum += fuel;
				while (fuel > 0)
				{
					part2Sum += fuel;
					fuel = fuel / 3 - 2;
				}
				n = 0;
			}
			else if (c >= '0')
				n = n * 10 + c - '0';
		}

		return (
			part1Sum.ToString(),
			part2Sum.ToString());
	}
}
