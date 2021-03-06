﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_10_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 10;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var nums = input.GetString()
				.Split(',')
				.Select(i => Convert.ToInt32(i))
				.ToList();

			var listCount = 256;
			var list = Enumerable.Range(0, listCount).ToArray();

			void KnotHashRound()
			{
				var position = 0;
				foreach (var i in nums.Select((len, skip) => (len, skip)))
				{
					var indexes = Enumerable.Range(position, i.len)
						.Select(idx => idx % listCount);

					var rev = indexes.Select(idx => list[idx]).Reverse().ToList();
					foreach (var x in indexes.Zip(rev, (idx, val) => (idx, val)))
					{
						list[x.idx] = x.val;
					}

					position = (position + i.len + i.skip) % listCount;
				}
			}

			KnotHashRound();
			Dump('A', list[0] * list[1]);

			list = Enumerable.Range(0, listCount).ToArray();
			nums = input.GetString()
				.Trim()
				.ToCharArray()
				.Select(c => (int)c)
				.Concat(new[] { 17, 31, 73, 47, 23, })
				.ToList();

			nums = Enumerable.Repeat(nums, 64)
				.SelectMany(i => i)
				.ToList();

			KnotHashRound();

			Dump('B',
				string.Join(
					"",
					list.Select((val, idx) => new { val, g = idx / 16, })
						.GroupBy(x => x.g)
						.Select(x => x.Aggregate(0, (a, v) => a ^ v.val))
						.Select(x => x.ToString("X2").ToLower())));
		}
	}
}
