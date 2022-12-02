namespace AdventOfCode;

public class Day_2016_12_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 12;
	public override CodeType CodeType => CodeType.Original;

	Match[] instructions;
	Dictionary<string, int> registers;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"(?<instruction>\w{3}) (?<x>-?\d+|a|b|c|d)(?: (?<y>-?\d+|a|b|c|d))?", RegexOptions.Compiled);

		instructions =
			input.GetLines()
				.Select(s => regex.Match(s))
				.ToArray();

		registers = new Dictionary<string, int>()
			{
				{ "a", 0 },
				{ "b", 0 },
				{ "c", 0 },
				{ "d", 0 },
			};

		RunInstructions('A');

		registers = new Dictionary<string, int>()
			{
				{ "a", 0 },
				{ "b", 0 },
				{ "c", 1 },
				{ "d", 0 },
			};

		RunInstructions('B');
	}

	private void RunInstructions(char part)
	{
		var ip = 0;
		while (ip < instructions.Length)
		{
			var instruction = instructions[ip];
			switch (instruction.Groups["instruction"].Value)
			{
				case "cpy":
					{
						var dest = instruction.Groups["y"].Value;
						var value = registers.ContainsKey(instruction.Groups["x"].Value) ? registers[instruction.Groups["x"].Value] : Convert.ToInt32(instruction.Groups["x"].Value);
						registers[dest] = value;
						break;
					}

				case "inc":
					{
						var dest = instruction.Groups["x"].Value;
						registers[dest]++;
						break;
					}

				case "dec":
					{
						var dest = instruction.Groups["x"].Value;
						registers[dest]--;
						break;
					}

				case "jnz":
					{
						var value = registers.ContainsKey(instruction.Groups["x"].Value) ? registers[instruction.Groups["x"].Value] : Convert.ToInt32(instruction.Groups["x"].Value);
						if (value != 0)
						{
							var distance = Convert.ToInt32(instruction.Groups["y"].Value);
							ip += distance;
							continue;
						}
						else
							break;
					}
			}
			ip++;
		}

		Dump(part, registers["a"]);
	}
}
