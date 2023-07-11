using System.Diagnostics;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 23, CodeType.Original)]
public partial class Day_23_Original : IPuzzle
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

		instructions[5] = new("cpy", "c", "a");
		instructions[6] = new("mul", "d", "a");
		instructions[7] = new("cpy", "0", "c");
		instructions[7] = new("cpy", "0", "d");
		instructions[8] = new("nop", "", "");

		return (
			ExecutePart(instructions.ToArray(), 7).ToString(),
			ExecutePart(instructions.ToArray(), 12).ToString());
	}

	private record Instruction(string I, string X, string Y);

	private static int ExecutePart(Instruction[] instructions, int a)
	{
		var registers = new Dictionary<string, int>()
		{
			["a"] = a,
			["b"] = 0,
			["c"] = 0,
			["d"] = 0,
		};

		int GetArgumentValue(string s) => registers.ContainsKey(s) ? registers[s] : Convert.ToInt32(s);

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

				case "mul":
				{
					registers[ins.Y] *= registers[ins.X];
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

				case "nop":
					break;

				default:
					throw new UnreachableException();
			}
			ip++;
		}

		return registers["a"];
	}
}
