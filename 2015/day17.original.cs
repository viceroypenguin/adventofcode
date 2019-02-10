using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2015_17_Original : Day
	{
		public override int Year => 2015;
		public override int DayNumber => 17;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var total = 150;

			var containers = input.GetLines()
				.Select(s => Convert.ToInt32(s))
				.ToList();

			var cnt = 0;
			var max = 1 << containers.Count;
			var numCombinations = 0;

			var minPop = int.MaxValue;
			var haveMinPop = 0;
			while (cnt < max)
			{
				var sum = 0;
				var bitstream = new BitArray(new[] { cnt });
				foreach (var _ in bitstream.OfType<bool>().Select((b, i) => new { b, i }))
					if (_.b)
						sum += containers[_.i];

				if (sum == total)
				{
					numCombinations++;

					var pop = bitstream.OfType<bool>().Where(b => b).Count();
					if (pop < minPop)
					{
						minPop = pop;
						haveMinPop = 1;
					}
					else if (pop == minPop)
						haveMinPop++;
					//Convert.ToString(cnt, 2).PadLeft(containers.Count, '0').Dump();
				}

				cnt++;
			}

			Dump('A', numCombinations);
			Dump('B', haveMinPop);
		}
	}
}
