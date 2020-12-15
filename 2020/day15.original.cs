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

			var spokenTimes = new int[30_000_000];
			Array.Fill(spokenTimes, -1);

			var i = 1;
			for (; i < numbers.Length + 1; i++)
				spokenTimes[numbers[i - 1]] = i;

			var curNumber = 0;
			for (; i < 2020; i++)
			{
				var prevTime = spokenTimes[curNumber];
				spokenTimes[curNumber] = i;
				curNumber = prevTime != -1 ? i - prevTime : 0;
			}

			PartA = curNumber.ToString();

			for (; i < 30_000_000; i++)
			{
				var prevTime = spokenTimes[curNumber];
				spokenTimes[curNumber] = i;
				curNumber = prevTime != -1 ? i - prevTime : 0;
			}

			PartB = curNumber.ToString();
		}
	}
}
