using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 06, CodeType.Fastest)]
public class Day_06_Fastest : IPuzzle
{
	private const int PERFORMANCE_NOTE = 16384;

	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day06.c
		Span<Vector128<byte>> vector = stackalloc Vector128<byte>[1];
		var bytes = MemoryMarshal.Cast<Vector128<byte>, byte>(vector);
		var ulongs = MemoryMarshal.Cast<byte, ulong>(bytes);

		var span = input.GetSpan();
		var n = 0;
		var ctr = 0;
		for (var i = 0; i < span.Length && ctr < 16; i++)
		{
			if (span[i] < '0')
			{
				bytes[ctr++] = (byte)n;
				n = 0;
			}
			else
			{
				n = (n * 10) + (span[i] - '0');
			}
		}

		var x = vector[0];
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
			var tmp = Sse2.Max(x, Ssse3.Shuffle(x, mask1));
			tmp = Sse2.Max(tmp, Ssse3.Shuffle(tmp, mask2));
			tmp = Sse2.Max(tmp, Ssse3.Shuffle(tmp, mask3));
			tmp = Sse2.Max(tmp, Ssse3.Shuffle(tmp, mask4));

			// every byte in tmp should be max value
			var max = Sse41.Extract(tmp, 0);

			// where is it in the original?
			var idx = (int)Bmi1.TrailingZeroCount((uint)
				Sse2.MoveMask(Sse2.CompareEqual(x, tmp)));

			// subtract it from it's original place
			var high = (ulong)(long)-((idx & 0x08) >> 3);
			var shift = idx << 3;
			ulongs[0] = ((ulong)max << shift) & ~high;
			ulongs[1] = ((ulong)max << shift) & high;
			tmp = Sse2.Subtract(x, vector[0]);

			// over 16? add 1 to all
			high = (ulong)(long)-((max & 0x10) >> 4);
			ulongs[0] = high & 0x0101010101010101ul;
			ulongs[1] = high & 0x0101010101010101ul;
			tmp = Sse2.Add(tmp, vector[0]);

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
				(((tmpLow << shift) | (tmpHigh >> (128 - shift))) & doShift) |
				(~doShift & tmpLow);
			highMask =
				(((tmpHigh << shift) | (tmpLow >> (128 - shift))) & doShift) |
				(~doShift & tmpHigh);

			// build our adders and add values
			ulongs[0] = 0x0101010101010101ul & lowMask;
			ulongs[1] = 0x0101010101010101ul & highMask;
			tmp = Sse2.Add(tmp, vector[0]);

			x = tmp;

			ctr++;
			if (map.TryGetValue(x, out var v))
			{
				return (
					ctr.ToString(),
					(ctr - v).ToString());
			}

			map[x] = ctr;
		}
	}
}
