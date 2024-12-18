namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 17, CodeType.Original)]
public partial class Day_17_Original : IPuzzle
{
	[GeneratedRegex(@"Register \w: (\d+)")]
	private static partial Regex RegisterParseRegex { get; }

	public (string, string) Solve(PuzzleInput input)
	{
		var splits = input.Lines.Split(string.Empty).ToList();
		var registerA = int.Parse(RegisterParseRegex.Match(splits[0][0]).Groups[1].Value);
		var instructionString = splits[1][0][9..];
		var instructions = instructionString.Split(',').Select(i => i[0] - '0').ToList();

		var part1 = string.Join(',', RunProgram(instructions, registerA));

		var part2 = 0L;
		for (var i = 0; i < instructions.Count; i++)
		{
			var num = part2 * 8;
			for (var j = 0; ; j++)
			{
				var values = RunProgram(instructions, num + j);
				if (values.Count == i + 1 && instructions.EndsWith(values))
				{
					part2 = num + j;
					break;
				}
			}
		}

		return (part1, part2.ToString());
	}

	private static List<int> RunProgram(List<int> instructions, long registerA)
	{
		var a = registerA;
		var b = 0L;
		var c = 0L;

		var ip = 0;

		var output = new List<int>();
		while (ip + 1 < instructions.Count)
		{
			var combo = instructions[ip + 1] switch
			{
				0 => 0,
				1 => 1,
				2 => 2,
				3 => 3,
				4 => a,
				5 => b,
				6 => c,
				_ => -1,
			};

			switch (instructions[ip])
			{
				case 0:
					a /= 1L << (int)combo;
					ip += 2;
					break;

				case 1:
					b ^= instructions[ip + 1];
					ip += 2;
					break;

				case 2:
					b = combo & 0x7;
					ip += 2;
					break;

				case 3:
					if (a != 0)
						ip = instructions[ip + 1];
					else
						ip += 2;

					break;

				case 4:
					b ^= c;
					ip += 2;
					break;

				case 5:
					output.Add((int)combo & 0x7);
					ip += 2;
					break;

				case 6:
					b = a / (1L << (int)combo);
					ip += 2;
					break;

				case 7:
					c = a / (1L << (int)combo);
					ip += 2;
					break;
			}
		}

		return output;
	}
}
