using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 15, CodeType.Fastest)]
public class Day_15_Fastest : IPuzzle
{
	private const ulong GenA = 16807;
	private const ulong GenA4 = GenA * GenA * GenA * GenA % 0x7fff_ffff;
	private const ulong GenB = 48271;
	private const ulong GenB4 = GenB * GenB * GenB * GenB % 0x7fff_ffff;

	private static readonly Vector256<ulong> s_aMul = Vector256.Create(GenA4, GenA4, GenA4, GenA4);
	private static readonly Vector256<ulong> s_bMul = Vector256.Create(GenB4, GenB4, GenB4, GenB4);

	private static readonly Vector256<ulong> s_and31Const = Vector256.Create(0x7fff_fffful, 0x7fff_fffful, 0x7fff_fffful, 0x7fff_fffful);
	private static readonly Vector256<ulong> s_and16Const = Vector256.Create(0xfffful, 0xfffful, 0xfffful, 0xfffful);
	private static readonly Vector256<ulong> s_and3Const = Vector256.Create(0x07ul, 0x07ul, 0x07ul, 0x07ul);
	private static readonly Vector256<ulong> s_and2Const = Vector256.Create(0x03ul, 0x03ul, 0x03ul, 0x03ul);

	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day15.c
		var span = input.Span;

		ulong aKey = 0, bKey = 0;
		for (var i = 0; i < span.Length; i++)
		{
			var c = span[i];
			if (c == '\n' && aKey == 0)
			{
				aKey = bKey;
				bKey = 0;
			}
			else if (c is >= (byte)'0' and <= (byte)'9')
			{
				bKey = (bKey * 10) + c - '0';
			}
		}

		var a0 = GetInitialVectors(aKey, GenA);
		var b0 = GetInitialVectors(bKey, GenB);

		var a = a0; var b = b0;
		var sums = Vector256<ulong>.Zero;
		for (var i = 0; i < 40_000_000 / 4; i++)
		{
			a = Generate(a, s_aMul);
			b = Generate(b, s_bMul);

			sums = Avx2.Subtract(sums, Avx2.CompareEqual(Avx2.And(a, s_and16Const), Avx2.And(b, s_and16Const)));
		}

		var partA = (Vector256.GetElement(sums, 0) + Vector256.GetElement(sums, 1) + Vector256.GetElement(sums, 2) + Vector256.GetElement(sums, 3)).ToString();

		a = a0; b = b0;
		var aMask = Avx.MoveMask(Avx2.CompareEqual(Avx2.And(a, s_and2Const), Vector256<ulong>.Zero).AsDouble());
		var bMask = Avx.MoveMask(Avx2.CompareEqual(Avx2.And(b, s_and3Const), Vector256<ulong>.Zero).AsDouble());
		var matches = 0;
		for (var i = 0; i < 5_000_000; i++)
		{
			while (aMask == 0)
			{
				a = Generate(a, s_aMul);
				aMask = Avx.MoveMask(Avx2.CompareEqual(Avx2.And(a, s_and2Const), Vector256<ulong>.Zero).AsDouble());
			}

			while (bMask == 0)
			{
				b = Generate(b, s_bMul);
				bMask = Avx.MoveMask(Avx2.CompareEqual(Avx2.And(b, s_and3Const), Vector256<ulong>.Zero).AsDouble());
			}

			var aIndex = (int)Bmi1.TrailingZeroCount((uint)aMask);
			var bIndex = (int)Bmi1.TrailingZeroCount((uint)bMask);

			aMask ^= 1 << aIndex;
			bMask ^= 1 << bIndex;

			if ((Vector256.GetElement(a, aIndex) & 0xffff) == (Vector256.GetElement(b, bIndex) & 0xffff))
				matches++;
		}

		var partB = matches.ToString();

		return (partA.ToString(), partB.ToString());
	}

	private static Vector256<ulong> GetInitialVectors(ulong key, ulong factor)
	{
		var v0 = key;
		var v1 = key * factor % 0x7fff_ffff;
		var v2 = v1 * factor % 0x7fff_ffff;
		var v3 = v2 * factor % 0x7fff_ffff;
		return Vector256.Create(v0, v1, v2, v3);
	}

	private static Vector256<ulong> Generate(Vector256<ulong> v, Vector256<ulong> mul)
	{
		var x = Avx2.Multiply(v.AsUInt32(), mul.AsUInt32());
		x = Avx2.Add(Avx2.And(x, s_and31Const), Avx2.ShiftRightLogical(x, 31));
		x = Avx2.Add(Avx2.And(x, s_and31Const), Avx2.ShiftRightLogical(x, 31));
		return x;
	}
}
