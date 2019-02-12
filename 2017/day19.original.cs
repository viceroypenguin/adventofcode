using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_19_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 19;
		public override CodeType CodeType => CodeType.Original;

		char[][] map;

		(int x, int y) coords;
		char direction;
		int count;

		Queue<char> queue;

		protected override void ExecuteDay(byte[] input)
		{
			map = input.GetLines()
				.Select(s => s.ToArray())
				.ToArray();

			coords = (x: 0, y: map[0].Select((c, i) => new { c, i }).First(x => x.c == '|').i);
			direction = 's';
			count = 0;

			queue = new Queue<char>();

			while (MoveNext())
				count++;

			Dump('A', string.Join("", queue));
			Dump('B', count);
		}

		bool MoveNext()
		{
			// $"Coords: {coords}; Value: {map[coords.x][coords.y]}".Dump();
			switch (map[coords.x][coords.y])
			{
				case '|':
				case '-':
					MoveStraight();
					return true;

				case '+':
					ChangeDirection();
					return true;

				case ' ':
					return false;

				default:
					queue.Enqueue(map[coords.x][coords.y]);
					goto case '|';
			}
		}

		void MoveStraight()
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

		void ChangeDirection()
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
}
