using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_05_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 5;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		var pc = new IntCodeComputer(instructions);
		pc.Inputs.Enqueue(1);
		pc.RunProgram();
		while (pc.Outputs.Count > 0)
		{
			var value = pc.Outputs.Dequeue();
			if (value > 0)
			{
				PartA = value.ToString();
				break;
			}
		}

		pc = new IntCodeComputer(instructions);
		pc.Inputs.Enqueue(5);
		pc.RunProgram();
		PartB = pc.Outputs.Dequeue().ToString();
	}
}
