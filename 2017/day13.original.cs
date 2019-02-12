using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2017_13_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 13;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var regex = new Regex(@"^(?<depth>\d+): (?<range>\d+)$", RegexOptions.Compiled);
			var depths = input.GetLines()
				.Select(l => regex.Match(l))
				.Select(m => new
				{
					depth = Convert.ToInt32(m.Groups["depth"].Value),
					range = Convert.ToInt32(m.Groups["range"].Value),
				})
				.ToList();

			Dump('A',
				depths
					.Where(f => (f.depth % ((f.range - 1) * 2)) == 0)
					.Select(f => f.depth * f.range)
					.Sum());

			Dump('B',
				Enumerable.Range(0, int.MaxValue)
					.Select(i =>
					{
						var any = depths
							.Where(f => ((f.depth + i) % ((f.range - 1) * 2)) == 0)
							.Any();
						return new { i, any, };

					})
					.Where(x => !x.any)
					.First()
					.i);
		}
	}
}
