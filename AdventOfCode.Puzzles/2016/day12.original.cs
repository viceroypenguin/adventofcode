using System.Diagnostics;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 12, CodeType.Original)]
public partial class Day_12_Original : IPuzzle
{
	[GeneratedRegex("(?<instruction>\\w{3}) (?<x>-?\\d+|a|b|c|d)(?: (?<y>-?\\d+|a|b|c|d))?", RegexOptions.Compiled)]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();

		var instructions =
			input.Lines
				.Select(s => regex.Match(s))
				.ToArray();

		var partA = RunInstructions(instructions, 0);
		var partB = RunInstructions(instructions, 1);

		return (partA.ToString(), partB.ToString());
	}

	private static int RunInstructions(Match[] instructions, int c)
	{
		var registers = new Dictionary<string, int>()
		{
			["a"] = 0,
			["b"] = 0,
			["c"] = c,
			["d"] = 0,
		};

		var ip = 0;
		while (ip < instructions.Length)
		{
			var instruction = instructions[ip];
			switch (instruction.Groups["instruction"].Value)
			{
				case "cpy":
				{
					var dest = instruction.Groups["y"].Value;
					if (!registers.TryGetValue(instruction.Groups["x"].Value, out var value))
						value = Convert.ToInt32(instruction.Groups["x"].Value);
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
					if (!registers.TryGetValue(instruction.Groups["x"].Value, out var value))
						value = Convert.ToInt32(instruction.Groups["x"].Value);
					if (value != 0)
					{
						var distance = Convert.ToInt32(instruction.Groups["y"].Value);
						ip += distance;
						continue;
					}
					else
					{
						break;
					}
				}

				default:
					throw new UnreachableException();
			}
			ip++;
		}

		return registers["a"];
	}
}
