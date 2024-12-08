using System.Runtime.CompilerServices;

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

		var dir = 0;
		var positions = new HashSet<(int, int)>() { guard };
		var obstructionSeen = new HashSet<(int, int, int)>();
		var obstructionCount = 0;

		while (true)
		{
			var (x, y) = MovePosition(guard, dir);

			if (!(x, y).IsValid(map))
				break;

			if (map[y][x] == '#')
			{
				dir = RotateDirection(dir);
				continue;
			}

			if (positions.Add((x, y)))
			{
				(var ch, map[y][x]) = (map[y][x], (byte)'#');

				if (IsObstructionLoop(map, guard, RotateDirection(dir), obstructionSeen))
					obstructionCount++;
				map[y][x] = ch;
			}

			guard = (x, y);
		}

		return (positions.Count.ToString(), obstructionCount.ToString());
	}

	private static int RotateDirection(int dir) => (dir + 1) % 4;

	private static (int x, int y) MovePosition((int x, int y) guard, int dir) =>
		dir switch
		{
			0 => (x: guard.x, y: guard.y - 1),
			1 => (x: guard.x + 1, y: guard.y),
			2 => (x: guard.x, y: guard.y + 1),
			3 => (x: guard.x - 1, y: guard.y),
			_ => default,
		};

	private static bool IsObstructionLoop(byte[][] map, (int x, int y) guard, int dir, HashSet<(int, int, int)> positions)
	{
		positions.Clear();
		positions.Add((guard.x, guard.y, dir));

		while (true)
		{
			var (x, y) = MovePosition(guard, dir);

			if (!(x, y).IsValid(map))
				return false;

			if (map[y][x] == '#')
			{
				dir = RotateDirection(dir);
			}
			else
			{
				if (!positions.Add((x, y, dir)))
					return true;

				guard = (x, y);
			}
		}
	}
}
