namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 14, CodeType.Original)]
public partial class Day_14_Original : IPuzzle
{
	private record struct Line(int x1, int y1, int x2, int y2);

	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var (lines, maxX, maxY) = ParseLines(input.Lines);

		return (
			RunSand(lines, maxX, maxY, false).ToString(),
			RunSand(lines, maxX, maxY, true).ToString());
	}

	private static (List<Line> lines, int maxX, int maxY) ParseLines(string[] lines)
	{
		var maxX = 0;
		var maxY = 0;

		return (
			lines
				.SelectMany(l => l.Split(" -> ")
					.Select(ParseCoordinates)
					.Do(x =>
					{
						if (x.x > maxX) maxX = x.x;
						if (x.y > maxY) maxY = x.y;
					})
					.Window(2)
					.Select(w => new Line(w[0].x, w[0].y, w[1].x, w[1].y))
					.Distinct())
				.ToList(),
			maxX,
			maxY);
	}

	private static (int x, int y) ParseCoordinates(string p)
	{
		var span = p.AsSpan();
		var (x, n) = span.AtoI();
		var (y, _) = span[(n + 1)..].AtoI();
		return (x, y);
	}

	private static int RunSand(List<Line> lines, int maxX, int maxY, bool p2)
	{
		maxY += 2;
		maxX = 510 + maxY;
		var map = new byte[maxX, maxY + 1];

		foreach (var (x1, y1, x2, y2) in lines)
		{
			if (x1 == x2)
			{
				var min = int.Min(y1, y2);
				var max = int.Max(y1, y2);

				for (int j = min; j <= max; j++)
					map[x1, j] = (byte)'#';
			}
			else
			{
				var min = int.Min(x1, x2);
				var max = int.Max(x1, x2);

				for (int j = min; j <= max; j++)
					map[j, y1] = (byte)'#';
			}
		}

		if (p2)
		{
			for (int x = 490 - maxY; x < 510 + maxY; x++)
				map[x, maxY] = (byte)'#';
			maxY++;
		}

		var n = 0;
		while (true)
		{
			var (x, y) = (500, 0);

			if (map[x, y] != 0)
				return n;

			while (true)
			{
				if (y + 1 >= maxY)
					return n;

				if (map[x, y + 1] == 0)
				{
					(x, y) = (x, y + 1);
					continue;
				}

				if (map[x - 1, y + 1] == 0)
				{
					(x, y) = (x - 1, y + 1);
					continue;
				}

				if (map[x + 1, y + 1] == 0)
				{
					(x, y) = (x + 1, y + 1);
					continue;
				}

				break;
			}

			map[x, y] = (byte)'O';
			n++;
		}
	}
}

