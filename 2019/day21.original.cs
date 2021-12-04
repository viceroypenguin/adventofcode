using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_21_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 21;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		// 64k should be enough for anyone
		Array.Resize(ref instructions, 64 * 1024);

		DoPartA(instructions);
		DoPartB(instructions);
	}

	private void DoPartA(long[] instructions)
	{
		var inputs = new BufferBlock<long>();
		var outputs = new BufferBlock<long>();

		var program = new IntCodeComputer(instructions.ToArray(), inputs, outputs)
			.RunProgram();
		outputs.TryReceiveAll(out var output);

		const string springScriptA =
@"NOT A J
NOT B T
OR T J
NOT C T
OR T J
AND D J
WALK
";
		foreach (var b in Encoding.ASCII.GetBytes(springScriptA).Where(b => b != '\r'))
			inputs.Post(b);

		program.GetAwaiter().GetResult();

		outputs.TryReceiveAll(out output);

		PartA = output.Any(o => o > 255)
			? output.Where(o => o > 255).First().ToString()
			: Encoding.ASCII.GetString(
				output.Select(b => (byte)b).ToArray());
	}

	private void DoPartB(long[] instructions)
	{
		var inputs = new BufferBlock<long>();
		var outputs = new BufferBlock<long>();

		var program = new IntCodeComputer(instructions.ToArray(), inputs, outputs)
			.RunProgram();
		outputs.TryReceiveAll(out var output);

		const string springScriptB =
@"NOT A J
NOT B T
OR T J
NOT C T
OR T J
AND D J
NOT E T
NOT T T
OR H T
AND T J
RUN
";
		foreach (var b in Encoding.ASCII.GetBytes(springScriptB).Where(b => b != '\r'))
			inputs.Post(b);

		program.GetAwaiter().GetResult();

		outputs.TryReceiveAll(out output);

		PartB = output.Any(o => o > 255)
			? output.Where(o => o > 255).First().ToString()
			: Encoding.ASCII.GetString(
				output.Select(b => (byte)b).ToArray());
	}
}
