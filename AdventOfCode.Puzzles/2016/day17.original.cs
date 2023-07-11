using System.Security.Cryptography;

namespace AdventOfCode;

public class Day_2016_17_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 17;
	public override CodeType CodeType => CodeType.Original;

	Position destination = new Position { x = 3, y = 3 };
	string passcode;
	MD5 md5 = MD5.Create();

	public struct Position
	{
		public int x;
		public int y;
		public string Path;
		public int Steps;

		public override bool Equals(object other)
		{
			return Equals((Position)other);
		}

		public bool Equals(Position p2)
		{
			return
				this.x == p2.x &&
				this.y == p2.y;
		}

		public override int GetHashCode()
		{
			return Tuple.Create(
				this.x,
				this.y).GetHashCode();
		}
	}

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		passcode = input[..^1].GetString();

		var initialPosition = new Position { x = 0, y = 0, Path = "", Steps = 0, };

		var successfulPaths = new List<Position>();

		var queue = new Queue<Position>();
		queue.Enqueue(initialPosition);

		while (queue.Count != 0)
		{
			var position = queue.Dequeue();

			foreach (var p in GetNextPositions(position))
			{
				if (p.x == destination.x && p.y == destination.y)
				{
					if (successfulPaths.Count == 0)
						Dump('A', p.Path);
					successfulPaths.Add(p);
				}
				else
					queue.Enqueue(p);
			}
		}

		Dump('B', successfulPaths.Max(p => p.Steps));
	}

	IEnumerable<Position> GetNextPositions(Position p)
	{
		var hashSrc = passcode + p.Path;
		var bytes = Encoding.ASCII.GetBytes(hashSrc);
		var hash = md5.ComputeHash(bytes);

		if (p.x > 0 && (hash[0] >> 4) >= 0xb) yield return new Position { x = p.x - 1, y = p.y, Path = p.Path + 'U', Steps = p.Steps + 1 };
		if (p.x < 3 && (hash[0] & 0xf) >= 0xb) yield return new Position { x = p.x + 1, y = p.y, Path = p.Path + 'D', Steps = p.Steps + 1 };
		if (p.y > 0 && (hash[1] >> 4) >= 0xb) yield return new Position { x = p.x, y = p.y - 1, Path = p.Path + 'L', Steps = p.Steps + 1 };
		if (p.y < 3 && (hash[1] & 0xf) >= 0xb) yield return new Position { x = p.x, y = p.y + 1, Path = p.Path + 'R', Steps = p.Steps + 1 };
	}
}
