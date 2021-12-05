namespace AdventOfCode;

public class Day_2019_02_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 2;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		instructions[1] = 12;
		instructions[2] = 2;
		var pc = new IntCodeComputer(instructions);
		pc.RunProgram();

		PartA = pc.Memory[0].ToString();

		for (int noun = 0; noun < 100; noun++)
			for (int verb = 0; verb < 100; verb++)
			{
				instructions[1] = noun;
				instructions[2] = verb;

				pc = new IntCodeComputer(instructions, null, null);
				pc.RunProgram();

				if (pc.Memory[0] == 19690720)
				{
					PartB = (noun * 100 + verb).ToString();
					return;
				}
			}
	}
}
