using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2015_03_Original : Day
	{
		public override int Year => 2015;
		public override int DayNumber => 3;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var current = (x: 0, y: 0);
			var santaHouses = input
				.Select(c =>
				{
					if (c == '>') current = (current.x + 1, current.y);
					if (c == '<') current = (current.x - 1, current.y);
					if (c == '^') current = (current.x, current.y + 1);
					if (c == 'v') current = (current.x, current.y - 1);
					return current;
				})
				.Concat(new[] { (0, 0) })
				.Distinct()
				.Count();
			Dump('A', santaHouses);

			current = (x: 0, y: 0);
			var other = current;
			var bothHouses = input
				.Select(c =>
				{
					var t = other;
					if (c == '>') other = (current.x + 1, current.y);
					if (c == '<') other = (current.x - 1, current.y);
					if (c == '^') other = (current.x, current.y + 1);
					if (c == 'v') other = (current.x, current.y - 1);
					current = t;
					return other;
				})
				.Concat(new[] { (0, 0) })
				.Distinct()
				.Count();

			Dump('B', bothHouses);
		}
	}
}
