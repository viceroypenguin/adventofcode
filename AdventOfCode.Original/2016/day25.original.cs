namespace AdventOfCode;

public class Day_2016_25_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 25;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"(?<i>\w{3}) (?<x>-?\d+|a|b|c|d)(?: (?<y>-?\d+|a|b|c|d))?", RegexOptions.Compiled);

		var instructions =
			input.GetLines()
				.Select(s => regex.Match(s))
				.Select(m => new
				{
					i = m.Groups["i"].Value,
					x = m.Groups["x"].Value,
					y = m.Groups["y"].Value,
				})
				.ToArray();

		var registers = new Dictionary<string, int>()
			{
				{ "a", 3 },
				{ "b", 0 },
				{ "c", 0 },
				{ "d", 0 },
			};

		Func<string, int> argumentValue = (s) => registers.ContainsKey(s) ? registers[s] : Convert.ToInt32(s);

		for (int i = 0; ; i++)
		{
			var str = new List<int>();
			registers["a"] = i;

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
							instructions[mip] = new
							{
								i = newCmd,
								x = instructions[mip].x,
								y = instructions[mip].y,
							};

							break;
						}

					case "out":
						{
							var value = argumentValue(ins.x);
							str.Add(value);

							if (str.Count == 10)
							{
								if (string.Join("", str) == "0101010101" ||
									string.Join("", str) == "1010101010")
								{
									Dump('A', i);
									return;
								}

								goto nextLoop;
							}
							break;
						}

					default:
						throw new InvalidOperationException("How did we get here?");
				}
				ip++;
			}

nextLoop:
			;
		}
	}
}
