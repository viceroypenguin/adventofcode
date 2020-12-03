using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_03_Fastest : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 3;
		public override CodeType CodeType => CodeType.Fastest;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			static long GetTreesOnSlope(byte[] input, int vx, int vy)
			{
				vx *= 32;

				var count = 0;
				for (int x = 0, y = 0; x < input.Length; x += vx, y += vy)
					count += input[x + (y % 31)] == '#' ? 1 : 0;
				return count;
			}

			PartA = GetTreesOnSlope(input, 1, 3).ToString();

			PartB = (
				GetTreesOnSlope(input, 1, 1) *
				GetTreesOnSlope(input, 1, 3) *
				GetTreesOnSlope(input, 1, 5) *
				GetTreesOnSlope(input, 1, 7) *
				GetTreesOnSlope(input, 2, 1)).ToString();
		}
	}
}
