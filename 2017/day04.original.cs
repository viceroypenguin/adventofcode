using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_04_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 4;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var lines = input.GetLines()
				.Select(x => x.Split())
				.ToList();

			Dump('A',
				lines
					.Where(l => l.Distinct().Count() == l.Count())
					.Count());

			Dump('B',
				lines
					.Where(l => l.Select(s => new string(s.OrderBy(c => c).ToArray())).Distinct().Count() == l.Count())
					.Count());
		}
	}
}
