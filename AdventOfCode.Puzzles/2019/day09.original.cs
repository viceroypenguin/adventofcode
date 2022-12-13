namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 09, CodeType.Original)]
public class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		var pc = new IntCodeComputer(instructions, size: 640 * 1024);
		pc.Inputs.Enqueue(1);
		pc.RunProgram();
		var part1 = pc.Outputs.Dequeue().ToString();

		pc = new IntCodeComputer(instructions, size: 640 * 1024);
		pc.Inputs.Enqueue(2);
		pc.RunProgram();
		var part2 = pc.Outputs.Dequeue().ToString();

		return (part1, part2);
	}
}
