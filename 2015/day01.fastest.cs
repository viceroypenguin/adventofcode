using System;
using System.Linq;
using System.Runtime.Intrinsics;

namespace AdventOfCode
{
	public class Day_2015_01_Fastest : Day
	{
		public override int Year => 2015;
		public override int DayNumber => 1;
		public override CodeType CodeType => CodeType.Fastest;

		protected override void ExecuteDay(byte[] input)
		{
			var level = 0;
			foreach (var c in input)
				level += ((byte)'(' - c) * 2 + 1;

			Dump('A', level);

			level = 0;
			int cnt = 0;
			foreach (var c in input)
			{
				cnt++;
				level += ((byte)'(' - c) * 2 + 1;
				if (level < 0)
					break;
			}

			Dump('B', cnt);
		}
	}
}
