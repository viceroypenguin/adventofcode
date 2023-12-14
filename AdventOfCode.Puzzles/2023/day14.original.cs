namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 14, CodeType.Original)]
public partial class Day_14_Original : IPuzzle
{
	private const int Cycles = 1_000_000_000;

	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		TiltMapNorth(map);
		var part1 = GetLoad(map);

		map = input.Bytes.GetMap();
		var map2 = input.Bytes.GetMap();

		var i = 0;
		for (i = 0; i < Cycles; i++)
		{
			TiltMap(map);
			TiltMap(map2);
			TiltMap(map2);

			if (GetHashCode(map) == GetHashCode(map2))
				break;
		}

		var remainder = Cycles % (i + 1);
		for (i = 0; i < remainder; i++)
			TiltMap(map);

		var part2 = GetLoad(map);

		return (part1.ToString(), part2.ToString());
	}

	private static void TiltMap(byte[][] map)
	{
		TiltMapNorth(map);
		TiltMapWest(map);
		TiltMapSouth(map);
		TiltMapEast(map);
	}

	private static string PrintMap(byte[][] map)
	{
		return string.Join(Environment.NewLine,
			map.Select(l => string.Join("", l.Select(c => (char)c)))
				.Append(string.Empty));
	}

	private static void TiltMapNorth(byte[][] map)
	{
		for (var y = 1; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] != 'O')
					continue;

				for (var j = y; j > 0 && map[j - 1][x] == '.'; j--)
				{
					map[j - 1][x] = (byte)'O';
					map[j][x] = (byte)'.';
				}
			}
		}
	}

	private static void TiltMapWest(byte[][] map)
	{
		for (var x = 1; x < map[0].Length; x++)
		{
			for (var y = 0; y < map.Length; y++)
			{
				if (map[y][x] != 'O')
					continue;

				for (var j = x; j > 0 && map[y][j - 1] == '.'; j--)
				{
					map[y][j - 1] = (byte)'O';
					map[y][j] = (byte)'.';
				}
			}
		}
	}

	private static void TiltMapSouth(byte[][] map)
	{
		for (var y = map.Length - 2; y >= 0; y--)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] != 'O')
					continue;

				for (var j = y; j < map.Length - 1 && map[j + 1][x] == '.'; j++)
				{
					map[j + 1][x] = (byte)'O';
					map[j][x] = (byte)'.';
				}
			}
		}
	}

	private static void TiltMapEast(byte[][] map)
	{
		for (var x = map[0].Length - 2; x >= 0; x--)
		{
			for (var y = 0; y < map.Length; y++)
			{
				if (map[y][x] != 'O')
					continue;

				for (var j = x; j < map.Length - 1 && map[y][j + 1] == '.'; j++)
				{
					map[y][j + 1] = (byte)'O';
					map[y][j] = (byte)'.';
				}
			}
		}
	}

	private static int GetLoad(byte[][] map)
	{
		var sum = 0;
		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] == 'O')
					sum += map.Length - y;
			}
		}
		return sum;
	}

	private static int GetHashCode(byte[][] map)
	{
		return map.GetMapPoints().Aggregate(0, HashCode.Combine);
	}
}
