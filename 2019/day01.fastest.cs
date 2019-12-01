using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2019_01_Fastest : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 1;
		public override CodeType CodeType => CodeType.Fastest;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			int part1Sum = 0, part2Sum = 0, n = 0;
			foreach (var c in input)
			{
				if (c == '\r')
				{
					var fuel = n / 3 - 2;
					part1Sum += fuel;
					while (fuel > 0)
					{
						part2Sum += fuel;
						fuel = fuel / 3 - 2;
					}
					n = 0;
				}
				else if (c >= '0')
					n = n * 10 + c - '0';
			}

			PartA = part1Sum.ToString();
			PartB = part2Sum.ToString();
		}
	}
}
