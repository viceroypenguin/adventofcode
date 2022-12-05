using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 5, CodeType.Fastest)]
public partial class Day_05_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<byte> stackData1 = stackalloc byte[64 * 10];
		Span<byte> stackLengths1 = stackalloc byte[10];

		Span<byte> stackData2 = stackalloc byte[64 * 10];
		Span<byte> stackLengths2 = stackalloc byte[10];

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static void PushStack(Span<byte> stackData, Span<byte> stackLengths, byte stack, byte value)
		{
			var i = stackLengths[stack]++;
			stackData[stack * 64 + i] = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static byte PopStack(Span<byte> stackData, Span<byte> stackLengths, byte stack)
		{
			var i = --stackLengths[stack];
			return stackData[stack * 64 + i];
		}

		// find end of map
		var lineCnt = 0;
		for (lineCnt = 0; input.Lines[lineCnt][1] != '1'; lineCnt++)
			;

		// extract map to stacks for p1 and p2
		var instructionLine = lineCnt + 2;
		for (lineCnt--; lineCnt >= 0; lineCnt--)
		{
			var l = input.Lines[lineCnt].AsSpan();
			for (byte stack = 0; stack < 9; stack++)
			{
				var c = (byte)l[stack * 4 + 1];
				if (c == ' ') continue;

				PushStack(stackData1, stackLengths1, stack, c);
				PushStack(stackData2, stackLengths2, stack, c);
			}
		}

		Span<byte> tmpStack = stackalloc byte[64];

		for (; instructionLine < input.Lines.Length; instructionLine++)
		{
			var l = input.Lines[instructionLine].AsSpan();
			var i = 5;
			var cnt = l[i] - '0';
			if (l[i + 1] != ' ')
			{
				i++;
				cnt = cnt * 10 + l[i] - '0';
			}

			i += 7;
			var from = (byte)(l[i] - '0' - 1);
			i += 5;
			var to = (byte)(l[i] - '0' - 1);

			for (i = 0; i < cnt; i++)
			{
				var c = PopStack(stackData1, stackLengths1, from);
				PushStack(stackData1, stackLengths1, to, c);

				c = PopStack(stackData2, stackLengths2, from);
				tmpStack[i] = c;
			}

			for (i--; i >= 0; i--)
			{
				PushStack(stackData2, stackLengths2, to, tmpStack[i]);
			}
		}

		var chars = MemoryMarshal.Cast<byte, char>(tmpStack)[0..9];

		for (byte i = 0; i < 9; i++)
			chars[i] = (char)PopStack(stackData1, stackLengths1, i);
		var part1 = new string(chars);

		for (byte i = 0; i < 9; i++)
			chars[i] = (char)PopStack(stackData2, stackLengths2, i);
		var part2 = new string(chars);

		return (part1, part2);
	}
}
