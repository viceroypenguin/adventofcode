using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_06_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 6;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var answerSets = input.GetLines(StringSplitOptions.TrimEntries)
				.Segment(l => string.IsNullOrWhiteSpace(l))
				.ToArray();

			PartA = answerSets
				.Sum(l => l.SelectMany(c => c)
					.Distinct()
					.Count())
				.ToString();

			PartB = answerSets
				.Sum(l =>
				{
					var numPeople = l.Where(s => !string.IsNullOrWhiteSpace(s)).Count();
					return l.SelectMany(c => c)
						.GroupBy(
							c => c,
							(c, _) => _.Count())
						.Count(x => x == numPeople);
				})
				.ToString();
		}
	}
}
