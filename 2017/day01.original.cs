using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_01_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 1;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var data = input.GetString()
				.Select(x => (int)x - (int)'0')
				.ToList();

			Dump('A',
				data.Zip(data.Skip(1), (a, b) => new { a, b })
					.Where(x => x.a == x.b)
					.Select(x => x.a)
					.Sum() + data.Last());

			var rotInput = data.Skip(data.Count / 2).Concat(data.Take(data.Count / 2));
			Dump('B',
				data.Zip(rotInput, (a, b) => new { a, b })
					.Where(x => x.a == x.b)
					.Select(x => x.a)
					.Sum());
		}
	}
}
