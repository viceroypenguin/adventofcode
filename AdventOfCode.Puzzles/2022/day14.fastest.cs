namespace AdventOfCode.Puzzles._2022;

#pragma warning disable CS1717 // Assignment made to same variable

[Puzzle(2022, 14, CodeType.Fastest)]
public partial class Day_14_Fastest : IPuzzle
{
	private record struct Line(int X1, int Y1, int X2, int Y2);

	public (string part1, string part2) Solve(PuzzleInput input)
	{
		Span<Line> lines = stackalloc Line[input.Bytes.Length / 8];
		var maxY = ParseLines(input.Bytes, ref lines);

		return (
			RunSand(lines, maxY, false).ToString(),
			RunSand(lines, maxY, true).ToString());
	}

	private static int ParseLines(ReadOnlySpan<byte> input, ref Span<Line> lines)
	{
		var maxY = 0;

		var lineCount = 0;
		while (input.Length > 0)
		{
			var (x, i) = input.AtoI();
			input = input[(i + 1)..];
			(var y, i) = input.AtoI();
			input = input[i..];
			if (y > maxY) maxY = y;

			while (true)
			{
				if (input[0] == '\n')
				{
					input = input[1..];
					break;
				}

				input = input[4..];
				(var x2, i) = input.AtoI();
				input = input[(i + 1)..];
				(var y2, i) = input.AtoI();
				input = input[i..];
				if (y2 > maxY) maxY = y2;

				lines[lineCount++] = new(x, y, x2, y2);
				(x, y) = (x2, y2);
			}
		}

		lines = lines[..lineCount];
		return maxY;
	}

	private static (int x, int y) ParseCoordinates(string p)
	{
		var span = p.AsSpan();
		var (x, n) = span.AtoI();
		var (y, _) = span[(n + 1)..].AtoI();
		return (x, y);
	}

	private static int RunSand(Span<Line> lines, int maxY, bool p2)
	{
		maxY += 2;
		var maxX = 510 + maxY;
		var map = new byte[maxX, maxY + 1];

		foreach (var (x1, y1, x2, y2) in lines)
		{
			if (x1 == x2)
			{
				var (min, max) = y1 < y2 ? (y1, y2) : (y2, y1);

				for (var j = min; j <= max; j++)
					map[x1, j] = (byte)'#';
			}
			else
			{
				var (min, max) = x1 < x2 ? (x1, x2) : (x2, x1);

				for (var j = min; j <= max; j++)
					map[j, y1] = (byte)'#';
			}
		}

		if (p2)
		{
			for (var x = 490 - maxY; x < 510 + maxY; x++)
				map[x, maxY] = (byte)'#';
			maxY++;
		}

		var path = new Stack<(int x, int y)>(maxY);
		path.Push((500, 0));

		var n = 0;
		while (true)
		{
			if (map[500, 0] != 0)
				return n;

			var (x, y) = path.Peek();

			while (true)
			{
				if (y + 1 >= maxY)
					return n;

				if (map[x, y + 1] == 0)
				{
					path.Push((x, y) = (x, y + 1));
					continue;
				}

				if (map[x - 1, y + 1] == 0)
				{
					path.Push((x, y) = (x - 1, y + 1));
					continue;
				}

				if (map[x + 1, y + 1] == 0)
				{
					path.Push((x, y) = (x + 1, y + 1));
					continue;
				}

				break;
			}

			_ = path.Pop();
			map[x, y] = (byte)'O';
			n++;
		}
	}
}

