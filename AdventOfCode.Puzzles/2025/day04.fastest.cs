using CommunityToolkit.HighPerformance;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 04, CodeType.Fastest)]
public partial class Day_04_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var stride = input.Span.IndexOf((byte)'\n');
		var map = new Span2D<byte>(input.Bytes.ToArray(), 0, input.Bytes.Length / (stride + 1), stride, 1);

		var queue1 = new HashSet<(int x, int y)>(2048);
		var queue2 = new HashSet<(int x, int y)>(2048);

		for (var y = 0; y < map.Height; y++)
		{
			var line = map.GetRowSpan(y);

			for (var x = 0; x < line.Length; x++)
			{
				if (
					line[x] == '@'
					&& CanAccessPaper(map, x, y)
				)
				{
					queue1.Add((x, y));
				}
			}
		}

		var part1 = queue1.Count;
		var part2 = 0;

		while (queue1.Count > 0)
		{
			part2 += queue1.Count;

			(queue1, queue2) = (queue2, queue1);
			foreach (var (x, y) in queue2)
				map[y, x] = (byte)'.';

			foreach (var (x, y) in queue2)
			{
				foreach (var (ax, ay) in MapExtensions.Adjacent)
				{
					if (
						!(y + ay).Between(0, map.Height - 1)
						|| !(x + ax).Between(0, map.Width - 1)
					)
					{
						continue;
					}

					if (
						map[y + ay, x + ax] == '@'
						&& !queue1.Contains((x + ax, y + ay))
						&& CanAccessPaper(map, x + ax, y + ay)
					)
					{
						queue1.Add((x + ax, y + ay));
					}
				}
			}

			queue2.Clear();
		}

		return (part1.ToString(), part2.ToString());

		static bool CanAccessPaper(
			Span2D<byte> map,
			int x, int y
		)
		{
			var count = 0;
			foreach (var (ax, ay) in MapExtensions.Adjacent)
			{
				if (
					!(y + ay).Between(0, map.Height - 1)
					|| !(x + ax).Between(0, map.Width - 1)
				)
				{
					continue;
				}

				if (map[y + ay, x + ax] == '@')
				{
					count++;
					if (count >= 4)
						return false;
				}
			}

			return true;
		}
	}
}
