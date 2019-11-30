using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode
{
	public class Day_2017_06_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 6;
		public override CodeType CodeType => CodeType.Fastest;

		private const int PERFORMANCE_NOTE = 16384;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override unsafe void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day06.c
			var bytes = stackalloc byte[Vector128<byte>.Count];
			var ulongs = (ulong*)bytes;

			var x = Vector128<byte>.Zero;
			int n = 0;
			var ctr = 0;
			for (int i = 0; i < input.Length && ctr < 16; i++)
			{
				if (input[i] < '0')
				{
					x = x.WithElement(ctr++, (byte)n);
					n = 0;
				}
				else
					n = n * 10 + (input[i] - '0');
			}

			var map = new Dictionary<Vector128<byte>, int>(capacity: PERFORMANCE_NOTE)
			{
				[x] = 0,
			};
			ctr = 0;

			var mask1 = Vector128.Create(0x0607040502030001ul, 0x0e0f0c0d0a0b0809ul).AsByte();
			var mask2 = Vector128.Create(0x0405060700010203ul, 0x0c0d0e0f08090a0bul).AsByte();
			var mask3 = Vector128.Create(0x0001020304050607ul, 0x08090a0b0c0d0e0ful).AsByte();
			var mask4 = Vector128.Create(0x08090a0b0c0d0e0ful, 0x0001020304050607ul).AsByte();
			while (true)
			{
				// get max byte
				var tmp = Avx2.Max(x, Avx2.Shuffle(x, mask1));
				tmp = Avx2.Max(tmp, Avx2.Shuffle(tmp, mask2));
				tmp = Avx2.Max(tmp, Avx2.Shuffle(tmp, mask3));
				tmp = Avx2.Max(tmp, Avx2.Shuffle(tmp, mask4));

				// every byte in tmp should be max value
				var max = Avx2.Extract(tmp, 0);

				// where is it in the original?
				var idx = (int)Bmi1.TrailingZeroCount((uint)
					Avx2.MoveMask(Avx2.CompareEqual(x, tmp)));

				// subtract it from it's original place
				var high = (ulong)(long)-((idx & 0x08) >> 3);
				var shift = idx << 3;
				ulongs[0] = ((ulong)max << shift) & ~high;
				ulongs[1] = ((ulong)max << shift) & high;
				tmp = Avx2.Subtract(x, Avx2.LoadVector128(bytes));

				// over 16? add 1 to all
				high = (ulong)(long)-((max & 0x10) >> 4);
				ulongs[0] = high & 0x0101010101010101ul;
				ulongs[1] = high & 0x0101010101010101ul;
				tmp = Avx2.Add(tmp, Avx2.LoadVector128(bytes));

				// spread remainder to all
				// bitmask however many we're adding
				max &= 0x0f;
				shift = max << 3;
				var isLong = (ulong)(long)-((max & 0x08) >> 3);
				var mask = (0x1ul << shift) - 1;
				var lowMask = isLong | mask;
				var highMask = isLong & mask;

				// rotate our start point
				var start = (idx + 1) & 0x0f;
				isLong = (ulong)(long)-((start & 0x08) >> 3);
				var tmpLow = (~isLong & lowMask) | (isLong & highMask);
				var tmpHigh = (isLong & lowMask) | (~isLong & highMask);

				var doShift = (ulong)((-(start & 0x07)) >> 4);
				shift = start << 3;
				lowMask = 
					((tmpLow << shift | tmpHigh >> (128 - shift)) & doShift) | 
					(~doShift & tmpLow);
				highMask = 
					((tmpHigh << shift | tmpLow >> (128 - shift)) & doShift) |
					(~doShift & tmpHigh);

				// build our adders and add values
				ulongs[0] = 0x0101010101010101ul & lowMask;
				ulongs[1] = 0x0101010101010101ul & highMask;
				tmp = Avx2.Add(tmp, Avx2.LoadVector128(bytes));

				x = tmp;

				ctr++;
				if (map.ContainsKey(x))
				{
					PartA = ctr.ToString();
					PartB = (ctr - map[x]).ToString();
					return;
				}

				map[x] = ctr;
			}
		}
	}
}
