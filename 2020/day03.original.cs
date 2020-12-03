using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_03_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 3;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var map = input.GetLines()
				.Select(s => s.Select(c => c == '#').ToArray())
				.ToArray();

			long GetTreesOnSlope(int vx, int vy)
			{
				var count = 0;
				for (int x = 0, y = 0; x < map.Length; x += vx, y += vy)
					if (map[x][y % map[x].Length])
						count++;
				return count;
			}

			PartA = GetTreesOnSlope(1, 3).ToString();

			PartB = (
				GetTreesOnSlope(1, 1) *
				GetTreesOnSlope(1, 3) *
				GetTreesOnSlope(1, 5) *
				GetTreesOnSlope(1, 7) *
				GetTreesOnSlope(2, 1)).ToString();
		}
	}
}
