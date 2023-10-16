using System.Numerics;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 9, CodeType.Fastest)]
public partial class Day_09_Fastest : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		Span<Vector2> snake = new Vector2[10];
		var part1 = new HashSet<Vector2>(input.Bytes.Length / 4) { new(0, 0), };
		var part2 = new HashSet<Vector2>(input.Bytes.Length / 4) { new(0, 0), };

		foreach (var l in input.Lines)
		{
			var (cnt, _) = l.AsSpan()[2..].AtoI();
			var dir = l[0] switch
			{
				'U' => new Vector2(0, -1),
				'D' => new Vector2(0, +1),
				'L' => new Vector2(-1, 0),
				'R' => new Vector2(+1, 0),
				_ => new Vector2(0, 0),
			};

			for (var i = 0; i < cnt; i++)
			{
				snake[0] += dir;

				for (var j = 1; j < 10; j++)
				{
					var move = snake[j - 1] - snake[j];

					move = move.Length() < 2
						? Vector2.Zero
						: Vector2.Clamp(move, -Vector2.One, Vector2.One);

					if (move == Vector2.Zero)
						break;

					snake[j] += move;
				}

				part1.Add(snake[1]);
				part2.Add(snake[^1]);
			}
		}

		return (part1.Count.ToString(), part2.Count.ToString());
	}
}
