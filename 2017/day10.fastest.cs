using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
	public class Day_2017_10_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 10;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected unsafe override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day10.c
			var bytes = stackalloc byte[256];
			for (var i = 0; i < 256; i++)
				bytes[i] = (byte)i;

			var n = 0;
			byte position = 0, skip = 0;
			foreach (var c in input)
			{
				if (c >= '0')
					n = n * 10 + c - '0';
				else if (c == ',' || c == '\n')
				{
					(position, skip) = DoRound(bytes, (byte)n, position, skip);
					n = 0;
				}
			}
			PartA = (bytes[0] * bytes[1]).ToString();

			var newLength = input.Length - 2 + 5;
			var seq = stackalloc byte[newLength];
			for (int i = 0; i < input.Length - 2; i++)
				seq[i] = input[i];
			seq[newLength - 5] = 17;
			seq[newLength - 4] = 31;
			seq[newLength - 3] = 73;
			seq[newLength - 2] = 47;
			seq[newLength - 1] = 23;

			for (var i = 0; i < 256; i++)
				bytes[i] = (byte)i;
			position = skip = 0;
			for (var i = 0; i < 64; i++)
			{
				for (int j = 0; j < newLength; j++)
					(position, skip) = DoRound(bytes, seq[j], position, skip);
			}

			var str = new char[32];
			for (int i = 0; i < 256; i += 16)
			{
				var b = bytes[i];
				for (int j = i + 1; j < i + 16; j++)
					b ^= bytes[j];

				str[i >> 3] = ToHex(b >> 4);
				str[(i >> 3) + 1] = ToHex(b & 0xf);
			}
			PartB = new string(str);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe (byte position, byte skip) DoRound(byte* bytes, byte length, byte position, byte skip)
		{
			// start in the middle
			byte i = (byte)(position + (length >> 1)),
				// do we need to add one or not
				j = (byte)(i - (~length & 1));

			// twist the rope
			while (i != position)
			{
				i--; j++;
				var tmp = bytes[i];
				bytes[i] = bytes[j];
				bytes[j] = tmp;
			}
			skip++; position = (byte)(j + skip);
			return (position, skip);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private char ToHex(int val) =>
			val >= 10
				? (char)(val - 10 + 'a')
				: (char)(val + '0');
	}
}
