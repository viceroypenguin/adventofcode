using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 18, CodeType.Original)]
public partial class Day_18_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = GetArea(input.Lines
			.Select(x => x.Split())
			.Select(l => (int.Parse(l[1]), l[0][0])));

		var part2 = GetArea(input.Lines
			.Select(x => x.Split())
			.Select(l => ConvertHex(l[2])));

		return (part1.ToString(), part2.ToString());
	}

	private static long GetArea(IEnumerable<(int num, char dir)> corners)
	{
		var p = (x: 0L, y: 0L);
		var area = 0L;
		var dist = 0L;

		foreach (var (num, dir) in corners)
		{
			var q = dir switch
			{
				'U' => (x: p.x, y: p.y - num),
				'D' => (x: p.x, y: p.y + num),
				'L' => (x: p.x - num, y: p.y),
				'R' => (x: p.x + num, y: p.y),
				_ => throw new UnreachableException(),
			};

			area += (p.x * q.y) - (q.x * p.y);
			dist += num;

			p = q;
		}

		return Math.Abs(area / 2) + (dist / 2) + 1;
	}

	private static (int num, char dir) ConvertHex(string hex)
	{
		var dir = hex[^2] switch
		{
			'0' => 'R',
			'1' => 'D',
			'2' => 'L',
			'3' => 'U',
			_ => throw new UnreachableException(),
		};

		var num = Convert.ToInt32(hex[2..7], 16);
		return (num, dir);
	}
}
