using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_05_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 5;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var nums = input.GetLines()
				.Select(x => Convert.ToInt32(x))
				.ToList();

			var instructions = nums.ToList();
			var ptr = 0;
			var count = 0;

			while (ptr >= 0 && ptr < instructions.Count)
			{
				var adjust = instructions[ptr];
				instructions[ptr] = adjust + 1;
				ptr += adjust;
				count++;
			}

			Dump('A', count);

			instructions = nums.ToList();
			ptr = 0;
			count = 0;

			while (ptr >= 0 && ptr < instructions.Count)
			{
				var adjust = instructions[ptr];
				instructions[ptr] =
					adjust >= 3
						? adjust - 1
						: adjust + 1;
				ptr += adjust;
				count++;
			}

			Dump('B', count);
		}
	}
}
