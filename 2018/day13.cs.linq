<Query Kind="Program" />

static Cart[] carts;
static char[][] input;

enum Direction
{
	Left = 0,
	Up,
	Right,
	Down,
}

void Main()
{
	input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day13.input.txt"))
		.Select(s => s.ToArray())
		.ToArray();

	var id = 1;
	carts = (
		from x in Enumerable.Range(0, input.Length)
		from y in Enumerable.Range(0, input[x].Length)
		let c = input[x][y]
		where new[] { '<', '>', '^', 'v' }.Contains(c)
		select new Cart
		{
			Id = id++,
			X = x,
			Y = y,
			Dir = c == '<' ? Direction.Left :
				c == '^' ? Direction.Up :
				c == '>' ? Direction.Right :
				Direction.Down,
			NextChangeDir = -1,
		}).ToArray();

	foreach (var c in carts)
		input[c.X][c.Y] = c.Dir == Direction.Left || c.Dir == Direction.Right ? '-' : '|';

	var ticks = 0;
	var firstCrash = false;
	while (true)
	{
		ticks++;
		foreach (var c in carts
				.OrderBy(c => c.Y)
				.ThenBy(c => c.X))
			c.MoveNext();

		if (carts.Any(c => c.IsCrashed))
		{
			if (!firstCrash)
			{
				var c = carts.First(x => x.IsCrashed);
				$"{c.Y},{c.X}".Dump("Part A");
				firstCrash = true;
			}
			carts = carts
				.Where(c => !c.IsCrashed)
				.ToArray();

			if (carts.Length == 1)
			{
				$"{carts[0].Y},{carts[0].X}".Dump("Part B");
				return;
			}
		}
	}
}

class Cart
{
	public int Id;
	public int X;
	public int Y;
	public Direction Dir;
	public int NextChangeDir;
	public bool IsCrashed;

	public void MoveNext()
	{
		if (IsCrashed)
			return;

		// $"Coords: {coords}; Value: {input[coords.x][coords.y]}".Dump();
		switch (input[X][Y])
		{
			case '|':
			case '-':
			case '/':
			case '\\':
				MoveStraight();
				break;

			case '+':
				ChangeDirection();
				break;

			default:
				throw new InvalidOperationException("wtf?");
		}

		var r = carts.FirstOrDefault(
			c => c.Id != this.Id &&
				c.X == this.X &&
				c.Y == this.Y);

		if (r != null)
		{
			IsCrashed = true;
			r.IsCrashed = true;
		}
	}

	void MoveStraight()
	{
		switch (Dir)
		{
			case Direction.Up:
				X--;
				if (input[X][Y] == '/')
					Dir = Direction.Right;
				if (input[X][Y] == '\\')
					Dir = Direction.Left;
				return;

			case Direction.Down:
				X++;
				if (input[X][Y] == '/')
					Dir = Direction.Left;
				if (input[X][Y] == '\\')
					Dir = Direction.Right;
				return;

			case Direction.Left:
				Y--;
				if (input[X][Y] == '/')
					Dir = Direction.Down;
				if (input[X][Y] == '\\')
					Dir = Direction.Up;
				break;

			case Direction.Right:
				Y++;
				if (input[X][Y] == '/')
					Dir = Direction.Up;
				if (input[X][Y] == '\\')
					Dir = Direction.Down;
				break;

			default:
				throw new InvalidOperationException("wtf?");
		}
	}

	void ChangeDirection()
	{
		Dir = (Direction)(((int)Dir + NextChangeDir + 4) % 4);
		NextChangeDir =
			NextChangeDir == -1 ? 0 :
			NextChangeDir == 0 ? 1 :
			-1;

		MoveStraight();
	}
}