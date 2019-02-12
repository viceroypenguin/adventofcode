using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2018_13_Original : Day
	{
		public override int Year => 2018;
		public override int DayNumber => 13;
		public override CodeType CodeType => CodeType.Original;

		static Cart[] carts;
		static char[][] map;

		enum Direction
		{
			Left = 0,
			Up,
			Right,
			Down,
		}

		protected override void ExecuteDay(byte[] input)
		{
			map = input.GetLines()
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
				map[c.X][c.Y] = c.Dir == Direction.Left || c.Dir == Direction.Right ? '-' : '|';

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
						Dump('A', $"{c.Y},{c.X}");
						firstCrash = true;
					}
					carts = carts
						.Where(c => !c.IsCrashed)
						.ToArray();

					if (carts.Length == 1)
					{
						Dump('B', $"{carts[0].Y},{carts[0].X}");
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
	}
}
