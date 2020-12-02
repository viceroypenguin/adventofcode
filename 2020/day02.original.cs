using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_02_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 2;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var regex = new Regex(@"(?<min>\d+)-(?<max>\d+) (?<char>\w): (?<pass>\w+)");
			var matches = input.GetLines()
				.Select(l => regex.Match(l))
				.Select(m => new
				{
					min = int.Parse(m.Groups["min"].Value),
					max = int.Parse(m.Groups["max"].Value),
					chr = m.Groups["char"].Value[0],
					pass = m.Groups["pass"].Value,
				})
				.ToArray();

			PartA = matches
				.Where(x => x.pass.Where(c => c == x.chr).CountBetween(x.min, x.max))
				.Count()
				.ToString();

			PartB = matches
				.Where(x => x.pass[x.min - 1] == x.chr ^ x.pass[x.max - 1] == x.chr)
				.Count()
				.ToString();
		}
	}
}
