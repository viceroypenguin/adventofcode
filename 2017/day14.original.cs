using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_14_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 14;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			byte[] KnotHash(string str)
			{
				const int ArrayLength = 256;
				var list = Enumerable.Range(0, ArrayLength)
					.Select(i => (byte)i)
					.ToArray();

				var iv = str
					.Trim()
					.ToCharArray()
					.Select(c => (int)c)
					.Concat(new[] { 17, 31, 73, 47, 23, })
					.ToList()
					.AsEnumerable();

				iv = Enumerable.Repeat(iv, 64).SelectMany(i => i);

				var position = 0;
				foreach (var i in iv.Select((len, skip) => (len, skip)))
				{
					var indexes = Enumerable.Range(position, i.len)
						.Select(idx => idx % ArrayLength);

					var rev = indexes.Select(idx => list[idx]).Reverse().ToList();
					foreach (var x in indexes.Zip(rev, (idx, val) => (idx, val)))
					{
						list[x.idx] = x.val;
					}

					position = (position + i.len + i.skip) % ArrayLength;
				}

				var final = list.Select((val, idx) => new { val, g = idx / 16, })
					.GroupBy(x => x.g)
					.Select(x => x.Aggregate((byte)0, (a, v) => (byte)(a ^ v.val)))
					.ToArray();

				return final;
			}

			var key = input.GetString();

			var map =
				Enumerable.Range(0, 128)
					.Select(i =>
					{
						var hashInput = $"{input}-{i}";
						var hash = KnotHash(hashInput);
						var bits = new BitArray(hash.Reverse().ToArray());
						return bits.OfType<bool>().Reverse().ToArray();
					})
					.ToArray();

			Dump('A', map
				.SelectMany(i => i)
				.Where(b => b)
				.Count());

			var groupCount = 0;
			var groupMap = new int?[128, 128];
			void ExploreGroup(int x, int y)
			{
				var groupNumber = ++groupCount;
				var alreadySeen = new HashSet<(int x, int y)>();
				var queue = new Queue<(int x, int y)>();
				queue.Enqueue((x, y));

				while (queue.Count != 0)
				{
					var pos = queue.Dequeue();
					if (alreadySeen.Contains(pos)) continue;

					alreadySeen.Add(pos);
					groupMap[pos.x, pos.y] = groupNumber;

					if (pos.x > 0 && map[pos.x - 1][pos.y])
						queue.Enqueue((pos.x - 1, pos.y));
					if (pos.x < 127 && map[pos.x + 1][pos.y])
						queue.Enqueue((pos.x + 1, pos.y));
					if (pos.y > 0 && map[pos.x][pos.y - 1])
						queue.Enqueue((pos.x, pos.y - 1));
					if (pos.y < 127 && map[pos.x][pos.y + 1])
						queue.Enqueue((pos.x, pos.y + 1));
				}
			}


			for (int x = 0; x < 128; x++)
				for (int y = 0; y < 128; y++)
				{
					if (groupMap[x, y] != null) continue;
					if (!map[x][y]) continue;

					ExploreGroup(x, y);
				}

			Dump('B', groupCount);
		}
	}
}
