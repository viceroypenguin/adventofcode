using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_02_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 2;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var lines = input.GetLines()
				.Select(x => x.Split().Select(s => Convert.ToInt32(s)).ToList())
				.ToList();

			Dump('A',
				lines
					.Select(x => x.Max() - x.Min())
					.Sum());

			Dump('B',
				lines
					.Select(arr =>
					{
						return (
							from num in arr
							from div in arr
							where num != div
							where num % div == 0
							select num / div).Single();
					})
					.Sum());
		}
	}
}
