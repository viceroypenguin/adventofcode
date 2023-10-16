namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 02, CodeType.Original)]
public class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		instructions[1] = 12;
		instructions[2] = 2;
		var pc = new IntCodeComputer(instructions);
		pc.RunProgram();

		var part1 = pc.Memory[0].ToString();

		for (var noun = 0; ; noun++)
			for (var verb = 0; verb < 100; verb++)
			{
				instructions[1] = noun;
				instructions[2] = verb;

				pc = new IntCodeComputer(instructions, null, null);
				pc.RunProgram();

				if (pc.Memory[0] == 19690720)
				{
					var part2 = (noun * 100 + verb).ToString();
					return (part1, part2);
				}
			}
	}
}
