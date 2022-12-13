namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 05, CodeType.Original)]
public class Day_05_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		var pc = new IntCodeComputer(instructions);
		pc.Inputs.Enqueue(1);
		pc.RunProgram();
		var part1 = string.Empty;
		while (pc.Outputs.Count > 0)
		{
			var value = pc.Outputs.Dequeue();
			if (value > 0)
			{
				part1 = value.ToString();
				break;
			}
		}

		pc = new IntCodeComputer(instructions);
		pc.Inputs.Enqueue(5);
		pc.RunProgram();
		var part2 = pc.Outputs.Dequeue().ToString();
		return (part1, part2);
	}
}
