using System.Diagnostics;
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
	private static readonly int[] s_powersOfTen = [1, 10, 100, 1000, 10000, 100000];

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private ref long GetParameter(int parameter)
	{
		ref var value = ref _memory[_ip + parameter];
		var mode = GetParameterMode(_memory[_ip], parameter);
		return
			ref mode == 1 ? ref value :
			ref mode == 2 ? ref _memory[value + _relativeBase] :
			ref _memory[value];
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static long GetParameterMode(long opCode, int parameter) =>
		opCode / s_powersOfTen[parameter + 1] % 10;

	public IntCodeComputer(
		long[] instructions,
		Queue<long> inputs = default,
		Queue<long> outputs = default,
		int size = 64 * 1024)
	{
		_memory = new long[size];
		instructions.CopyTo(_memory, 0);

		Inputs = inputs ?? new();
		Outputs = outputs ?? new();

		_ip = 0;
		_relativeBase = 0L;
	}

	public IReadOnlyList<long> Memory => _memory;
	public Queue<long> Inputs { get; }
	public Queue<long> Outputs { get; }

	public ProgramStatus ProgramStatus { get; private set; } = ProgramStatus.WaitingToRun;

	private readonly long[] _memory;
	private int _ip;
	private long _relativeBase;

	public ProgramStatus RunProgram()
	{
		ProgramStatus = ProgramStatus.Running;

		while (_ip < _memory.Length && _memory[_ip] != 99)
		{
			switch (_memory[_ip] % 100)
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
				default: throw new UnreachableException();
			}
		}

		return ProgramStatus = ProgramStatus.Completed;
	}

	private void DoAddInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		GetParameter(3) = num1 + num2;
		_ip += 4;
	}

	private void DoMulInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		GetParameter(3) = num1 * num2;
		_ip += 4;
	}

	private bool DoInputInstruction()
	{
		if (Inputs.Count == 0)
			return false;
		var value = Inputs.Dequeue();
		GetParameter(1) = value;
		_ip += 2;
		return true;
	}

	private void DoOutputInstruction()
	{
		Outputs.Enqueue(GetParameter(1));
		_ip += 2;
	}

	private void DoJnzInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		_ip = num1 == 0 ? _ip + 3 : (int)num2;
	}

	private void DoJzInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		_ip = num1 != 0 ? _ip + 3 : (int)num2;
	}

	private void DoSetLtInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		GetParameter(3) = num1 < num2 ? 1 : 0;
		_ip += 4;
	}

	private void DoSetEInstruction()
	{
		var num1 = GetParameter(1);
		var num2 = GetParameter(2);
		GetParameter(3) = num1 == num2 ? 1 : 0;
		_ip += 4;
	}

	private void DoAdjustBaseInstruction()
	{
		_relativeBase += GetParameter(1);
		_ip += 2;
	}
}
