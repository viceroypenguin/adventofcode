namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 22, CodeType.Original)]
public partial class Day_22_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var xRanges = input.Lines.Take(..^2)
			.Select(l => (
				from: l.IndexOfAny(new[] { '.', '#', }),
				to: l.LastIndexOfAny(new[] { '.', '#', })))
			.ToList();
		var yRanges = Enumerable.Range(0, input.Lines[0].Length)
			.Select(x => (
				from: Enumerable.Range(0, input.Lines.Length - 3)
					.Select((y, i) => (c: input.Lines[y][x], i))
					.First(z => z.c is '.' or '#')
					.i,
				to: -SuperEnumerable.Sequence(input.Lines.Length - 3, 0, -1)
					.Select((y, i) => (
						c: input.Lines[y].Length > x ? input.Lines[y][x] : 0,
						i))
					.First(z => z.c is '.' or '#')
					.i + input.Lines.Length - 3))
			.ToList();

		var part1 = DoPart1(input.Lines, xRanges, yRanges).ToString();
		var part2 = DoPart2(input.Lines, xRanges, yRanges).ToString();
		return (part1, part2);
	}

	private static int DoPart1(string[] grid, List<(int from, int to)> xRanges, List<(int from, int to)> yRanges)
	{
		var pos = (x: xRanges[0].from, y: 0, face: 0);
		var span = grid[^1].AsSpan();
		while (span.Length > 0)
		{
			var (n, i) = span.AtoI();

			switch (pos.face)
			{
				case 0:
				{
					for (int j = 0; j < n; j++)
					{
						var newX = pos.x + 1;
						if (newX > xRanges[pos.y].to)
							newX = xRanges[pos.y].from;
						if (grid[pos.y][newX] == '#')
							break;
						pos = (newX, pos.y, pos.face);
					}
					break;
				}

				case 1:
				{
					for (int j = 0; j < n; j++)
					{
						var newY = pos.y + 1;
						if (newY > yRanges[pos.x].to)
							newY = yRanges[pos.x].from;
						if (grid[newY][pos.x] == '#')
							break;
						pos = (pos.x, newY, pos.face);
					}
					break;
				}

				case 2:
				{
					for (int j = 0; j < n; j++)
					{
						var newX = pos.x - 1;
						if (newX < xRanges[pos.y].from)
							newX = xRanges[pos.y].to;
						if (grid[pos.y][newX] == '#')
							break;
						pos = (newX, pos.y, pos.face);
					}
					break;
				}

				case 3:
				{
					for (int j = 0; j < n; j++)
					{
						var newY = pos.y - 1;
						if (newY < yRanges[pos.x].from)
							newY = yRanges[pos.x].to;
						if (grid[newY][pos.x] == '#')
							break;
						pos = (pos.x, newY, pos.face);
					}
					break;
				}
			}

			if (span.Length == i)
				break;

			var t = span[i];
			var f = pos.face + (t == 'R' ? 1 : -1);
			if (f >= 4) f = 0;
			if (f < 0) f = 3;
			pos = (pos.x, pos.y, f);

			span = span[(i + 1)..];
		}

		return (pos.y + 1) * 1000 + (pos.x + 1) * 4 + pos.face;
	}

	private static int DoPart2(string[] grid, List<(int from, int to)> xRanges, List<(int from, int to)> yRanges)
	{
		var pos = (x: xRanges[0].from, y: 0, face: 0);
		var span = grid[^1].AsSpan();
		while (span.Length > 0)
		{
			var (n, i) = span.AtoI();

			for (int j = 0; j < n; j++)
			{
				var next = MoveNextPart2(grid, xRanges, yRanges, pos);
				if (next == pos)
					break;
				pos = next;
			}

			if (span.Length == i)
				break;

			{
				var t = span[i];
				var f = pos.face + (t == 'R' ? 1 : -1);
				if (f >= 4) f = 0;
				if (f < 0) f = 3;
				pos = (pos.x, pos.y, f);
			}

			span = span[(i + 1)..];
		}

		return (pos.y + 1) * 1000 + (pos.x + 1) * 4 + pos.face;
	}

	private static (int x, int y, int face) MoveNextPart2(string[] grid, List<(int from, int to)> xRanges, List<(int from, int to)> yRanges, (int x, int y, int face) pos)
	{
		var next = pos;
		switch (pos.face)
		{
			case 0:
			{
				next.x += 1;
				if (next.x > xRanges[pos.y].to)
				{
					if (next.y < 50)
					{
						var y = 149 - next.y;
						var x = 99;
						var f = 2;
						next = (x, y, f);
					}
					else if (next.y < 100)
					{
						var y = 49;
						var x = next.y + 50;
						var f = 3;
						next = (x, y, f);
					}
					else if (next.y < 150)
					{
						var y = 149 - next.y;
						var x = 149;
						var f = 2;
						next = (x, y, f);
					}
					else
					{
						var y = 149;
						var x = next.y - 100;
						var f = 3;
						next = (x, y, f);
					}
				}
				break;
			}

			case 1:
			{
				next.y += 1;
				if (next.y > yRanges[pos.x].to)
				{
					if (next.x < 50)
					{
						var y = 0;
						var x = next.x + 100;
						var f = 1;
						next = (x, y, f);
					}
					else if (next.x < 100)
					{
						var y = next.x + 100;
						var x = 49;
						var f = 2;
						next = (x, y, f);
					}
					else
					{
						var y = next.x - 50;
						var x = 99;
						var f = 2;
						next = (x, y, f);
					}
				}
				break;
			}

			case 2:
			{
				next.x -= 1;
				if (next.x < xRanges[pos.y].from)
				{
					if (next.y < 50)
					{
						var y = 149 - next.y;
						var x = 0;
						var f = 0;
						next = (x, y, f);
					}
					else if (next.y < 100)
					{
						var y = 100;
						var x = next.y - 50;
						var f = 1;
						next = (x, y, f);
					}
					else if (next.y < 150)
					{
						var y = 149 - next.y;
						var x = 50;
						var f = 0;
						next = (x, y, f);
					}
					else
					{
						var y = 0;
						var x = next.y - 100;
						var f = 1;
						next = (x, y, f);
					}
				}
				break;
			}

			case 3:
			{
				next.y -= 1;
				if (next.y < yRanges[pos.x].from)
				{
					if (next.x < 50)
					{
						var y = next.x + 50;
						var x = 50;
						var f = 0;
						next = (x, y, f);
					}
					else if (next.x < 100)
					{
						var y = next.x + 100;
						var x = 0;
						var f = 0;
						next = (x, y, f);
					}
					else
					{
						var y = 199;
						var x = next.x - 100;
						var f = 3;
						next = (x, y, f);
					}
				}
				break;
			}
		}

		return grid[next.y][next.x] == '#'
			? pos : next;
	}
}
