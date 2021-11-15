using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

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

	public IntCodeComputer(long[] memory, BufferBlock<long> inputs, BufferBlock<long> outputs)
	{
		this.memory = memory;
		this.inputs = inputs;
		this.outputs = outputs;

		this.ip = 0;
		this.relativeBase = 0L;
	}

	private readonly long[] memory;
	private readonly BufferBlock<long> inputs;
	private readonly BufferBlock<long> outputs;
	private int ip;
	private long relativeBase;

	public async Task RunProgram()
	{
		while (ip < memory.Length && memory[ip] != 99)
			await OpCodes[memory[ip] % 100](this);
	}

	private static readonly Dictionary<long, Func<IntCodeComputer, Task>> OpCodes =
		new Dictionary<long, Func<IntCodeComputer, Task>>
		{
			[1] = DoAddInstruction,
			[2] = DoMulInstruction,
			[3] = DoInputInstruction,
			[4] = DoOutputInstruction,
			[5] = DoJnzInstruction,
			[6] = DoJzInstruction,
			[7] = DoSetLtInstruction,
			[8] = DoSetEInstruction,
			[9] = DoAdjustBaseInstruction,
		};

	private static Task DoAddInstruction(IntCodeComputer computer)
	{
		var num1 = computer.GetParameter(1);
		var num2 = computer.GetParameter(2);
		computer.GetParameter(3) = num1 + num2;
		computer.ip += 4;
		return Task.CompletedTask;
	}

	private static Task DoMulInstruction(IntCodeComputer computer)
	{
		var num1 = computer.GetParameter(1);
		var num2 = computer.GetParameter(2);
		computer.GetParameter(3) = num1 * num2;
		computer.ip += 4;
		return Task.CompletedTask;
	}

	private static async Task DoInputInstruction(IntCodeComputer computer)
	{
		var value = await computer.inputs.ReceiveAsync();
		computer.GetParameter(1) = value;
		computer.ip += 2;
	}

	private static Task DoOutputInstruction(IntCodeComputer computer)
	{
		computer.outputs.Post(computer.GetParameter(1));
		computer.ip += 2;
		return Task.CompletedTask;
	}

	private static Task DoJnzInstruction(IntCodeComputer computer)
	{
		var num1 = computer.GetParameter(1);
		var num2 = computer.GetParameter(2);
		computer.ip = num1 == 0 ? computer.ip + 3 : (int)num2;
		return Task.CompletedTask;
	}

	private static Task DoJzInstruction(IntCodeComputer computer)
	{
		var num1 = computer.GetParameter(1);
		var num2 = computer.GetParameter(2);
		computer.ip = num1 != 0 ? computer.ip + 3 : (int)num2;
		return Task.CompletedTask;
	}

	private static Task DoSetLtInstruction(IntCodeComputer computer)
	{
		var num1 = computer.GetParameter(1);
		var num2 = computer.GetParameter(2);
		computer.GetParameter(3) = num1 < num2 ? 1 : 0;
		computer.ip += 4;
		return Task.CompletedTask;
	}

	private static Task DoSetEInstruction(IntCodeComputer computer)
	{
		var num1 = computer.GetParameter(1);
		var num2 = computer.GetParameter(2);
		computer.GetParameter(3) = num1 == num2 ? 1 : 0;
		computer.ip += 4;
		return Task.CompletedTask;
	}

	private static Task DoAdjustBaseInstruction(IntCodeComputer computer)
	{
		computer.relativeBase += computer.GetParameter(1);
		computer.ip += 2;
		return Task.CompletedTask;
	}
}
