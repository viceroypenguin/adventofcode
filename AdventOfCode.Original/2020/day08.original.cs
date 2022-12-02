namespace AdventOfCode;

public class Day_2020_08_Original : Day
{
	public override int Year => 2020;
	public override int DayNumber => 8;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"^(?<inst>\w+) (?<val>(\+|-)?\d+)$");
		var instructions = input.GetLines()
			.Select(l => regex.Match(l))
			.Select(m => (
				opcode: m.Groups["inst"].Value,
				value: Convert.ToInt32(m.Groups["val"].Value)))
			.ToArray();

		PartA = RunProgram(instructions).acc.ToString();

		for (int i = 0; i < instructions.Length; i++)
		{
			var orig = instructions[i];
			if (orig.opcode == "acc") continue;
			instructions[i] = orig switch
			{
				("nop", var v) => ("jmp", v),
				("jmp", var v) => ("nop", v),
				_ => throw new InvalidOperationException(),
			};

			var (looped, acc) = RunProgram(instructions);
			if (!looped)
			{
				PartB = acc.ToString();
				return;
			}
			else
				instructions[i] = orig;
		}
	}

	private (bool looped, int acc) RunProgram((string opcode, int value)[] program)
	{
		var executed = new List<int>();
		int acc = 0, ip = 0;
		while (true)
		{
			switch (program[ip])
			{
				case ("nop", _): ip++; break;

				case ("acc", var value):
					acc += value;
					ip++;
					break;

				case ("jmp", var value):
					ip += value;
					break;
			}

			if (ip >= program.Length)
				return (false, acc);
			else if (executed.Contains(ip))
				return (true, acc);
			else
				executed.Add(ip);
		}
	}
}
