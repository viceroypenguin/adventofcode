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
			.ToList();

		var partA = instructions.ToArray();
		partA[1] = 12;
		partA[2] = 2;
		var pc = new IntCodeComputer(partA, null, null);
		pc.RunProgram()
			.GetAwaiter()
			.GetResult();

		PartA = partA[0].ToString();

		for (int noun = 0; noun < 100; noun++)
			for (int verb = 0; verb < 100; verb++)
			{
				var partB = instructions.ToArray();
				partB[1] = noun;
				partB[2] = verb;

				pc = new IntCodeComputer(partB, null, null);
				pc.RunProgram()
					.GetAwaiter()
					.GetResult();

				if (partB[0] == 19690720)
				{
					PartB = (noun * 100 + verb).ToString();
					return;
				}
			}
	}
}
