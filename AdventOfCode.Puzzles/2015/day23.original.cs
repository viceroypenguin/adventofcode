namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 23, CodeType.Original)]
public partial class Day_23_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input
			.Lines
			.Select(Instruction.ParseInstruction)
			.ToArray();

		var cpu = new CPU();
		while (cpu.IP < instructions.Length)
			instructions[cpu.IP](cpu);
		var partA = cpu.B;

		cpu = new()
		{
			A = 1
		};

		while (cpu.IP < instructions.Length)
			instructions[cpu.IP](cpu);

		var partB = cpu.B;

		return (partA.ToString(), partB.ToString());
	}

	public class CPU
	{
		public uint A { get; set; }
		public uint B { get; set; }
		public uint IP { get; set; }
	}

	public abstract class Instruction
	{
		public abstract Regex Parser();
		public abstract void ProcessInstruction(Match instruction, CPU cpu);

		public static Action<CPU> ParseInstruction(string instruction)
		{
			foreach (var i in s_instructions)
			{
				var m = i.Parser().Match(instruction);
				if (m.Success)
					return (cpu) => i.ProcessInstruction(m, cpu);
			}

			throw new InvalidOperationException();
		}

		private static readonly Instruction[] s_instructions =
		[
			new Half(),
			new Third(),
			new Increment(),
			new Jump(),
			new JumpEven(),
			new JumpOne(),
		];
	}

	public partial class Half : Instruction
	{
		[GeneratedRegex(@"hlf (\w)", RegexOptions.Compiled)]
		public override partial Regex Parser();

		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			cpu.IP++;
			switch (instruction.Groups[1].Value)
			{
				case "a": cpu.A /= 2; break;
				case "b": cpu.B /= 2; break;
				default: throw new InvalidOperationException();
			}
		}
	}

	public partial class Third : Instruction
	{
		[GeneratedRegex(@"tpl (\w)", RegexOptions.Compiled)]
		public override partial Regex Parser();

		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			cpu.IP++;
			switch (instruction.Groups[1].Value)
			{
				case "a": cpu.A *= 3; break;
				case "b": cpu.B *= 3; break;
				default: throw new InvalidOperationException();
			}
		}
	}

	public partial class Increment : Instruction
	{
		[GeneratedRegex(@"inc (\w)", RegexOptions.Compiled)]
		public override partial Regex Parser();

		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			cpu.IP++;
			switch (instruction.Groups[1].Value)
			{
				case "a": cpu.A++; break;
				case "b": cpu.B++; break;
				default: throw new InvalidOperationException();
			}
		}
	}

	public partial class Jump : Instruction
	{
		[GeneratedRegex(@"jmp ((\+|\-)\d+)", RegexOptions.Compiled)]
		public override partial Regex Parser();

		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			var cnt = Convert.ToInt32(instruction.Groups[1].Value);
			cpu.IP = (uint)(cpu.IP + cnt);
		}
	}

	public partial class JumpEven : Instruction
	{
		[GeneratedRegex(@"jie (\w), ((\+|\-)\d+)", RegexOptions.Compiled)]
		public override partial Regex Parser();

		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			var reg = instruction.Groups[1].Value switch
			{
				"a" => cpu.A,
				"b" => cpu.B,
				_ => throw new InvalidOperationException(),
			};

			if (reg % 2 == 0)
			{
				var cnt = Convert.ToInt32(instruction.Groups[2].Value);
				cpu.IP = (uint)(cpu.IP + cnt);
			}
			else
			{
				cpu.IP++;
			}
		}
	}

	public partial class JumpOne : Instruction
	{
		[GeneratedRegex(@"jio (\w), ((\+|\-)\d+)", RegexOptions.Compiled)]
		public override partial Regex Parser();

		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			var reg = instruction.Groups[1].Value switch
			{
				"a" => cpu.A,
				"b" => cpu.B,
				_ => throw new InvalidOperationException(),
			};

			if (reg == 1)
			{
				var cnt = Convert.ToInt32(instruction.Groups[2].Value);
				cpu.IP = (uint)(cpu.IP + cnt);
			}
			else
			{
				cpu.IP++;
			}
		}
	}
}
