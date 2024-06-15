namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 15, CodeType.Original)]
public class Day_15_Original : IPuzzle
{
	private static string[] s_map;
	private static Square[][] s_space;

	public (string, string) Solve(PuzzleInput input)
	{
		s_map = input.Lines;
		var part1 = RunGame(true, 3);

		for (var i = 4; ; i++)
		{
			var value = RunGame(false, i);
			if (value > 0)
			{
				var part2 = value;
				return (part1.ToString(), part2.ToString());
			}
		}
	}

	private static int RunGame(bool allowElfDeath, int elfStrength)
	{
		s_space = s_map
			.Select((s, y) => s
				.Select((c, x) =>
					c == '#' ? new Wall() :
					c == 'E' ? new Unit(UnitType.Elf, elfStrength, (x, y)) :
					c == 'G' ? new Unit(UnitType.Goblin, 3, (x, y)) :
					new Square())
				.ToArray())
			.ToArray();

		var elves = s_space.SelectMany(r => r)
			.OfType<Unit>()
			.Where(u => u.UnitType == UnitType.Elf)
			.ToList();

		var rounds = 0;
		while (
			s_space.SelectMany(r => r)
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

		var totalHitPoints = s_space.SelectMany(r => r)
			.OfType<Unit>()
			.Where(u => u.IsAlive)
			.Sum(u => u.HitPoints);

		return rounds * totalHitPoints;
	}

	private class Square { public override string ToString() => "."; }

	private sealed class Wall : Square { public override string ToString() => "#"; }

	private enum UnitType
	{
		Elf, Goblin,
	}

	private sealed class Unit(UnitType type, int attackStrength, (int x, int y) location) : Square
	{
		public UnitType UnitType { get; } = type;
		public int HitPoints { get; private set; } = 200;
		public int AttackStrength { get; } = attackStrength;
		public bool IsAlive => HitPoints > 0;
		public (int x, int y) Location { get; private set; } = location;

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
#pragma warning disable IDE0120 // Simplify LINQ expression
				return s_space
					.SelectMany(r => r)
					.OfType<Unit>()
					.Where(u => u.UnitType != UnitType)
					.Where(u => u.IsAlive)
					.Any();
#pragma warning restore IDE0120 // Simplify LINQ expression
			}

			DoAttack();
			return true;
		}

		private bool DoMove()
		{
			var dir = GetDirection();
			if (dir == null)
				return false;

			if (s_space[dir.Value.y][dir.Value.x] is Unit u &&
					u.IsAlive)
			{
				return true;
			}

			s_space[Location.y][Location.x] = new Square();
			Location = dir.Value;
			s_space[Location.y][Location.x] = this;
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
				var (dir, loc) = queue.Dequeue();
				if (visited.Contains(loc))
					continue;
				_ = visited.Add(loc);

				var s = s_space[loc.y][loc.x];
				if (s is Wall) continue;
				if (s is Unit u && u.IsAlive)
				{
					if (u.UnitType == UnitType)
						continue;
					else
						return dir;
				}

				queue.Enqueue((dir, loc: (loc.x, loc.y - 1)));
				queue.Enqueue((dir, loc: (loc.x - 1, loc.y)));
				queue.Enqueue((dir, loc: (loc.x + 1, loc.y)));
				queue.Enqueue((dir, loc: (loc.x, loc.y + 1)));
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
				.Select(l => s_space[l.y][l.x])
				.OfType<Unit>()
				.Where(u => u.UnitType != UnitType)
				.Where(u => u.IsAlive)
				.OrderBy(u => u.HitPoints)
				.ThenBy(u => u.Location.y)
				.ThenBy(u => u.Location.x)
				.FirstOrDefault();

			attackUnit?.ReceiveAttack(AttackStrength);
		}
	}
}
