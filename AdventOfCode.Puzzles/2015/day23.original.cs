namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 23, CodeType.Original)]
public class Day_23_Original : IPuzzle
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

		cpu = new CPU
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
		public abstract Regex Parser { get; }
		public abstract void ProcessInstruction(Match instruction, CPU cpu);

		public static Action<CPU> ParseInstruction(string instruction)
		{
			foreach (var i in _Instructions)
			{
				var m = i.Parser.Match(instruction);
				if (m.Success)
					return (cpu) => i.ProcessInstruction(m, cpu);
			}

			throw new InvalidOperationException();
		}

		private static Instruction[] _Instructions = new Instruction[]
		{
			new Half(),
			new Third(),
			new Increment(),
			new Jump(),
			new JumpEven(),
			new JumpOne(),
		};
	}

	public class Half : Instruction
	{
		public override Regex Parser => new Regex(@"hlf (\w)", RegexOptions.Compiled);
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

	public class Third : Instruction
	{
		public override Regex Parser => new Regex(@"tpl (\w)", RegexOptions.Compiled);
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

	public class Increment : Instruction
	{
		public override Regex Parser => new Regex(@"inc (\w)", RegexOptions.Compiled);
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

	public class Jump : Instruction
	{
		public override Regex Parser => new Regex(@"jmp ((\+|\-)\d+)", RegexOptions.Compiled);
		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			var cnt = Convert.ToInt32(instruction.Groups[1].Value);
			cpu.IP = (uint)(cpu.IP + cnt);
		}
	}

	public class JumpEven : Instruction
	{
		public override Regex Parser => new Regex(@"jie (\w), ((\+|\-)\d+)", RegexOptions.Compiled);
		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			uint reg;
			switch (instruction.Groups[1].Value)
			{
				case "a": reg = cpu.A; break;
				case "b": reg = cpu.B; break;
				default: throw new InvalidOperationException();
			}

			if (reg % 2 == 0)
			{
				var cnt = Convert.ToInt32(instruction.Groups[2].Value);
				cpu.IP = (uint)(cpu.IP + cnt);
			}
			else
				cpu.IP++;
		}
	}

	public class JumpOne : Instruction
	{
		public override Regex Parser => new Regex(@"jio (\w), ((\+|\-)\d+)", RegexOptions.Compiled);
		public override void ProcessInstruction(Match instruction, CPU cpu)
		{
			uint reg;
			switch (instruction.Groups[1].Value)
			{
				case "a": reg = cpu.A; break;
				case "b": reg = cpu.B; break;
				default: throw new InvalidOperationException();
			}

			if (reg == 1)
			{
				var cnt = Convert.ToInt32(instruction.Groups[2].Value);
				cpu.IP = (uint)(cpu.IP + cnt);
			}
			else
				cpu.IP++;
		}
	}
}
