namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 04, CodeType.Original)]
public partial class Day_04_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();
		var count = 0;

		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] is not (byte)'X')
					continue;

				foreach (var (dx, dy) in MapExtensions.Adjacent)
				{
					var (nx, ny) = (x + dx, y + dy);

					if (nx < 0 || ny < 0 || nx >= map[y].Length || ny >= map.Length)
						continue;
					if (map[ny][nx] != (byte)'M')
						continue;

					(nx, ny) = (nx + dx, ny + dy);

					if (nx < 0 || ny < 0 || nx >= map[y].Length || ny >= map.Length)
						continue;
					if (map[ny][nx] != (byte)'A')
						continue;

					(nx, ny) = (nx + dx, ny + dy);

					if (nx < 0 || ny < 0 || nx >= map[y].Length || ny >= map.Length)
						continue;
					if (map[ny][nx] != (byte)'S')
						continue;

					count++;
				}
			}
		}

		var part1 = count.ToString();

		count = 0;

		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (x is 0 || y is 0 || x == map[y].Length - 1 || y == map.Length - 1)
					continue;

				if (map[y][x] != (byte)'A')
					continue;

				if ((
						(map[y - 1][x - 1] == 'M' && map[y + 1][x + 1] == 'S')
						|| (map[y - 1][x - 1] == 'S' && map[y + 1][x + 1] == 'M'))
					&& (
						(map[y - 1][x + 1] == 'M' && map[y + 1][x - 1] == 'S')
						|| (map[y - 1][x + 1] == 'S' && map[y + 1][x - 1] == 'M'))
				)
				{
					count++;
				}
			}
		}

		var part2 = count.ToString();

		return (part1, part2);
	}
}
