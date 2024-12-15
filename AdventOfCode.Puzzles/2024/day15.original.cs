using System.Diagnostics;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 15, CodeType.Original)]
public partial class Day_15_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var splits = input.Lines.Split(string.Empty).ToList();
		var map = splits[0].Select(s => s.ToCharArray()).ToArray();
		var data = string.Join("", splits[1]);

		var (rx, ry) = map.GetMapPoints()
			.Single(p => p.item == '@')
			.p;

		foreach (var c in data)
		{
			var (dx, dy) = GetDirection(c);

			var (nx, ny) = (rx + dx, ry + dy);
			while (map[ny][nx] == 'O')
				(nx, ny) = (nx + dx, ny + dy);

			if (map[ny][nx] == '#')
				continue;

			map[ry][rx] = '.';
			(rx, ry) = (rx + dx, ry + dy);
			map[ny][nx] = 'O';
			map[ry][rx] = '@';
		}

		var part1 = 0;
		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] == 'O')
					part1 += (y * 100) + x;
			}
		}

		map = splits[0]
			.Select(s => s
				.SelectMany(c => c switch
				{
					'#' => "##",
					'O' => "[]",
					'.' => "..",
					'@' or _ => "@.",
				})
				.ToArray()
			)
			.ToArray();

		(rx, ry) = map.GetMapPoints()
			.Single(p => p.item == '@')
			.p;

		var nextPoints = new List<(int x, int y)>();
		var scratch = new List<(int x, int y)>();
		var boxes = new List<(int x, int y)>();

		foreach (var c in data)
		{
			var (dx, dy) = GetDirection(c);

			if (dy is 0)
			{
				if (map[ry][rx + dx] is '.')
				{
					map[ry][rx] = '.';
					rx += dx;
					map[ry][rx] = '@';
					continue;
				}

				var (nx, ny) = (rx + dx, ry);
				while (map[ny][nx] is '[' or ']')
					nx += dx;

				if (map[ny][nx] == '#')
					continue;

				map[ry][rx] = '.';
				rx += dx;
				(var ch, map[ry][rx]) = (map[ry][rx], '@');

				(nx, ny) = (rx + dx, ry);
				while (map[ny][nx] is not '.')
				{
					(ch, map[ny][nx]) = (map[ny][nx], ch);
					nx += dx;
				}

				map[ny][nx] = ch;
			}
			else
			{
				if (map[ry + dy][rx] is '#')
					continue;

				if (map[ry + dy][rx] is '.')
				{
					map[ry][rx] = '.';
					ry += dy;
					map[ry][rx] = '@';
					continue;
				}

				boxes.Clear();
				nextPoints.Clear();
				nextPoints.Add((rx, ry + dy));

				var flag = true;
				while (nextPoints.Count != 0)
				{
					scratch.Clear();

					foreach (var (px, py) in nextPoints)
					{
						if (map[py][px] is '#')
						{
							flag = false;
							scratch.Clear();
							break;
						}

						if (map[py][px] is '[')
						{
							boxes.Add((px, py));
							scratch.Add((px, py + dy));
							scratch.Add((px + 1, py + dy));
						}
						else if (map[py][px] is ']')
						{
							boxes.Add((px - 1, py));
							scratch.Add((px, py + dy));
							scratch.Add((px - 1, py + dy));
						}
					}

					(scratch, nextPoints) = (nextPoints, scratch);
				}

				if (!flag)
					continue;

				for (var i = boxes.Count - 1; i >= 0; i--)
				{
					var (bx, by) = boxes[i];
					(map[by + dy][bx], map[by][bx]) = ('[', '.');
					(map[by + dy][bx + 1], map[by][bx + 1]) = (']', '.');
				}

				map[ry][rx] = '.';
				ry += dy;
				map[ry][rx] = '@';
			}
		}

		var part2 = 0;
		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] == '[')
					part2 += (y * 100) + x;
			}
		}

		return (part1.ToString(), part2.ToString());
	}

	private static (int, int) GetDirection(char c) =>
		c switch
		{
			'<' => (-1, 0),
			'>' => (1, 0),
			'^' => (0, -1),
			'v' or _ => (0, 1),
		};
}
