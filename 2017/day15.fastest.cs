using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode
{
	public class Day_2017_15_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 15;
		public override CodeType CodeType => CodeType.Fastest;

		private const ulong GenA = 16807;
		private const ulong GenA4 = GenA * GenA * GenA * GenA % 0x7fff_ffff;
		private const ulong GenB = 48271;
		private const ulong GenB4 = GenB * GenB * GenB * GenB % 0x7fff_ffff;

		private readonly Vector256<ulong> aMul = Vector256.Create(GenA4, GenA4, GenA4, GenA4);
		private readonly Vector256<ulong> bMul = Vector256.Create(GenB4, GenB4, GenB4, GenB4);

		private readonly Vector256<ulong> and31Const = Vector256.Create(0x7fff_fffful, 0x7fff_fffful, 0x7fff_fffful, 0x7fff_fffful);
		private readonly Vector256<ulong> and16Const = Vector256.Create(0xfffful, 0xfffful, 0xfffful, 0xfffful);
		private readonly Vector256<ulong> and3Const = Vector256.Create(0x07ul, 0x07ul, 0x07ul, 0x07ul);
		private readonly Vector256<ulong> and2Const = Vector256.Create(0x03ul, 0x03ul, 0x03ul, 0x03ul);

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day15.c
			ulong aKey = 0, bKey = 0;
			for (int i = 0; i < input.Length; i++)
			{
				var c = input[i];
				if (c == '\n' && aKey == 0)
				{
					aKey = bKey;
					bKey = 0;
				}
				else if (c >= '0' && c <= '9')
					bKey = bKey * 10 + c - '0';
			}

			var a0 = GetInitialVectors(aKey, GenA);
			var b0 = GetInitialVectors(bKey, GenB);

			var a = a0; var b = b0;
			var sums = Vector256<ulong>.Zero;
			for (int i = 0; i < 40_000_000 / 4; i++)
			{
				a = Generate(a, aMul);
				b = Generate(b, bMul);

				sums = Avx2.Subtract(sums, Avx2.CompareEqual(Avx2.And(a, and16Const), Avx2.And(b, and16Const)));
			}
			PartA = (Vector256.GetElement(sums, 0) + Vector256.GetElement(sums, 1) + Vector256.GetElement(sums, 2) + Vector256.GetElement(sums, 3)).ToString();

			a = a0; b = b0;
			var aMask = Avx2.MoveMask(Avx2.CompareEqual(Avx2.And(a, and2Const), Vector256<ulong>.Zero).AsDouble());
			var bMask = Avx2.MoveMask(Avx2.CompareEqual(Avx2.And(b, and3Const), Vector256<ulong>.Zero).AsDouble());
			var matches = 0;
			for (int i = 0; i < 5_000_000; i++)
			{
				while (aMask == 0)
				{
					a = Generate(a, aMul);
					aMask = Avx2.MoveMask(Avx2.CompareEqual(Avx2.And(a, and2Const), Vector256<ulong>.Zero).AsDouble());
				}
				while (bMask == 0)
				{
					b = Generate(b, bMul);
					bMask = Avx2.MoveMask(Avx2.CompareEqual(Avx2.And(b, and3Const), Vector256<ulong>.Zero).AsDouble());
				}

				var aIndex = (int)Bmi1.TrailingZeroCount((uint)aMask);
				var bIndex = (int)Bmi1.TrailingZeroCount((uint)bMask);

				aMask ^= 1 << aIndex;
				bMask ^= 1 << bIndex;

				if ((Vector256.GetElement(a, aIndex) & 0xffff) == (Vector256.GetElement(b, bIndex) & 0xffff))
					matches++;
			}
			PartB = matches.ToString();
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private Vector256<ulong> GetInitialVectors(ulong key, ulong factor)
		{
			var v0 = key;
			var v1 = (key * factor) % 0x7fff_ffff;
			var v2 = (v1 * factor) % 0x7fff_ffff;
			var v3 = (v2 * factor) % 0x7fff_ffff;
			return Vector256.Create(v0, v1, v2, v3);
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private Vector256<ulong> Generate(Vector256<ulong> v, Vector256<ulong> mul)
		{
			var x = Avx2.Multiply(v.AsUInt32(), mul.AsUInt32());
			x = Avx2.Add(Avx2.And(x, and31Const), Avx2.ShiftRightLogical(x, 31));
			x = Avx2.Add(Avx2.And(x, and31Const), Avx2.ShiftRightLogical(x, 31));
			return x;
		}
	}
}
