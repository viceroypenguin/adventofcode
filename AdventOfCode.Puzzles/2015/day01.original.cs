namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var level = 0;
		var basement = 0;
		foreach ((var i, var c) in input.Bytes.Index(1))
		{
			level += (('(' - c) * 2) + 1;
			if (basement == 0 && level == -1)
				basement = i;
		}

		return (level.ToString(), basement.ToString());
	}
}
