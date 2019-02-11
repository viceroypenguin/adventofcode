using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2016_24_Original : Day
	{
		public override int Year => 2016;
		public override int DayNumber => 24;
		public override CodeType CodeType => CodeType.Original;

		int[][] map;

		const int WALL = -2;
		const int FLOOR = -1;

		class Position : IEquatable<Position>
		{
			public int x;
			public int y;
			public bool[] visited;
			public int steps;

			public override int GetHashCode()
			{
				unchecked
				{
					const int p = 16777619;
					int hash = (int)2166136261;

					hash = (hash ^ (byte)x) * p;
					hash = (hash ^ (byte)y) * p;
					for (int i = 0; i < visited.Length; i++)
						hash = (hash ^ (visited[i] ? (byte)1 : (byte)0)) * p;

					hash += hash << 13;
					hash ^= hash >> 7;
					hash += hash << 3;
					hash ^= hash >> 17;
					hash += hash << 5;
					return hash;
				}
			}

			public override bool Equals(object other)
			{
				return Equals((Position)other);
			}

			public bool Equals(Position other)
			{
				return
					this.x == other.x &&
					this.y == other.y &&
					this.visited.Length == other.visited.Length &&
					!this.visited.Zip(other.visited, (a, b) => a == b).Where(b => !b).Any();
			}
		}

		HashSet<Position> visitedLocations = new HashSet<Position>();

		protected override void ExecuteDay(byte[] input)
		{
			map =
				input.GetLines()
					.Select(s => s
						.Select(c =>
							c == '#' ? WALL :
							c == '.' ? FLOOR :
							c - '0')
						.ToArray())
					.ToArray();

			DoPartA();
			DoPartB();
		}

		private void DoPartA()
		{
			var initialPosition = new Position();
			var maxNum = -1;
			for (int y = 0; y < map.Length; y++)
				for (int x = 0; x < map[y].Length; x++)
				{
					if (map[y][x] == 0)
					{
						initialPosition.x = x;
						initialPosition.y = y;
					}

					if (map[y][x] > maxNum)
						maxNum = map[y][x];
				}

			initialPosition.steps = 0;
			initialPosition.visited = new bool[maxNum + 1];
			initialPosition.visited[0] = true;

			visitedLocations.Add(initialPosition);

			var queue = new Queue<Position>();
			queue.Enqueue(initialPosition);

			while (queue.Count > 0)
			{
				var pos = queue.Dequeue();

				foreach (var p in GetNextPositions(pos))
				{
					if (!p.visited.Any(b => !b))
					{
						Dump('A', p.steps);
						return;
					}

					queue.Enqueue(p);
				}
			}
		}

		private void DoPartB()
		{
			visitedLocations = new HashSet<Position>();
			var initialPosition = new Position();
			var maxNum = -1;
			for (int y = 0; y < map.Length; y++)
				for (int x = 0; x < map[y].Length; x++)
				{
					if (map[y][x] == 0)
					{
						initialPosition.x = x;
						initialPosition.y = y;
					}

					if (map[y][x] > maxNum)
						maxNum = map[y][x];
				}

			maxNum++;

			map[initialPosition.y][initialPosition.x] = maxNum;

			initialPosition.steps = 0;
			initialPosition.visited = new bool[maxNum + 1];
			initialPosition.visited[0] = true;

			visitedLocations.Add(initialPosition);

			var queue = new Queue<Position>();
			queue.Enqueue(initialPosition);

			while (queue.Count > 0)
			{
				var pos = queue.Dequeue();

				foreach (var p in GetNextPositions(pos))
				{
					if (!p.visited.Any(b => !b))
					{
						Dump('B', p.steps);
						return;
					}

					queue.Enqueue(p);
				}
			}
		}

		IEnumerable<Position> GetNextPositions(Position p)
		{
			foreach (var newP in _GetNextPositions(p))
			{
				if (visitedLocations.Contains(newP))
					continue;
				visitedLocations.Add(newP);
				yield return newP;
			}
		}

		IEnumerable<Position> _GetNextPositions(Position p)
		{
			foreach (var newP in __GetNextPositions(p))
			{
				var space = map[newP.y][newP.x];
				switch (space)
				{
					case WALL: continue;
					case FLOOR:
						{
							newP.visited = p.visited;
							yield return newP;
							break;
						}

					default:
						{
							newP.visited = p.visited.ToArray();
							newP.visited[space] = true;
							yield return newP;
							break;
						}
				}
			}
		}

		IEnumerable<Position> __GetNextPositions(Position p)
		{
			if (p.y > 0) yield return new Position { x = p.x, y = p.y - 1, steps = p.steps + 1 };
			if (p.x > 0) yield return new Position { x = p.x - 1, y = p.y, steps = p.steps + 1 };
			if (p.y < map.Length - 1) yield return new Position { x = p.x, y = p.y + 1, steps = p.steps + 1 };
			if (p.x < map[p.y].Length - 1) yield return new Position { x = p.x + 1, y = p.y, steps = p.steps + 1 };
		}
	}
}
