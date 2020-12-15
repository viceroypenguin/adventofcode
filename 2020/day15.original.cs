using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using MoreLinq;
using static AdventOfCode.Helpers;

namespace AdventOfCode
{
	public class Day_2020_15_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 15;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var numbers = input.GetString()
				.Split(',')
				.Select(int.Parse)
				.ToArray();

			var spokenTimes = numbers
				.Select((n, i) => (n, i))
				.ToDictionary(
					x => x.n,
					x => x.i + 1);

			var curNumber = 0;
			var i = numbers.Length + 1;
			for (; i < 2020; i++)
			{
				var hadValue = spokenTimes.TryGetValue(curNumber, out var prevTime);
				spokenTimes[curNumber] = i;
				curNumber = hadValue ? i - prevTime : 0;
			}

			PartA = curNumber.ToString();

			for (; i < 30_000_000; i++)
			{
				var hadValue = spokenTimes.TryGetValue(curNumber, out var prevTime);
				spokenTimes[curNumber] = i;
				curNumber = hadValue ? i - prevTime : 0;
			}

			PartB = curNumber.ToString();
		}
	}
}
