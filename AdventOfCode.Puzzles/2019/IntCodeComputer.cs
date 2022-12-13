using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2019;

public enum ProgramStatus
{
	WaitingToRun,
	Running,
	WaitingForInput,
	Completed,
}

internal sealed class IntCodeComputer
{
	private static readonly int[] PowersOfTen = new[] { 1, 10, 100, 1000, 10000, 100000, };

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private ref long GetParameter(int parameter)
	{
		ref long value = ref memory[ip + parameter];
		var mode = GetParameterMode(memory[ip], parameter);
		return
			ref mode == 1 ? ref value :
			ref mode == 2 ? ref memory[value + relativeBase] :
			ref memory[value];
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static long GetParameterMode(long opCode, int parameter) =>
		opCode / PowersOfTen[parameter + 1] % 10;

	public IntCodeComputer(
		long[] instructions,
		Queue<long> inputs = default,
		Queue<long> outputs = default,
		int size = 64 * 1024)
	{
		memory = new long[size];
		instructions.CopyTo(memory, 0);

		this.Inputs = inputs ?? new();
		this.Outputs = outputs ?? new();

		this.ip = 0;
		this.relativeBase = 0L;
	}

	public IReadOnlyList<long> Memory => memory;
	public Queue<long> Inputs { get; }
	public Queue<long> Outputs { get; }

	public ProgramStatus ProgramStatus { get; private set; } = ProgramStatus.WaitingToRun;

	private readonly long[] memory;
	private int ip;
	private long relativeBase;

	public ProgramStatus RunProgram()
	{
		ProgramStatus = ProgramStatus.Running;

		while (ip < memory.Length && memory[ip] != 99)
		{
			switch (memory[ip] % 100)
			{
				case 1: DoAddInstruction(); break;
				case 2: DoMulInstruction(); break;
				case 3: 
					if (!DoInputInstruction()) 
						return ProgramStatus = ProgramStatus.WaitingForInput; 
					break;
				case 4: DoOutputInstruction(); break;
				case 5: DoJnzInstruction(); break;
				case 6: DoJzInstruction(); break;
				case 7: DoSetLtInstruction(); break;
				case 8: DoSetEInstruction(); break;
				case 9: DoAdjustBaseInstruction(); break;
				case 99: return ProgramStatus.Completed;
			}
		}
		return ProgramStatus = ProgramStatus.Completed;
	}

	private void DoAddInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		GetParameter(3) = num1 + num2;
		ip += 4;
	}

	private void DoMulInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		GetParameter(3) = num1 * num2;
		ip += 4;
	}

	private bool DoInputInstruction()
	{
		if (Inputs.Count == 0)
			return false;
		var value = Inputs.Dequeue();
		GetParameter(1) = value;
		ip += 2;
		return true;
	}

	private void DoOutputInstruction()
	{
		Outputs.Enqueue(GetParameter(1));
		ip += 2;
	}

	private void DoJnzInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		ip = num1 == 0 ? ip + 3 : (int)num2;
	}

	private void DoJzInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		ip = num1 != 0 ? ip + 3 : (int)num2;
	}

	private void DoSetLtInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		GetParameter(3) = num1 < num2 ? 1 : 0;
		ip += 4;
	}

	private void DoSetEInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		GetParameter(3) = num1 == num2 ? 1 : 0;
		ip += 4;
	}

	private void DoAdjustBaseInstruction()
	{
		relativeBase += GetParameter(1);
		ip += 2;
	}
}
