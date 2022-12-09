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
				head = MoveHead(head, l[0]);
				tail = MoveFollower(head, tail);

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
				snake[0] = MoveHead(snake[0], l[0]);

				for (int j = 1; j < 10; j++)
				{
					var newTail = MoveFollower(snake[j - 1], snake[j]);

					if (newTail == snake[j])
						break;

					snake[j] = newTail;
				}

				tailVisited.Add(snake[^1]);
			}
		}

		var part2 = tailVisited.Count.ToString();

		return (part1, part2);
	}

	static (int x, int y) MoveHead((int x, int y) head, char dir) =>
		dir switch
		{
			'U' => (head.x, head.y - 1),
			'D' => (head.x, head.y + 1),
			'L' => (head.x - 1, head.y),
			'R' => (head.x + 1, head.y),
			_ => head,
		};

	static (int x, int y) MoveFollower((int x, int y) head, (int x, int y) tail) =>
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
}
