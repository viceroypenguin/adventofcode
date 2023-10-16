namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 13, CodeType.Original)]
public class Day_13_Original : IPuzzle
{
	private static Cart[] carts;
	private static char[][] map;

	private enum Direction
	{
		Left = 0,
		Up,
		Right,
		Down,
	}

	public (string, string) Solve(PuzzleInput input)
	{
		map = input.Lines
			.Select(s => s.ToArray())
			.ToArray();

		var id = 1;
		carts = (
			from x in Enumerable.Range(0, map.Length)
			from y in Enumerable.Range(0, map[x].Length)
			let c = map[x][y]
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
			map[c.X][c.Y] = c.Dir is Direction.Left or Direction.Right ? '-' : '|';

		var ticks = 0;
		var firstCrash = false;
		var part1 = string.Empty;
		while (true)
		{
			ticks++;
			foreach (var c in carts
				.OrderBy(c => c.Y)
				.ThenBy(c => c.X))
			{
				c.MoveNext();
			}

			if (carts.Any(c => c.IsCrashed))
			{
				if (!firstCrash)
				{
					var c = carts.First(x => x.IsCrashed);
					part1 = $"{c.Y},{c.X}";
					firstCrash = true;
				}
				carts = carts
					.Where(c => !c.IsCrashed)
					.ToArray();

				if (carts.Length == 1)
				{
					var part2 = $"{carts[0].Y},{carts[0].X}";
					return (part1, part2);
				}
			}
		}
	}

	private sealed class Cart
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

			// $"Coords: {coords}; Value: {map[coords.x][coords.y]}".Dump();
			switch (map[X][Y])
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
				c => c.Id != Id &&
					c.X == X &&
					c.Y == Y);

			if (r != null)
			{
				IsCrashed = true;
				r.IsCrashed = true;
			}
		}

		private void MoveStraight()
		{
			switch (Dir)
			{
				case Direction.Up:
					X--;
					if (map[X][Y] == '/')
						Dir = Direction.Right;
					if (map[X][Y] == '\\')
						Dir = Direction.Left;
					return;

				case Direction.Down:
					X++;
					if (map[X][Y] == '/')
						Dir = Direction.Left;
					if (map[X][Y] == '\\')
						Dir = Direction.Right;
					return;

				case Direction.Left:
					Y--;
					if (map[X][Y] == '/')
						Dir = Direction.Down;
					if (map[X][Y] == '\\')
						Dir = Direction.Up;
					break;

				case Direction.Right:
					Y++;
					if (map[X][Y] == '/')
						Dir = Direction.Up;
					if (map[X][Y] == '\\')
						Dir = Direction.Down;
					break;

				default:
					throw new InvalidOperationException("wtf?");
			}
		}

		private void ChangeDirection()
		{
			Dir = (Direction)(((int)Dir + NextChangeDir + 4) % 4);
			NextChangeDir =
				NextChangeDir == -1 ? 0 :
				NextChangeDir == 0 ? 1 :
				-1;

			MoveStraight();
		}
	}
}
