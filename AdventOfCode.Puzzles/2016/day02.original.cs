using System.Diagnostics;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 02, CodeType.Original)]
public class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		return (
			DoPartA(input.Lines),
			DoPartB(input.Lines));
	}

	private static string DoPartA(string[] lines)
	{
		var buttons = new[]
		{
			new [] { 1, 2, 3 },
			[4, 5, 6],
			[7, 8, 9],
		};

		var p = (x: 1, y: 1);

		var password = new List<int>();

		foreach (var line in lines)
		{
			foreach (var c in line.Trim())
			{
				p = c switch
				{
					'U' => (p.x, Math.Max(p.y - 1, 0)),
					'D' => (p.x, Math.Min(p.y + 1, 2)),
					'L' => (Math.Max(p.x - 1, 0), p.y),
					'R' => (Math.Min(p.x + 1, 2), p.y),
					_ => throw new UnreachableException(),
				};
			}

			password.Add(buttons[p.y][p.x]);
		}

		return string.Join("", password);
	}

	private static string DoPartB(string[] lines)
	{
		var buttons = new[]
		{
			new char? [] { null, null,  '1', null, null, },
			[null,  '2',  '3',  '4', null,],
			['5',  '6',  '7',  '8',  '9',],
			[null,  'A',  'B',  'C', null,],
			[null, null,  'D', null, null,],
		};

		var p = (x: 0, y: 2);

		var password = new List<char>();

		foreach (var line in lines)
		{
			foreach (var c in line.Trim())
			{
				var pos = c switch
				{
					'U' => (p.x, y: Math.Max(p.y - 1, 0)),
					'D' => (p.x, y: Math.Min(p.y + 1, 4)),
					'L' => (x: Math.Max(p.x - 1, 0), p.y),
					'R' => (x: Math.Min(p.x + 1, 4), p.y),
					_ => throw new UnreachableException(),
				};

				if (buttons[pos.y][pos.x].HasValue)
					p = pos;
			}

			password.Add(buttons[p.y][p.x].Value);
		}

		return string.Join("", password);
	}
}
