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

		// 640k should be enough for anyone
		Array.Resize(ref instructions, 640 * 1024);

		var inputs = new BufferBlock<long>();
		inputs.Post(1);
		var outputs = new BufferBlock<long>();
		var pc = new IntCodeComputer(instructions.ToArray(), inputs, outputs);
		pc.RunProgram()
			.GetAwaiter()
			.GetResult();
		PartA = outputs.Receive().ToString();

		inputs = new BufferBlock<long>();
		inputs.Post(2);
		outputs = new BufferBlock<long>();
		pc = new IntCodeComputer(instructions.ToArray(), inputs, outputs);
		pc.RunProgram()
			.GetAwaiter()
			.GetResult();
		PartB = outputs.Receive().ToString();
	}
}
