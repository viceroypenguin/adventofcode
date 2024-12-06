
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

		var positions = RunGuard(map, guard);
		var part1 = positions.Count.ToString();

		var part2 = DoPart2(map, guard, positions);

		return (part1, part2);
	}

	private static HashSet<(int, int)> RunGuard(byte[][] map, (int x, int y) guard)
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

		return positions;
	}

	private static char RotateDirection(char dir) =>
		dir switch
		{
			'^' => '>',
			'>' => 'v',
			'v' => '<',
			'<' => '^',
			_ => ' ',
		};

	private static (int x, int y) MovePosition((int x, int y) guard, char dir) =>
		dir switch
		{
			'^' => (x: guard.x, y: guard.y - 1),
			'>' => (x: guard.x + 1, y: guard.y),
			'v' => (x: guard.x, y: guard.y + 1),
			'<' => (x: guard.x - 1, y: guard.y),
			_ => default,
		};

	private static string DoPart2(byte[][] map, (int x, int y) guard, HashSet<(int, int)> positions)
	{
		var obstructionCount = 0;

		foreach (var (x, y) in positions)
		{
			var ch = map[y][x];
			map[y][x] = (byte)'#';
			if (IsObstructionLoop(map, guard))
				obstructionCount++;
			map[y][x] = ch;
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
