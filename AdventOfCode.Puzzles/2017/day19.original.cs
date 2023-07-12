namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 19, CodeType.Original)]
public class Day_19_Original : IPuzzle
{
	private byte[][] map;

	private (int x, int y) coords;
	private char direction;
	private int count;

	private Queue<byte> queue;

	public (string, string) Solve(PuzzleInput input)
	{
		map = input.Bytes.GetMap();

		coords = (x: 0, y: map[0].Select((c, i) => new { c, i }).First(x => x.c == '|').i);
		direction = 's';
		count = 0;

		queue = new();

		while (MoveNext())
			count++;

		var partA = string.Join("", queue.Select(c => (char)c));
		var partB = count;

		return (partA, partB.ToString());
	}

	private bool MoveNext()
	{
		// $"Coords: {coords}; Value: {map[coords.x][coords.y]}".Dump();
		switch (map[coords.x][coords.y])
		{
			case (byte)'|':
			case (byte)'-':
				MoveStraight();
				return true;

			case (byte)'+':
				ChangeDirection();
				return true;

			case (byte)' ':
				return false;

			default:
				queue.Enqueue(map[coords.x][coords.y]);
				goto case (byte)'|';
		}
	}

	private void MoveStraight()
	{
		switch (direction)
		{
			case 'n':
				coords.x--;
				return;

			case 's':
				coords.x++;
				return;

			case 'w':
				coords.y--;
				break;

			case 'e':
				coords.y++;
				break;

			default:
				throw new InvalidOperationException("wtf?");
		}
	}

	private void ChangeDirection()
	{
		if (direction != 's' &&
			coords.x > 0 &&
			map[coords.x - 1][coords.y] != ' ')
		{
			direction = 'n';
			MoveStraight();
			return;
		}

		if (direction != 'n' &&
			coords.x < (map.Length - 1) &&
			map[coords.x + 1][coords.y] != ' ')
		{
			direction = 's';
			MoveStraight();
			return;
		}

		if (direction != 'e' &&
			coords.y > 0 &&
			map[coords.x][coords.y - 1] != ' ')
		{
			direction = 'w';
			MoveStraight();
			return;
		}

		if (direction != 'w' &&
			coords.y < (map[coords.x].Length - 1) &&
			map[coords.x][coords.y + 1] != ' ')
		{
			direction = 'e';
			MoveStraight();
			return;
		}
	}
}
