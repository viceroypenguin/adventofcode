using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_10_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 10;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var numbers = input.GetLines()
				.Select(int.Parse)
				.OrderBy(x => x)
				.ToArray();

			var differences = numbers
				.Prepend(0)
				.Append(numbers[^1] + 3)
				.Window(2)
				.Select(x => x[1] - x[0])
				.ToArray();

			var counts = differences
				.GroupBy(x => x, (d, _) => (diff: d, count: _.Count()))
				.ToArray();

			var num1 = counts.Single(x => x.diff == 1).count;
			var num3 = counts.Single(x => x.diff == 3).count;

			PartA = (num1 * num3).ToString();

			var sequences = differences
				.Segment((cur, prev, _) => cur != prev)
				.Where(x => x.First() == 1)
				.Select(x => x.Count() switch
				{
					1 => 1,
					2 => 2,
					3 => 4,
					4 => 7,
					5 => 15,
					_ => throw new NotImplementedException("??"),
				})
				.Aggregate(1L, (agg, x) => agg * x);

			PartB = sequences.ToString();
		}
	}
}
