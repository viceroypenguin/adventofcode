using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_02_Fastest : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 2;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			int part1 = 0, part2 = 0;

			var span = new ReadOnlySpan<byte>(input);
			for (int i = 0; i < span.Length;)
			{
				var x = span[i..].AtoI();
				var min = x.value;
				i += x.numChars + 1;

				x = span[i..].AtoI();
				var max = x.value;
				i += x.numChars + 1;

				var chr = span[i];
				i += 2;

				part2 += ((span[i + min] == chr) ^ (span[i + max] == chr)) ? 1 : 0;
				i++;
				
				var cnt = 0;
				for (; i < span.Length && span[i] != '\n'; i++)
					cnt += span[i] == chr ? 1 : 0;
				part1 += cnt >= min && cnt <= max ? 1 : 0;

				i++;
			}

			PartA = part1.ToString();
			PartB = part2.ToString();
		}
	}
}
