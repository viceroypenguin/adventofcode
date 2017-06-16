<Query Kind="Program" />

void Main()
{
	var cpu = new CPU();
	var input =
@"jio a, +22
inc a
tpl a
tpl a
tpl a
inc a
tpl a
inc a
tpl a
inc a
inc a
tpl a
inc a
inc a
tpl a
inc a
inc a
tpl a
inc a
inc a
tpl a
jmp +19
tpl a
tpl a
tpl a
tpl a
inc a
inc a
tpl a
inc a
tpl a
inc a
inc a
tpl a
inc a
inc a
tpl a
inc a
tpl a
tpl a
jio a, +8
inc b
jie a, +4
tpl a
inc a
jmp +2
hlf a
jmp -7";

	var instructions = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(i=>Instruction.ParseInstruction(i))
		.ToArray();
	while (cpu.IP < instructions.Length)
		instructions[cpu.IP](cpu);

	cpu.Dump();
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

