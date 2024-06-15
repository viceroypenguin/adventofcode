using System.Diagnostics;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 25, CodeType.Original)]
public partial class Day_25_Original : IPuzzle
{
	[GeneratedRegex("(?<i>\\w{3}) (?<x>-?\\d+|a|b|c|d)(?: (?<y>-?\\d+|a|b|c|d))?", RegexOptions.Compiled)]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();

		var instructions =
			input.Lines
				.Select(s => regex.Match(s))
				.Select(m => new Instruction(
					m.Groups["i"].Value,
					m.Groups["x"].Value,
					m.Groups["y"].Value))
				.ToArray();

		for (var i = 0; ; i++)
		{
			if (RunProgram(instructions, i))
				return (i.ToString(), string.Empty);
		}

	}

	private sealed record Instruction(string I, string X, string Y);

	private static bool RunProgram(Instruction[] instructions, int a)
	{
		var registers = new Dictionary<string, int>()
		{
			["a"] = a,
			["b"] = 0,
			["c"] = 0,
			["d"] = 0,
		};

		int GetArgumentValue(string s) =>
			registers.TryGetValue(s, out var value)
				? value : Convert.ToInt32(s);

		var str = new List<int>();

		var ip = 0;
		while (ip < instructions.Length)
		{
			var ins = instructions[ip];
			switch (ins.I)
			{
				case "cpy":
				{
					var value = GetArgumentValue(ins.X);
					var dest = ins.Y;
					if (registers.ContainsKey(dest))
						registers[dest] = value;
					break;
				}

				case "inc":
				{
					var dest = ins.X;
					registers[dest]++;
					break;
				}

				case "dec":
				{
					var dest = ins.X;
					registers[dest]--;
					break;
				}

				case "jnz":
				{
					var value = GetArgumentValue(ins.X);
					if (value != 0)
					{
						var distance = GetArgumentValue(ins.Y);
						ip += distance;
						continue;
					}
					else
					{
						break;
					}
				}

				case "tgl":
				{
					var value = GetArgumentValue(ins.X);
					var mip = value + ip;
					if (mip >= instructions.Length) break;

					var oldCmd = instructions[mip].I;
					var newCmd =
						oldCmd == "jnz" ? "cpy" :
						oldCmd == "cpy" ? "jnz" :
						oldCmd == "inc" ? "dec" :
						oldCmd == "dec" ? "inc" :
						oldCmd == "tgl" ? "inc" :
						"";
					instructions[mip] = new(
						newCmd,
						instructions[mip].X,
						instructions[mip].Y);

					break;
				}

				case "out":
				{
					var value = GetArgumentValue(ins.X);
					str.Add(value);

					if (str.Count == 10)
					{
						return string.Join("", str)
							is "0101010101"
							or "1010101010";
					}

					break;
				}

				default:
					throw new UnreachableException();
			}

			ip++;
		}

		return false;
	}
}
