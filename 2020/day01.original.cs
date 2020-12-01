using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_01_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 1;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var numbers = input.GetLines()
				.Select(x => int.Parse(x))
				.ToArray();

			var max = 2020 - numbers.Min();

			var (x, y, z) = (
				from _x in numbers
				where _x <= max
				from _y in numbers
				where _x + _y == 2020
				select (_x, _y, 0)).First();

			PartA = (x * y).ToString();

			(x, y, z) = (
				from _x in numbers
				where _x <= max
				from _y in numbers
				where _x + _y <= max
				from _z in numbers
				where _x + _y + _z == 2020
				select (_x, _y, _z)).First();

			PartB = (x * y * z).ToString();
		}
	}
}
