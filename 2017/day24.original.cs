using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_24_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 24;
		public override CodeType CodeType => CodeType.Original;

		class Component
		{
			public int PortA { get; set; }
			public int PortB { get; set; }
		}

		protected override void ExecuteDay(byte[] input)
		{
			var ports =input.GetLines()
				.Select(x => x.Split('/'))
				.Select(x => new Component { PortA = Convert.ToInt32(x[0]), PortB = Convert.ToInt32(x[1]), })
				.ToList();

			var map =
				ports.Select(x => new { i = x.PortA, x, })
					.Concat(ports
						.Where(x => x.PortA != x.PortB)
						.Select(x => new { i = x.PortB, x, }))
					.ToLookup(
						x => x.i,
						x => x.x);

			var strength = CalculateStrength(map, ImmutableList<Component>.Empty, 0, (0, 0, 0));

			Dump('A', strength.maxStrength);
			Dump('B', strength.maxLongestPath);
		}

		(int maxStrength, int longestPath, int maxLongestPath)
			CalculateStrength(ILookup<int, Component> map, ImmutableList<Component> path, int openConnection, (int maxStrength, int longestPath, int maxLongestPath) x)
		{
			var list = map[openConnection]
				.Where(c => !path.Contains(c))
				.Select(c => CalculateStrength(
					map,
					path.Add(c),
					openConnection == c.PortA ? c.PortB : c.PortA,
					(maxStrength: x.maxStrength + c.PortA + c.PortB, x.longestPath, x.maxLongestPath)))
				.ToList();

			if (list.Any())
				return (
					list.Max(y => y.maxStrength),
					list.Max(y => y.longestPath),
					list.OrderByDescending(y => y.longestPath).ThenByDescending(y => y.maxLongestPath).First().maxLongestPath);

			return (x.maxStrength, path.Count, x.maxStrength);
		}
	}
}
