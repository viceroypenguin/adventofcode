using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 16, CodeType.Fastest)]
public class Day_16_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		const ulong Identity = 0xfedcba9876543210UL;
		ulong swapRotate = Identity, permute = Identity;

		byte command = 0;
		int a = 0, b = 0;
		foreach (var c in span)
		{
			if (c > 'p')
			{
				command = c;
			}
			// p could be command or number
			// both other commands use numbers, 
			// so resetting command is ok
			else if (c == 'p') { command = c; a = 15; }
			else if (c >= 'a')
			{
				a = c - 'a';
			}
			else if (c >= '0')
			{
				a = (a * 10) + c - '0';
			}
			else if (c == '/') { b = a; a = 0; }
			else if (c is (byte)',' or (byte)'\n')
			{
				if (command == 'p')
					permute = NibbleSwap(permute, a, b);
				else if (command == 's')
					swapRotate = BitOperations.RotateLeft(swapRotate, a << 2);
				else if (command == 'x')
					swapRotate = NibbleSwap(swapRotate, a, b);
				a = b = command = 0;
			}
		}

		// permute instructions create the inverse transformation
		permute = Inverse(permute);

		var partA = Format(Compose(permute, swapRotate));

		ulong swapRotateB = Identity, permuteB = Identity;
		for (long n = 1_000_000_000; n != 0; n >>= 1)
		{
			if ((n & 0x1) != 0)
			{
				swapRotateB = Compose(swapRotateB, swapRotate);
				permuteB = Compose(permuteB, permute);
			}

			swapRotate = Compose(swapRotate, swapRotate);
			permute = Compose(permute, permute);
		}

		var partB = Format(Compose(permuteB, swapRotateB));

		return (partA, partB);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong NibbleSwap(ulong x, int a, int b)
	{
		var r = ((a <<= 2) - (b <<= 2)) & 63;
		var maskA = 15UL << a;
		var maskB = 15UL << b;
		return (x & ~(maskA | maskB)) |
			(BitOperations.RotateLeft(x, r) & maskA) |
			(BitOperations.RotateRight(x, r) & maskB);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong Inverse(ulong x)
	{
		var ret = 0UL;
		for (ulong i = 0; i != 16; i++)
			ret |= i << (int)((15 & (x >> (int)(i << 2))) << 2);
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong Compose(ulong x, ulong y)
	{
		var ret = 0UL;
		for (var i = 0; i != 64; i += 4)
			ret |= (15 & (x >> (int)((15 & (y >> i)) << 2))) << i;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string Format(ulong x)
	{
		var c = new char[16];
		for (var i = 0; i < 16; i++, x >>= 4)
			c[i] = (char)('a' + (x & 15));
		return new string(c);
	}
}
