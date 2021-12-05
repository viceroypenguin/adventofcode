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

		PartA = DoPart(instructions, springScriptA);
		PartB = DoPart(instructions, springScriptB);
	}

		const string springScriptA =
@"NOT A J
NOT B T
OR T J
NOT C T
OR T J
AND D J
WALK
";

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

	private static string DoPart(long[] instructions, string scriptCode)
	{
		var pc = new IntCodeComputer(instructions);
		foreach (var b in Encoding.ASCII.GetBytes(scriptCode).Where(b => b != '\r'))
			pc.Inputs.Enqueue(b);

		pc.RunProgram();

		return pc.Outputs.Any(o => o > 255)
			? pc.Outputs.Where(o => o > 255).First().ToString()
			: Encoding.ASCII.GetString(
				pc.Outputs.Select(b => (byte)b).ToArray());
	}
}
