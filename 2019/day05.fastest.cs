using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MoreLinq;

namespace AdventOfCode
{
	public unsafe class Day_2019_05_Fastest : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 5;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) { RunProgram(null, 0, 0, out var _); return; }

			var nums = stackalloc int[input.Length / 2];
			int numCount = 0, n = 0, sign = 1;
			foreach (var c in input)
			{
				if (c == ',')
				{
					nums[numCount++] = sign * n;
					n = 0;
					sign = 1;
				}
				else if (c == '-')
					sign = -1;
				else if (c >= '0')
					n = n * 10 + c - '0';
			}
			nums[numCount++] = n;

			var copy = stackalloc int[numCount];
			Unsafe.CopyBlock((void*)copy, (void*)nums, (uint)numCount * sizeof(int));

			RunProgram(copy, numCount, 1, out var progOutput);
			PartA = progOutput.ToString();

			Unsafe.CopyBlock((void*)copy, (void*)nums, (uint)numCount * sizeof(int));

			RunProgram(copy, numCount, 5, out progOutput);
			PartB = progOutput.ToString();
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void RunProgram(int* instructions, int instructionCount, int progInput, out int progOutput)
		{
			progOutput = 0;
			if (instructions == null) return;

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
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static int GetParameter(int* instructions, int ip, int parameter)
		{
			var value = instructions[ip + parameter];
			return GetParameterMode(instructions[ip], parameter) != 0
				? value
				: instructions[value];
		}

		private static readonly IReadOnlyList<int> powersOfTen = new[] { 1, 10, 100, 1000, 10000, 100000, };
		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static int GetParameterMode(int opCode, int parameter) =>
			opCode / powersOfTen[parameter + 1] % 10;


		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void DoAddInstruction(int* instructions, ref int ip)
		{
			var num1 = GetParameter(instructions, ip, 1);
			var num2 = GetParameter(instructions, ip, 2);
			instructions[instructions[ip + 3]] = num1 + num2;
			ip += 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void DoMulInstruction(int* instructions, ref int ip)
		{
			var num1 = GetParameter(instructions, ip, 1);
			var num2 = GetParameter(instructions, ip, 2);
			instructions[instructions[ip + 3]] = num1 * num2;
			ip += 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void DoInputInstruction(int* instructions, ref int ip, int progInput)
		{
			instructions[instructions[ip + 1]] = progInput;
			ip += 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void DoOutputInstruction(int* instructions, ref int ip, ref int progOutput)
		{
			progOutput = GetParameter(instructions, ip, 1);
			ip += 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void DoJumpTrueInstruction(int* instructions, ref int ip)
		{
			var num1 = GetParameter(instructions, ip, 1);
			var num2 = GetParameter(instructions, ip, 2);
			ip = num1 == 0 ? ip + 3 : num2;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void DoJumpFalseInstruction(int* instructions, ref int ip)
		{
			var num1 = GetParameter(instructions, ip, 1);
			var num2 = GetParameter(instructions, ip, 2);
			ip = num1 != 0 ? ip + 3 : num2;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void DoSetLessThanInstruction(int* instructions, ref int ip)
		{
			var num1 = GetParameter(instructions, ip, 1);
			var num2 = GetParameter(instructions, ip, 2);
			instructions[instructions[ip + 3]] = num1 < num2 ? 1 : 0;
			ip += 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void DoSetEqualInstruction(int* instructions, ref int ip)
		{
			var num1 = GetParameter(instructions, ip, 1);
			var num2 = GetParameter(instructions, ip, 2);
			instructions[instructions[ip + 3]] = num1 == num2 ? 1 : 0;
			ip += 4;
		}
	}
}
