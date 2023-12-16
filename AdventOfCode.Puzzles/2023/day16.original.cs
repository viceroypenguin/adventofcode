namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 16, CodeType.Original)]
public partial class Day_16_Original : IPuzzle
{
	private enum Dir { North, East, South, West, }
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();
		var max = GetActivatedTiles(map, (0, 0, Dir.East));

		var part1 = max;

		max = Math.Max(max, GetActivatedTiles(map, (map[0].Length - 1, 0, Dir.West)));
		for (var y = 1; y < map.Length; y++)
		{
			max = Math.Max(max, GetActivatedTiles(map, (0, 0, Dir.East)));
			max = Math.Max(max, GetActivatedTiles(map, (map[0].Length - 1, 0, Dir.West)));
		}

		for (var x = 0; x < map[0].Length; x++)
		{
			max = Math.Max(max, GetActivatedTiles(map, (x, 0, Dir.South)));
			max = Math.Max(max, GetActivatedTiles(map, (x, map.Length - 1, Dir.North)));
		}

		var part2 = max;
		return (part1.ToString(), part2.ToString());
	}

	private static int GetActivatedTiles(byte[][] map, (int x, int y, Dir dir) start)
	{
		var queue = new Queue<(int x, int y, Dir dir)>();
		var seen = new HashSet<(int x, int y, Dir dir)>();
		queue.Enqueue(start);
		while (queue.Count > 0)
		{
			var (x, y, dir) = queue.Dequeue();

			static (int x, int y) Advance(int x, int y, Dir dir) =>
				dir switch
				{
					Dir.North => (x, y - 1),
					Dir.East => (x + 1, y),
					Dir.South => (x, y + 1),
					Dir.West => (x - 1, y),
				};

			while (true)
			{
				if (!seen.Add((x, y, dir)))
					break;

				var c = map[y][x];
				if (c == '/')
				{
					dir = dir switch
					{
						Dir.North => Dir.East,
						Dir.South => Dir.West,
						Dir.West => Dir.South,
						Dir.East => Dir.North,
					};
				}
				else if (c == '\\')
				{
					dir = dir switch
					{
						Dir.North => Dir.West,
						Dir.South => Dir.East,
						Dir.West => Dir.North,
						Dir.East => Dir.South,
					};
				}
				else if (c == '-')
				{
					if (dir is Dir.North or Dir.South)
					{
						queue.Enqueue((x, y, Dir.East));
						queue.Enqueue((x, y, Dir.West));
						break;
					}
				}
				else if (c == '|')
				{
					if (dir is Dir.East or Dir.West)
					{
						queue.Enqueue((x, y, Dir.North));
						queue.Enqueue((x, y, Dir.South));
						break;
					}
				}

				(x, y) = Advance(x, y, dir);
				if (x < 0 || y < 0) break;
				if (x >= map[0].Length || y >= map.Length) break;
			}
		}

		return seen.Select(x => (x.x, x.y)).Distinct().Count();
	}
}
