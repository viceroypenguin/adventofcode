namespace AdventOfCode;

public class Day_2016_23_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 23;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var regex = new Regex(@"(?<i>\w{3}) (?<x>-?\d+|a|b|c|d)(?: (?<y>-?\d+|a|b|c|d))?", RegexOptions.Compiled);

		var instructions =
			input.GetLines()
				.Select(s => regex.Match(s))
				.Select(m => new Instruction
				{
					i = m.Groups["i"].Value,
					x = m.Groups["x"].Value,
					y = m.Groups["y"].Value,
				})
				.ToArray();

		ExecutePart(
			instructions,
			new Dictionary<string, int>()
			{
					{ "a", 7 },
					{ "b", 0 },
					{ "c", 0 },
					{ "d", 0 },
			},
			'A');

		ExecutePart(
			instructions,
			new Dictionary<string, int>()
			{
					{ "a", 12 },
					{ "b", 0 },
					{ "c", 0 },
					{ "d", 0 },
			},
			'B');
	}

	private void ExecutePart(Instruction[] instructions, Dictionary<string, int> registers, char part)
	{
		Func<string, int> argumentValue = (s) => registers.ContainsKey(s) ? registers[s] : Convert.ToInt32(s);

		var ip = 0;
		while (ip < instructions.Length)
		{
			var ins = instructions[ip];
			switch (ins.i)
			{
				case "cpy":
					{
						var value = argumentValue(ins.x);
						var dest = ins.y;
						if (registers.ContainsKey(dest))
							registers[dest] = value;
						break;
					}

				case "inc":
					{
						var dest = ins.x;
						registers[dest]++;
						break;
					}

				case "dec":
					{
						var dest = ins.x;
						registers[dest]--;
						break;
					}

				case "jnz":
					{
						var value = argumentValue(ins.x);
						if (value != 0)
						{
							var distance = argumentValue(ins.y);
							ip += distance;
							continue;
						}
						else
							break;
					}

				case "tgl":
					{
						var value = argumentValue(ins.x);
						var mip = value + ip;
						if (mip >= instructions.Length) break;

						var oldCmd = instructions[mip].i;
						var newCmd =
							oldCmd == "jnz" ? "cpy" :
							oldCmd == "cpy" ? "jnz" :
							oldCmd == "inc" ? "dec" :
							oldCmd == "dec" ? "inc" :
							oldCmd == "tgl" ? "inc" :
							"";
						instructions[mip] = new Instruction
						{
							i = newCmd,
							x = instructions[mip].x,
							y = instructions[mip].y,
						};

						break;
					}

				default:
					throw new InvalidOperationException("How did we get here?");
			}
			ip++;
		}

		Dump(part, registers["a"]);
	}

	private class Instruction
	{
		public string i { get; set; }
		public string x { get; set; }
		public string y { get; set; }
	}
}
