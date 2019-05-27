using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
	public class Day_2017_01_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 1;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var sum = 0;
			var last = input[0] -= (byte)'0';
			for (int i = 1; i < input.Length; i++)
			{
				if ((input[i] -= (byte)'0') == last)
					sum += last;
				last = input[i];
			}
			sum += input[^1];
			PartA = sum.ToString();

			sum = 0;
			for (int i = 0, j = input.Length / 2; j < input.Length; i++, j++)
				if (input[i] == input[j])
					sum += input[i];
			PartB = (sum << 1).ToString();
		}
	}
}
