using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_09_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 9;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		var pc = new IntCodeComputer(instructions, size: 640 * 1024);
		pc.Inputs.Enqueue(1);
		pc.RunProgram();
		PartA = pc.Outputs.Dequeue().ToString();

		pc = new IntCodeComputer(instructions, size: 640 * 1024);
		pc.Inputs.Enqueue(2);
		pc.RunProgram();
		PartB = pc.Outputs.Dequeue().ToString();
	}
}
