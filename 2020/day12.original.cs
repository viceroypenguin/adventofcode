using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_12_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 12;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var lines = input.GetLines();

			DoPartA(lines);
			DoPartB(lines);
		}

		private void DoPartA(string[] lines)
		{
			(int x, int y, int d) Move(int x, int y, int d, int dir, int amount) =>
				(x, y, d) = dir switch
				{
					0 => (x + amount, y, d),
					1 => (x, y - amount, d),
					2 => (x - amount, y, d),
					3 => (x, y + amount, d),
				};

			(int x, int y, int d) RotateDir(int x, int y, int d, int a) =>
				(x, y, (d + a) % 4);

			int x = 0, y = 0, d = 0;
			foreach (var (_d, n) in lines.Select(s => (s[0], Convert.ToInt32(s[1..]))))
			{
				(x, y, d) = _d switch
				{
					'F' => Move(x, y, d, d, n),

					'E' => Move(x, y, d, 0, n),
					'S' => Move(x, y, d, 1, n),
					'W' => Move(x, y, d, 2, n),
					'N' => Move(x, y, d, 3, n),

					'R' => RotateDir(x, y, d, n / 90),
					'L' => RotateDir(x, y, d, 4 - (n / 90)),
				};
			}

			PartA = (Math.Abs(x) + Math.Abs(y)).ToString();
		}

		private void DoPartB(string[] lines)
		{
			(int x, int y, int wayx, int wayy) RotateWaypoint(int x, int y, int wayx, int wayy, int a) =>
				a switch
				{
					0 => (x, y, wayx, wayy),
					1 => (x, y, wayy, -wayx),
					2 => (x, y, -wayx, -wayy),
					3 => (x, y, -wayy, wayx),
				};

			int wayx = 10, wayy = 1;
			int x = 0, y = 0;
			foreach (var (_d, n) in lines.Select(s => (s[0], Convert.ToInt32(s[1..]))))
			{
				(x, y, wayx, wayy) = _d switch
				{
					'F' => (x + (wayx * n), y + (wayy * n), wayx, wayy),

					'E' => (x, y, wayx + n, wayy),
					'S' => (x, y, wayx, wayy - n),
					'W' => (x, y, wayx - n, wayy),
					'N' => (x, y, wayx, wayy + n),

					'R' => RotateWaypoint(x, y, wayx, wayy, n / 90),
					'L' => RotateWaypoint(x, y, wayx, wayy, 4 - (n / 90)),
				};
			}

			PartB = (Math.Abs(x) + Math.Abs(y)).ToString();
		}
	}
}
