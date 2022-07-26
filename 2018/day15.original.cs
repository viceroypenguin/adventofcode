namespace AdventOfCode;

public class Day_2018_15_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 15;
	public override CodeType CodeType => CodeType.Original;

	static string[] map;
	static Square[][] space;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		map = input.GetLines();
		Dump('A', RunGame(true, 3));

		for (int i = 4; ; i++)
		{
			var value = RunGame(false, i);
			if (value > 0)
			{
				Dump('B', value);
				return;
			}
		}
	}

	private int RunGame(bool allowElfDeath, int elfStrength)
	{
		space = map
			.Select((s, y) => s
				.Select((c, x) =>
					c == '#' ? new Wall() :
					c == 'E' ? new Unit(UnitType.Elf, elfStrength, (x, y)) :
					c == 'G' ? new Unit(UnitType.Goblin, 3, (x, y)) :
					new Square())
				.ToArray())
			.ToArray();

		var elves = space.SelectMany(r => r)
			.OfType<Unit>()
			.Where(u => u.UnitType == UnitType.Elf)
			.ToList();

		var rounds = 0;
		while (
			space.SelectMany(r => r)
				.OfType<Unit>()
				.OrderBy(u => u.Location.y)
				.ThenBy(u => u.Location.x)
				.ToList()
				.All(u => u.DoTurn()))
		{
			if (!allowElfDeath && elves.Any(e => !e.IsAlive))
				return -1;

			rounds++;
		}

		var totalHitPoints = space.SelectMany(r => r)
			.OfType<Unit>()
			.Where(u => u.IsAlive)
			.Sum(u => u.HitPoints);

		return (rounds * totalHitPoints);
	}

	class Square { public override string ToString() => "."; }

	class Wall : Square { public override string ToString() => "#"; }

	enum UnitType
	{
		Elf, Goblin,
	}
	class Unit : Square
	{
		public Unit(UnitType type, int attackStrength, (int x, int y) location)
		{
			UnitType = type;
			Location = location;
			AttackStrength = attackStrength;
		}

		public UnitType UnitType { get; }
		public int HitPoints { get; private set; } = 200;
		public int AttackStrength { get; }
		public bool IsAlive => HitPoints > 0;
		public (int x, int y) Location { get; private set; }

		public void ReceiveAttack(int attackStrength) =>
			HitPoints -= attackStrength;

		public override string ToString() =>
			!IsAlive ? "." :
			UnitType == UnitType.Elf ? "E" : "G";

		public bool DoTurn()
		{
			if (!IsAlive)
				return true;

			var moved = DoMove();
			if (!moved)
			{
				return space
					.SelectMany(r => r)
					.OfType<Unit>()
					.Where(u => u.UnitType != this.UnitType)
					.Where(u => u.IsAlive)
					.Any();
			}

			DoAttack();
			return true;
		}

		private bool DoMove()
		{
			var dir = GetDirection();
			if (dir == null)
				return false;

			if (space[dir.Value.y][dir.Value.x] is Unit u &&
					u.IsAlive)
				return true;

			space[Location.y][Location.x] = new Square();
			Location = dir.Value;
			space[Location.y][Location.x] = this;
			return true;
		}

		private (int x, int y)? GetDirection()
		{
			var queue = new Queue<((int x, int y) dir, (int x, int y) loc)>();
			queue.Enqueue(((Location.x, Location.y - 1), (Location.x, Location.y - 1)));
			queue.Enqueue(((Location.x - 1, Location.y), (Location.x - 1, Location.y)));
			queue.Enqueue(((Location.x + 1, Location.y), (Location.x + 1, Location.y)));
			queue.Enqueue(((Location.x, Location.y + 1), (Location.x, Location.y + 1)));

			var visited = new HashSet<(int x, int y)> { Location };

			while (queue.Count > 0)
			{
				var l = queue.Dequeue();
				if (visited.Contains(l.loc))
					continue;
				visited.Add(l.loc);

				var s = space[l.loc.y][l.loc.x];
				if (s is Wall) continue;
				if (s is Unit u && u.IsAlive)
				{
					if (u.UnitType == this.UnitType)
						continue;
					else
						return l.dir;
				}

				queue.Enqueue((l.dir, loc: (l.loc.x, l.loc.y - 1)));
				queue.Enqueue((l.dir, loc: (l.loc.x - 1, l.loc.y)));
				queue.Enqueue((l.dir, loc: (l.loc.x + 1, l.loc.y)));
				queue.Enqueue((l.dir, loc: (l.loc.x, l.loc.y + 1)));
			}

			return null;
		}

		private void DoAttack()
		{
			var attackUnit =
				new (int x, int y)[]
				{
						(Location.x, Location.y - 1),
						(Location.x - 1, Location.y),
						(Location.x + 1, Location.y),
						(Location.x, Location.y + 1),
				}
				.Select(l => space[l.y][l.x])
				.OfType<Unit>()
				.Where(u => u.UnitType != this.UnitType)
				.Where(u => u.IsAlive)
				.OrderBy(u => u.HitPoints)
				.ThenBy(u => u.Location.y)
				.ThenBy(u => u.Location.x)
				.FirstOrDefault();

			if (attackUnit != null)
				attackUnit.ReceiveAttack(this.AttackStrength);
		}
	}
}
