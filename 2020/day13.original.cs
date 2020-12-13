using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;
using static AdventOfCode.Helpers;

namespace AdventOfCode
{
	public class Day_2020_13_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 13;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var lines = input.GetLines();
			var times = lines[1].Split(',');

			var myEarliestTime = int.Parse(lines[0]);
			PartA = times
				.Where(s => s != "x")
				.Select(int.Parse)
				.Select(b => (bus: b, firstTimeAfter: (myEarliestTime / b + 1) * b - myEarliestTime))
				.PartialSortBy(1, x => x.firstTimeAfter)
				.Select(x => x.bus * x.firstTimeAfter)
				.First()
				.ToString();

			var earliestTime = long.Parse(times[0]);
			var increment = earliestTime;
			for (int i = 1; i < times.Length; i++)
			{
				if (times[i] == "x") continue;

				var curTime = long.Parse(times[i]);
				var modValue = curTime - (i % curTime);
				while (earliestTime % curTime != modValue)
					earliestTime += increment;
				increment = lcm(increment, curTime);
			}

			PartB = earliestTime.ToString();
		}
	}
}
