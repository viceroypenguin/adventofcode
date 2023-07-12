namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 05, CodeType.Original)]
public class Day_05_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var nums = input.Lines
			.Select(x => Convert.ToInt32(x))
			.ToList();

		var instructions = nums.ToList();
		var ptr = 0;
		var count = 0;

		while (ptr >= 0 && ptr < instructions.Count)
		{
			var adjust = instructions[ptr];
			instructions[ptr] = adjust + 1;
			ptr += adjust;
			count++;
		}

		var partA = count;

		instructions = nums.ToList();
		ptr = 0;
		count = 0;

		while (ptr >= 0 && ptr < instructions.Count)
		{
			var adjust = instructions[ptr];
			instructions[ptr] =
				adjust >= 3
					? adjust - 1
					: adjust + 1;
			ptr += adjust;
			count++;
		}

		var partB = count;

		return (partA.ToString(), partB.ToString());
	}
}
