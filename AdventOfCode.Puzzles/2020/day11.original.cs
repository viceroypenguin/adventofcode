namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Text;
		var width = map.IndexOf('\n');

		var part1 = RunPart(map, width, true);
		var part2 = RunPart(map, width, false);
		return (part1, part2);
	}

	private static string RunPart(string map, int width, bool immediate)
	{
		while (true)
		{
			var nextMap = RunStep(map, width, immediate);

			if (map == nextMap)
				break;

			map = nextMap;
		}

		return map.Count(c => c == '#').ToString();
	}

	private static readonly (int yadj, int xadj)[] dirs = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1), };
	private static string RunStep(string map, int width, bool immediate)
	{
		Span<char> newMap = stackalloc char[map.Length];
		for (int y = 0; y < map.Length; y += width + 1)
			for (int x = 0; x < width + 1; x++)
			{
				if (map[y + x] == '.')
				{
					newMap[y + x] = '.';
					continue;
				}
				if (map[y + x] == '\n')
				{
					newMap[y + x] = '\n';
					continue;
				}

				var cnt = 0;

				foreach (var (yadj, xadj) in dirs)
				{
					int _y = y, _x = x;

					do
					{
						_y += (width + 1) * yadj;
						if (!_y.Between(0, map.Length - (width + 1)))
							break;
						_x += xadj;
						if (!_x.Between(0, width))
							break;

						if (map[_y + _x] == '#')
						{
							cnt++;
							break;
						}

						if (map[_y + _x] == 'L')
							break;
					}
					while (!immediate);
				}

				newMap[y + x] = (map[y + x], immediate, cnt) switch
				{
					('L', _, 0) => '#',
					('#', true, >= 4) => 'L',
					('#', false, >= 5) => 'L',
					(var c, _, _) => c,
				};
			}

		return new string(newMap);
	}
}
