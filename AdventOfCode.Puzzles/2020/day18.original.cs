using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 18, CodeType.Original)]
public class Day_18_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<long> stack = stackalloc long[64];
		int stackLevel = -1;

		long grandSum = 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void NextLine(Span<long> stack)
		{
			grandSum += Pop(stack);
			if (stackLevel != -1) throw new InvalidOperationException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Push(Span<long> stack, long value) =>
				stack[++stackLevel] = value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Operate(Span<long> stack, long val) =>
				Push(
					stack,
					Pop(stack) switch
					{
						-1 => Pop(stack) + val,
						-2 => Pop(stack) * val,
					});

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		long Pop(Span<long> stack) =>
				stack[stackLevel--];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ProcessNumber(Span<long> stack, long val)
		{
			if (stackLevel == -1 || stack[stackLevel] == -3)
				Push(stack, val);
			else
				Operate(stack, val);
		}

		foreach (var c in input.Bytes)
		{
			switch (c)
			{
				case (byte)' ':
					break;

				case (byte)'\n':
					NextLine(stack);
					break;

				case (byte)'+':
					Push(stack, -1);
					break;

				case (byte)'*':
					Push(stack, -2);
					break;

				case (byte)'(':
					Push(stack, -3);
					break;

				case (byte)')':
				{
					var val = Pop(stack);
					Pop(stack);
					ProcessNumber(stack, val);
					break;
				}

				default:
				{
					// since all ints in problem are 1-char long...
					int val = c - (byte)'0';
					ProcessNumber(stack, val);
					break;
				}
			}
		}

		var part1 = grandSum.ToString();

		grandSum = 0;
		foreach (var c in input.Bytes)
		{
			switch (c)
			{
				case (byte)' ':
					break;

				case (byte)'\n':
				{
					var val = Pop(stack);
					while (stackLevel > 0 && Pop(stack) == -2)
						val *= Pop(stack);
					Push(stack, val);
					NextLine(stack);
					break;
				}

				case (byte)'+':
					Push(stack, -1);
					break;

				case (byte)'*':
					Push(stack, -2);
					break;

				case (byte)'(':
					Push(stack, -3);
					break;

				case (byte)')':
				{
					var val = Pop(stack);
					while (Pop(stack) == -2)
						val *= Pop(stack);
					if (stackLevel > 0 && stack[stackLevel] == -1)
						Operate(stack, val);
					else
						Push(stack, val);

					break;
				}

				default:
				{
					// since all ints in problem are 1-char long...
					int val = c - (byte)'0';
					if (stackLevel > 0 && stack[stackLevel] == -1)
						Operate(stack, val);
					else
						Push(stack, val);
					break;
				}
			}
		}

		var part2 = grandSum.ToString();

		return (part1, part2);
	}
}
