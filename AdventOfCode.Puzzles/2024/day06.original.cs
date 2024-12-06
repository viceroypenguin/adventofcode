
using System;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 06, CodeType.Original)]
public partial class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		var guard = map.GetMapPoints()
			.Where(i => i.item == '^')
			.Select(i => i.p)
			.First();

		var part1 = DoPart1(map, guard);

		var part2 = DoPart2(map, guard);

		return (part1, part2);
	}

	private static string DoPart1(byte[][] map, (int x, int y) guard)
	{
		var dir = '^';

		var positions = new HashSet<(int, int)>() { guard };
		while (true)
		{
			var newPos = MovePosition(guard, dir);

			if (newPos.y < 0 || newPos.y >= map.Length || newPos.x < 0 || newPos.x >= map[0].Length)
			{
				break;
			}
			else if (map[newPos.y][newPos.x] == '#')
			{
				dir = RotateDirection(dir);
			}
			else
			{
				positions.Add(newPos);
				guard = newPos;
			}
		}

		var part1 = positions.Count.ToString();
		return part1;
	}

	private static char RotateDirection(char dir) =>
		dir switch
		{
			'^' => '>',
			'>' => 'v',
			'v' => '<',
			'<' => '^',
		};

	private static (int x, int y) MovePosition((int x, int y) guard, char dir) =>
		dir switch
		{
			'^' => (x: guard.x, y: guard.y - 1),
			'>' => (x: guard.x + 1, y: guard.y),
			'v' => (x: guard.x, y: guard.y + 1),
			'<' => (x: guard.x - 1, y: guard.y),
		};

	private static string DoPart2(byte[][] map, (int x, int y) guard)
	{
		var obstructionCount = 0;

		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] == '#')
					continue;

				var ch = map[y][x];
				map[y][x] = (byte)'#';
				if (IsObstructionLoop(map, guard))
					obstructionCount++;
				map[y][x] = ch;
			}
		}

		return obstructionCount.ToString();
	}

	private static bool IsObstructionLoop(byte[][] map, (int x, int y) guard)
	{
		var dir = '^';

		var positions = new HashSet<(int, int, char)>() { (guard.x, guard.y, dir) };
		while (true)
		{
			var newPos = MovePosition(guard, dir);

			if (newPos.y < 0 || newPos.y >= map.Length || newPos.x < 0 || newPos.x >= map[0].Length)
			{
				return false;
			}
			else if (map[newPos.y][newPos.x] == '#')
			{
				dir = RotateDirection(dir);
			}
			else
			{
				if (!positions.Add((newPos.x, newPos.y, dir)))
					return true;

				guard = newPos;
			}
		}
	}
}
