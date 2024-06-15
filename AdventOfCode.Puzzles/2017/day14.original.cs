using System.Collections;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 14, CodeType.Original)]
public class Day_14_Original : IPuzzle
{
	private static readonly int[] s_keys = [17, 31, 73, 47, 23,];

	public (string, string) Solve(PuzzleInput input)
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
				.Concat(s_keys)
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

		var key = input.Text;

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

		var partA = map
			.SelectMany(SuperEnumerable.Identity)
			.Count(b => b);

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

				_ = alreadySeen.Add(pos);
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

		for (var x = 0; x < 128; x++)
		{
			for (var y = 0; y < 128; y++)
			{
				if (groupMap[x, y] != null) continue;
				if (!map[x][y]) continue;

				ExploreGroup(x, y);
			}
		}

		var partB = groupCount;

		return (partA.ToString(), partB.ToString());
	}
}
