using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 5, CodeType.Fastest)]
public class Day_05_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<int> nums = stackalloc int[input.Bytes.Length / 2];
		int numCount = 0, n = 0, sign = 1;
		foreach (var c in input.Bytes)
		{
			if (c == ',')
			{
				nums[numCount++] = sign * n;
				n = 0;
				sign = 1;
			}
			else if (c == '-')
			{
				sign = -1;
			}
			else if (c >= '0')
			{
				n = (n * 10) + c - '0';
			}
		}

		nums[numCount++] = n;
		nums = nums[..numCount];

		Span<int> copy = stackalloc int[numCount];
		nums.CopyTo(copy);

		RunProgram(copy, numCount, 1, out var progOutput);
		var part1 = progOutput.ToString();

		nums.CopyTo(copy);

		RunProgram(copy, numCount, 5, out progOutput);
		var part2 = progOutput.ToString();

		return (part1, part2);
	}

	private static void RunProgram(Span<int> instructions, int instructionCount, int progInput, out int progOutput)
	{
		progOutput = 0;

		var ip = 0;
		while (ip < instructionCount && instructions[ip] != 99)
		{
			switch (instructions[ip] % 100)
			{
				case 1:
					DoAddInstruction(instructions, ref ip);
					break;
				case 2:
					DoMulInstruction(instructions, ref ip);
					break;
				case 3:
					DoInputInstruction(instructions, ref ip, progInput);
					break;
				case 4:
					DoOutputInstruction(instructions, ref ip, ref progOutput);
					break;
				case 5:
					DoJumpTrueInstruction(instructions, ref ip);
					break;
				case 6:
					DoJumpFalseInstruction(instructions, ref ip);
					break;
				case 7:
					DoSetLessThanInstruction(instructions, ref ip);
					break;
				case 8:
					DoSetEqualInstruction(instructions, ref ip);
					break;
				default:
					throw new UnreachableException();
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static int GetParameter(Span<int> instructions, int ip, int parameter)
	{
		var value = instructions[ip + parameter];
		return GetParameterMode(instructions[ip], parameter) != 0
			? value
			: instructions[value];
	}

	private static readonly IReadOnlyList<int> s_powersOfTen = [1, 10, 100, 1000, 10000, 100000,];
	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static int GetParameterMode(int opCode, int parameter) =>
		opCode / s_powersOfTen[parameter + 1] % 10;

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void DoAddInstruction(Span<int> instructions, ref int ip)
	{
		var num1 = GetParameter(instructions, ip, 1);
		var num2 = GetParameter(instructions, ip, 2);
		instructions[instructions[ip + 3]] = num1 + num2;
		ip += 4;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void DoMulInstruction(Span<int> instructions, ref int ip)
	{
		var num1 = GetParameter(instructions, ip, 1);
		var num2 = GetParameter(instructions, ip, 2);
		instructions[instructions[ip + 3]] = num1 * num2;
		ip += 4;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void DoInputInstruction(Span<int> instructions, ref int ip, int progInput)
	{
		instructions[instructions[ip + 1]] = progInput;
		ip += 2;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void DoOutputInstruction(Span<int> instructions, ref int ip, ref int progOutput)
	{
		progOutput = GetParameter(instructions, ip, 1);
		ip += 2;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void DoJumpTrueInstruction(Span<int> instructions, ref int ip)
	{
		var num1 = GetParameter(instructions, ip, 1);
		var num2 = GetParameter(instructions, ip, 2);
		ip = num1 == 0 ? ip + 3 : num2;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void DoJumpFalseInstruction(Span<int> instructions, ref int ip)
	{
		var num1 = GetParameter(instructions, ip, 1);
		var num2 = GetParameter(instructions, ip, 2);
		ip = num1 != 0 ? ip + 3 : num2;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void DoSetLessThanInstruction(Span<int> instructions, ref int ip)
	{
		var num1 = GetParameter(instructions, ip, 1);
		var num2 = GetParameter(instructions, ip, 2);
		instructions[instructions[ip + 3]] = num1 < num2 ? 1 : 0;
		ip += 4;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void DoSetEqualInstruction(Span<int> instructions, ref int ip)
	{
		var num1 = GetParameter(instructions, ip, 1);
		var num2 = GetParameter(instructions, ip, 2);
		instructions[instructions[ip + 3]] = num1 == num2 ? 1 : 0;
		ip += 4;
	}
}
