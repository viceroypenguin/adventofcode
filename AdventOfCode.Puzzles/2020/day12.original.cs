using System.Diagnostics;

namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 12, CodeType.Original)]
public class Day_12_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = DoPartA(input.Lines);
		var part2 = DoPartB(input.Lines);
		return (part1, part2);
	}

	private static string DoPartA(string[] lines)
	{
		(int x, int y, int d) Move(int x, int y, int d, int dir, int amount) =>
			(x, y, d) = dir switch
			{
				0 => (x + amount, y, d),
				1 => (x, y - amount, d),
				2 => (x - amount, y, d),
				3 => (x, y + amount, d),
				_ => throw new UnreachableException(),
			};

		(int x, int y, int d) RotateDir(int x, int y, int d, int a) =>
			(x, y, (d + a) % 4);

		int x = 0, y = 0, d = 0;
		foreach (var (dir, n) in lines.Select(s => (s[0], Convert.ToInt32(s[1..]))))
		{
			(x, y, d) = dir switch
			{
				'F' => Move(x, y, d, d, n),

				'E' => Move(x, y, d, 0, n),
				'S' => Move(x, y, d, 1, n),
				'W' => Move(x, y, d, 2, n),
				'N' => Move(x, y, d, 3, n),

				'R' => RotateDir(x, y, d, n / 90),
				'L' => RotateDir(x, y, d, 4 - (n / 90)),

				_ => throw new UnreachableException(),
			};
		}

		return (Math.Abs(x) + Math.Abs(y)).ToString();
	}

	private static string DoPartB(string[] lines)
	{
		static (int x, int y, int wayx, int wayy) RotateWaypoint(int x, int y, int wayx, int wayy, int a) =>
			a switch
			{
				0 => (x, y, wayx, wayy),
				1 => (x, y, wayy, -wayx),
				2 => (x, y, -wayx, -wayy),
				3 => (x, y, -wayy, wayx),
				_ => throw new UnreachableException(),
			};

		int wayx = 10, wayy = 1;
		int x = 0, y = 0;
		foreach (var (dir, n) in lines.Select(s => (s[0], Convert.ToInt32(s[1..]))))
		{
			(x, y, wayx, wayy) = dir switch
			{
				'F' => (x + (wayx * n), y + (wayy * n), wayx, wayy),

				'E' => (x, y, wayx + n, wayy),
				'S' => (x, y, wayx, wayy - n),
				'W' => (x, y, wayx - n, wayy),
				'N' => (x, y, wayx, wayy + n),

				'R' => RotateWaypoint(x, y, wayx, wayy, n / 90),
				'L' => RotateWaypoint(x, y, wayx, wayy, 4 - (n / 90)),

				_ => throw new UnreachableException(),
			};
		}

		return (Math.Abs(x) + Math.Abs(y)).ToString();
	}
}
