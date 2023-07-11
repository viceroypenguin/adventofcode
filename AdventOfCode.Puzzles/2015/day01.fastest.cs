namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 01, CodeType.Fastest)]
public class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var level = 0;
		foreach (var c in input.Bytes)
			level += (('(' - c) * 2) + 1;
		var partA = level;

		level = 0;
		var cnt = 0;
		foreach (var c in input.Bytes)
		{
			cnt++;
			level += (('(' - c) * 2) + 1;
			if (level < 0)
				break;
		}

		return (partA.ToString(), cnt.ToString());
	}
}
