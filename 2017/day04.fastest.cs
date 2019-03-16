using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode
{
	public class Day_2017_04_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 4;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var line = new ulong[32];
			var number = 0ul;
			int part1 = 0, part2 = 0;
			bool flag1 = false, flag2 = false;
			for (int i = 0, j = 0; i < input.Length; i++)
			{
				var c = input[i];
				if (c >= 'a')
					number = (number << 8) + c;
				else if (c != '\n')
				{
					var sortedNumber = SortBytes(number);
					for (int k = 0; !flag1 && k < j; k += 2)
						if (line[k] == number)
							flag1 = true;
					for (int k = 1; !flag2 && k < j; k += 2)
						if (line[k] == sortedNumber)
							flag2 = true;

					if (c == '\r')
					{
						if (!flag1) part1++;
						if (!flag2) part2++;
						flag1 = false;
						flag2 = false;
						j = 0;
						number = 0;
					}
					else
					{
						line[j++] = number;
						line[j++] = sortedNumber;
						number = 0;
					}
				}
			}

			Dump('A', part1);
			Dump('B', part2);
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private unsafe ulong SortBytes(ulong bytes)
		{
			var arr = (byte*)&bytes;
			for (int i = 0; i < 7; i++)
				for (int j = i + 1; j < 8; j++)
					if (arr[j] < arr[i])
					{
						var t = arr[j];
						arr[j] = arr[i];
						arr[i] = t;
					}
			return bytes;
		}
	}
}
