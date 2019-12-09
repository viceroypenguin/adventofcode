using System;
using System.Collections.Generic;
using System.Linq;
using static MoreLinq.Extensions.BatchExtension;

namespace AdventOfCode
{
	public class Day_2019_08_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 8;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var layers = input
				.SkipLast(1)
				.Batch(25 * 6)
				.Select(s => s.ToList())
				.ToList();

			var zeroLayer = layers
				.OrderBy(x => x.Count(z => z == '0'))
				.First();

			Dump('A', zeroLayer.Count(z => z == '1') * zeroLayer.Count(z => z == '2'));

			DumpScreen('B', Enumerable.Range(0, 25 * 6)
				.Select(p => Enumerable.Range(0, layers.Count)
					.Select(l => layers[l][p])
					.Aggregate('2', (c, lc) => c != '2' ? c : lc == '0' ? ' ' : lc == '1' ? '█' : (char)lc))
				.Batch(25));
		}
	}
}
