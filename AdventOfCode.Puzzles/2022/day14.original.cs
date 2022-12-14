namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 14, CodeType.Original)]
public partial class Day_14_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input) =>
		(
			RunSand(input.Lines, false).ToString(),
			RunSand(input.Lines, true).ToString());

	private static int RunSand(IEnumerable<string> lines, bool p2)
	{
		var map = new HashSet<(int x, int y)>();
		foreach (var l in lines)
		{
			foreach (var (start, end) in l.Split(" -> ")
				.Select(x => x.Split(','))
				.Select(s => (x: int.Parse(s[0]), y: int.Parse(s[1])))
				.Window(2)
				.Select(w => (w[0], w[1])))
			{
				if (start.x == end.x)
				{
					var min = int.Min(start.y, end.y);
					var max = int.Max(start.y, end.y);

					for (int j = min; j <= max; j++)
						map.Add((start.x, j));
				}
				else
				{
					var min = int.Min(start.x, end.x);
					var max = int.Max(start.x, end.x);

					for (int j = min; j <= max; j++)
						map.Add((j, start.y));
				}
			}
		}

		var maxY = map.Select(k => k.y).Max();
		if (p2)
		{
			maxY += 2;
			for (int x = 490 - maxY; x < 510 + maxY; x++)
				map.Add((x, maxY));
			maxY++;
		}

		var n = 0;
		while (true)
		{
			var pos = (x: 500, y: 0);

			if (map.Contains(pos))
				return n;

			while (true)
			{
				var newPos = (pos.x, y: pos.y + 1);
				if (newPos.y > maxY)
					return n;

				if (!map.Contains(newPos))
				{
					pos = newPos;
					continue;
				}

				newPos = (pos.x - 1, pos.y + 1);
				if (!map.Contains(newPos))
				{
					pos = newPos;
					continue;
				}

				newPos = (pos.x + 1, pos.y + 1);
				if (!map.Contains(newPos))
				{
					pos = newPos;
					continue;
				}

				break;
			}

			map.Add(pos);
			n++;
		}
	}
}

