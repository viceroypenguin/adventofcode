namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 8, CodeType.Original)]
public partial class Day_08_Original : IPuzzle
{
	[GeneratedRegex("^(?<inst>\\w+) (?<val>(\\+|-)?\\d+)$")]
	private static partial Regex InstructionsRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionsRegex();
		var instructions = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => (
				opcode: m.Groups["inst"].Value,
				value: Convert.ToInt32(m.Groups["val"].Value)))
			.ToArray();

		var part1 = RunProgram(instructions).acc.ToString();

		for (var i = 0; ; i++)
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
				var part2 = acc.ToString();
				return (part1, part2);
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
