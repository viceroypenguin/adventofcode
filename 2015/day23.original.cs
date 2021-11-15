namespace AdventOfCode;

public class Day_2015_23_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 23;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var instructions = input
			.GetLines()
			.Select(i => Instruction.ParseInstruction(i))
			.ToArray();

		var cpu = new CPU();
		while (cpu.IP < instructions.Length)
			instructions[cpu.IP](cpu);
		Dump('A', cpu.B);

		cpu = new CPU();
		cpu.A = 1;
		while (cpu.IP < instructions.Length)
			instructions[cpu.IP](cpu);
		Dump('B', cpu.B);
	}

	public class CPU
	{
		public uint A { get; set; } = 0;
		public uint B { get; set; } = 0;
		public uint IP { get; set; } = 0;
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
