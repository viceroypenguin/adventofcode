namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Text;
		var width = map.IndexOf('\n');

		var part1 = RunPart(map, width, immediate: true);
		var part2 = RunPart(map, width, immediate: false);
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

	private static string RunStep(string map, int width, bool immediate)
	{
		Span<char> newMap = stackalloc char[map.Length];
		for (var y = 0; y < map.Length; y += width + 1)
		{
			for (var x = 0; x < width + 1; x++)
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

				foreach (var (yadj, xadj) in MapExtensions.Adjacent)
				{
					var (newY, newX) = (y, x);

					do
					{
						newY += (width + 1) * yadj;
						if (!newY.Between(0, map.Length - (width + 1)))
							break;

						newX += xadj;
						if (!newX.Between(0, width))
							break;

						if (map[newY + newX] == '#')
						{
							cnt++;
							break;
						}

						if (map[newY + newX] == 'L')
							break;
					}
					while (!immediate);
				}

#pragma warning disable IDE0072 // Bug in IDE0072 doesn't realize `(var c, _, _)` covers remaining cases
				newMap[y + x] = (map[y + x], immediate, cnt) switch
				{
					('L', _, 0) => '#',
					('#', true, >= 4) => 'L',
					('#', false, >= 5) => 'L',
					(var c, _, _) => c,
				};
#pragma warning restore IDE0072
			}
		}

		return new string(newMap);
	}
}
