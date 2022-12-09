namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 9, CodeType.Original)]
public partial class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var head = (x: 0, y: 0);
		var tail = (x: 0, y: 0);
		var tailVisited = new HashSet<(int, int)>() { (0, 0), };

		foreach (var l in input.Lines)
		{
			var cnt = int.Parse(l.AsSpan()[2..]);
			for (int i = 0; i < cnt; i++)
			{
				head = l[0] switch
				{
					'U' => (head.x, head.y - 1),
					'D' => (head.x, head.y + 1),
					'L' => (head.x - 1, head.y),
					'R' => (head.x + 1, head.y),
				};

				tail =
					(head.x - tail.x, head.y - tail.y) switch
					{
						( > 1, _) => (head.x - 1, head.y),
						( < -1, _) => (head.x + 1, head.y),
						(_, > 1) => (head.x, head.y - 1),
						(_, < -1) => (head.x, head.y + 1),
						_ => (tail.x, tail.y),
					};

				tailVisited.Add(tail);
			}
		}

		var part1 = tailVisited.Count.ToString();

		var snake = new (int x, int y)[10];
		tailVisited = new HashSet<(int, int)>() { (0, 0), };

		foreach (var l in input.Lines)
		{
			var cnt = int.Parse(l.AsSpan()[2..]);
			for (int i = 0; i < cnt; i++)
			{
				snake[0] = l[0] switch
				{
					'U' => (snake[0].x, snake[0].y - 1),
					'D' => (snake[0].x, snake[0].y + 1),
					'L' => (snake[0].x - 1, snake[0].y),
					'R' => (snake[0].x + 1, snake[0].y),
				};

				for (int j = 1; j < 10; j++)
				{
					head = snake[j - 1];
					tail = snake[j];

					var newTail =
						(head.x - tail.x, head.y - tail.y) switch
						{
							( > 1, > 1) => (head.x - 1, head.y - 1),
							( > 1, < -1) => (head.x - 1, head.y + 1),
							( < -1, > 1) => (head.x + 1, head.y - 1),
							( < -1, < -1) => (head.x + 1, head.y + 1),
							( > 1, _) => (head.x - 1, head.y),
							( < -1, _) => (head.x + 1, head.y),
							(_, > 1) => (head.x, head.y - 1),
							(_, < -1) => (head.x, head.y + 1),
							_ => (tail.x, tail.y),
						};

					if (newTail == tail)
						break;

					snake[j] = newTail;
				}

				tailVisited.Add(snake[^1]);
			}
		}

		var part2 = tailVisited.Count.ToString();

		return (part1, part2);
	}
}
