using System.Diagnostics;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 23, CodeType.Original)]
public partial class Day_23_Original : IPuzzle
{
	[GeneratedRegex("^(?<inst>set|sub|mul|jnz) (?<dst>\\w|-?\\d+)( (?<src>\\w|-?\\d+))?$", RegexOptions.Compiled)]
	private static partial Regex InstructionRegex();

	private sealed class Instruction
	{
		public string Operation { get; set; }
		public string Source { get; set; }
		public string Destination { get; set; }
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();
		var instructions = input.Lines
			.Select(inst => regex.Match(inst))
			.Select(m => new Instruction
			{
				Operation = m.Groups["inst"].Value,
				Source = m.Groups["src"].Value,
				Destination = m.Groups["dst"].Value,
			})
			.ToList();

		return (
			ProcessInstructions(instructions).ToString(),
			CountComposites(instructions).ToString());
	}

	private static long ProcessInstructions(IList<Instruction> input)
	{
		var registers = new Dictionary<string, long>();

		long GetValue(string src) =>
			int.TryParse(src, out var x)
				? x
				: registers.GetValueOrDefault(src);

		var ip = 0;
		var mulInstructions = 0;

		while (ip < input.Count)
		{
			var instruction = input[ip];
			switch (instruction.Operation)
			{
				case "set":
				{
					registers[instruction.Destination] =
						GetValue(instruction.Source);
					break;
				}

				case "sub":
				{
					var register = registers.GetValueOrDefault(instruction.Destination);
					register -= GetValue(instruction.Source);
					registers[instruction.Destination] = register;
					break;
				}

				case "mul":
				{
					mulInstructions++;
					var register = registers.GetValueOrDefault(instruction.Destination);
					register *= GetValue(instruction.Source);
					registers[instruction.Destination] = register;
					break;
				}

				case "jnz":
				{
					var value = GetValue(instruction.Destination);
					if (value != 0)
					{
						ip += (int)GetValue(instruction.Source);
						continue;
					}

					break;
				}

				default:
					throw new UnreachableException();
			}

			ip++;
		}

		return mulInstructions;
	}

	private static long CountComposites(IList<Instruction> input)
	{
		var initial = (Convert.ToInt32(input[0].Source) * 100) + 100000;
		var max = initial - Convert.ToInt32(input[7].Source);
		var maxFactor = (int)Math.Sqrt(max);
		var increment = -Convert.ToInt32(input[30].Source);

		var composites = 0;
		for (var x = initial; x <= max; x += increment)
		{
			if (x % 2 == 0)
			{
				composites++;
				continue;
			}

			for (var n = 3; n <= maxFactor; n += 2)
			{
				if (x % n == 0)
				{
					composites++;
					break;
				}
			}
		}

		return composites;
	}
}
