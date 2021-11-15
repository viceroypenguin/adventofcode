namespace AdventOfCode;

public class Day_2016_13_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 13;
	public override CodeType CodeType => CodeType.Original;

	int number;
	Position destination = new Position { x = 31, y = 39 };

	uint BitCount(uint i)
	{
		i = (i & 0x55555555) + ((i >> 1) & 0x55555555);     // adjacent bits grouped
		i = (i & 0x33333333) + ((i >> 2) & 0x33333333);     // adjacent 2-bit groups paired
		i = (i & 0x0F0F0F0F) + ((i >> 4) & 0x0F0F0F0F);     // adjacent 4-bit groups paired
		i = (i & 0x00FF00FF) + ((i >> 8) & 0x00FF00FF);     // adjacent 8-bit groups paired
		i = (i & 0x0000FFFF) + ((i >> 16) & 0x0000FFFF);     // adjacent 16-bit groups paired
		return (i & 0x0000003F);    // a 32-bit unsigned int can have at most 32 set bits, 
									// which in decimal needs on 6 bits to represent
	}

	bool IsWall(int x, int y)
	{
		var num = x * x + 3 * x + 2 * x * y + y + y * y + number;
		var bitCount = BitCount((uint)num);
		return (bitCount % 2) == 1;
	}

	public struct Position
	{
		public int x;
		public int y;
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

	HashSet<Position> _VisitedStates = new HashSet<Position>();
	public bool HasVisitedPosition(Position position)
	{
		if (_VisitedStates.Contains(position))
		{
			return true;
		}
		else
		{
			_VisitedStates.Add(position);
			return false;
		}
	}

	protected override void ExecuteDay(byte[] input)
	{
		number = Convert.ToInt32(input.GetString());

		var initialPosition = new Position { x = 1, y = 1, Steps = 0 };
		var path = new HashSet<Position>();

		var queue = new Queue<Position>();
		queue.Enqueue(initialPosition);

		while (queue.Count != 0)
		{
			var position = queue.Dequeue();

			foreach (var p in GetNextPositions(position)
				.Where(p => !IsWall(p.x, p.y))
				.Where(p => !HasVisitedPosition(p)))
			{
				if (p.x == destination.x && p.y == destination.y)
				{
					Dump('A', p.Steps);
					queue.Clear();
					_VisitedStates.Clear();
					break;
				}

				queue.Enqueue(p);
			}
		}

		queue.Enqueue(initialPosition);
		var count = -1;
		while (queue.Peek().Steps <= 50)
		{
			var position = queue.Dequeue();
			count++;

			foreach (var p in GetNextPositions(position)
				.Where(p => !IsWall(p.x, p.y))
				.Where(p => !HasVisitedPosition(p)))
			{
				queue.Enqueue(p);
			}
		}

		Dump('B', count);
	}

	IEnumerable<Position> GetNextPositions(Position p)
	{
		if (p.x > 0) yield return new Position { x = p.x - 1, y = p.y, Steps = p.Steps + 1 };
		if (p.y > 0) yield return new Position { x = p.x, y = p.y - 1, Steps = p.Steps + 1 };
		yield return new Position { x = p.x + 1, y = p.y, Steps = p.Steps + 1 };
		yield return new Position { x = p.x, y = p.y + 1, Steps = p.Steps + 1 };
	}
}
