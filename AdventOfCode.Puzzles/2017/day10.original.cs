namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 10, CodeType.Original)]
public class Day_10_Original : IPuzzle
{
	private static readonly int[] keys = [17, 31, 73, 47, 23,];

	public (string, string) Solve(PuzzleInput input)
	{
		var nums = input.Text
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
		var partA = list[0] * list[1];

		list = Enumerable.Range(0, listCount).ToArray();
		nums = input.Text
			.Trim()
			.ToCharArray()
			.Select(c => (int)c)
			.Concat(keys)
			.ToList();

		nums = Enumerable.Repeat(nums, 64)
			.SelectMany(i => i)
			.ToList();

		KnotHashRound();

		var partB =
			string.Join(
				"",
				list.Select((val, idx) => new { val, g = idx / 16, })
					.GroupBy(x => x.g)
					.Select(x => x.Aggregate(0, (a, v) => a ^ v.val))
					.Select(x => x.ToString("X2").ToLower()));

		return (partA.ToString(), partB);
	}
}
