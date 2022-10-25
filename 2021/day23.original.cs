namespace AdventOfCode;

public class Day_2021_23_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 23;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input.GetMap();
		var tokens = Enumerable.Range(0, map.Length)
			.SelectMany(y => Enumerable.Range(0, map[y].Length)
				.Select(x => (x, y, token: map[y][x]))
				.Where(t => t.token >= 'A'))
			.ToDictionary(t => (t.x, t.y), t => t.token);

		var wellDepth = map.Length - 2;
		var holdingSpots = new (int x, int y)[]
		{
			(1, 1), (2, 1), (4, 1), (6, 1), (8, 1), (10, 1), (11, 1),
		};

		static Dictionary<(int, int), byte> MoveToken(Dictionary<(int, int), byte> tokens, (int x, int y) from, (int x, int y) to)
		{
			var newTokens = new Dictionary<(int, int), byte>(tokens)
			{
				[to] = tokens[from],
			};
			newTokens.Remove(from);
			return newTokens;
		}

		IEnumerable<(Dictionary<(int x, int y), byte>, int)> MoveTokens(Dictionary<(int x, int y), byte> tokens, int cost)
		{
			// for each token, where can we go?
			var positions = tokens.SelectMany(_ =>
			{
				var ((x, y), t) = _;

				// how much does it cost per tile?
				// and where are we trying to get to?
				var (tileEnergy, destColumn) =
					t switch
					{
						(byte)'A' => (1, 3),
						(byte)'B' => (10, 5),
						(byte)'C' => (100, 7),
						(byte)'D' => (1000, 9),
						_ => default,
					};

				// are we in our final position?
				if (x == destColumn && !Enumerable.Range(y + 1, wellDepth - y).Any(y => tokens.TryGetValue((x, y), out var tmp) && tmp != t))
					// no need to process further
					return Array.Empty<(Dictionary<(int, int), byte>, int)>();

				// is there's someone between us and main hallway?
				if (y > 2 && tokens.ContainsKey((x, y - 1)))
					// we can't go anywhere anyway
					return Array.Empty<(Dictionary<(int, int), byte>, int)>();

				// can we get to the destination column from here?
				if (!holdingSpots
						.Where(p => x != p.x && p.x.Between(x, destColumn))
						.Any(p => tokens.ContainsKey(p)))
				{
					// check destination column from bottom up
					for (int i = wellDepth; i >= 2; i--)
						if (tokens.TryGetValue((destColumn, i), out var tmp))
						{
							// somebody else where we need to go?
							if (tmp != t)
								// not allowed to move into column
								break;
						}
						else
						{
							// empty space
							// always cheapest solution to go direct if we can
							return new[]
							{
								(
									// move here
									MoveToken(tokens, (x, y), (destColumn, i)),
									// tiles = horizontal distance
									//       + vertical distance to 1
									//       + vertical distance to dest from 1
									// cost = tiles * tileEnergy
									cost + (Math.Abs(x - destColumn) + Math.Abs(y - 1) + (i - 1)) * tileEnergy),
							};
						}
				}

				// are we in a holding spot or a column?
				if (y == 1)
					// we're in a holding spot
					// no path to destination
					return Array.Empty<(Dictionary<(int, int), byte>, int)>();

				// which holding spots can we access?
				return holdingSpots
					.Where(d => !holdingSpots
						.Where(p => p.x.Between(x, d.x))
						.Any(p => tokens.ContainsKey(p)))
					// we know where we can go
					.Select(q => (
						// move here
						MoveToken(tokens, (x, y), q),
						// tiles = horizontal distance
						//       + vertical distance to 1
						//       + vertical distance to dest from 1
						// cost = tiles * tileEnergy
						cost + (Math.Abs(x - q.x) + Math.Abs(y - 1) + (q.y - 1)) * tileEnergy));
			}).ToList();
			return positions;
		}

		var energy = SuperEnumerable.GetShortestPathCost<Dictionary<(int x, int y), byte>, int>(
			tokens,
			MoveTokens,
			new Dictionary<(int x, int y), byte>
			{
				[(3, 3)] = (byte)'A',
				[(3, 2)] = (byte)'A',
				[(5, 3)] = (byte)'B',
				[(5, 2)] = (byte)'B',
				[(7, 3)] = (byte)'C',
				[(7, 2)] = (byte)'C',
				[(9, 3)] = (byte)'D',
				[(9, 2)] = (byte)'D',
			},
			stateComparer: ProjectionEqualityComparer.Create<Dictionary<(int x, int y), byte>>(
				(a, b) => a.Count == b.Count && a.All(x => b.GetValueOrDefault(x.Key) == x.Value),
				a => a
					.OrderBy(kvp => kvp.Key.x)
					.ThenBy(kvp => kvp.Key.y)
					.Aggregate(0, (x, y) => HashCode.Combine(x, y.GetHashCode()))),
			costComparer: default);
		PartA = energy.ToString();

		wellDepth += 2;
		foreach (var ((x, y), t) in tokens.ToList())
			tokens[(x, y + 2)] = t;
		tokens[(3, 3)] = (byte)'D'; tokens[(3, 4)] = (byte)'D';
		tokens[(5, 3)] = (byte)'C'; tokens[(5, 4)] = (byte)'B';
		tokens[(7, 3)] = (byte)'B'; tokens[(7, 4)] = (byte)'A';
		tokens[(9, 3)] = (byte)'A'; tokens[(9, 4)] = (byte)'C';

		energy = SuperEnumerable.GetShortestPathCost<Dictionary<(int x, int y), byte>, int>(
			tokens,
			MoveTokens,
			new Dictionary<(int x, int y), byte>
			{
				[(3, 5)] = (byte)'A',
				[(3, 4)] = (byte)'A',
				[(3, 3)] = (byte)'A',
				[(3, 2)] = (byte)'A',
				[(5, 5)] = (byte)'B',
				[(5, 4)] = (byte)'B',
				[(5, 3)] = (byte)'B',
				[(5, 2)] = (byte)'B',
				[(7, 5)] = (byte)'C',
				[(7, 4)] = (byte)'C',
				[(7, 3)] = (byte)'C',
				[(7, 2)] = (byte)'C',
				[(9, 5)] = (byte)'D',
				[(9, 4)] = (byte)'D',
				[(9, 3)] = (byte)'D',
				[(9, 2)] = (byte)'D',
			},
			stateComparer: ProjectionEqualityComparer.Create<Dictionary<(int x, int y), byte>>(
				(a, b) => a.Count == b.Count && a.All(x => b.GetValueOrDefault(x.Key) == x.Value),
				a => a
					.OrderBy(kvp => kvp.Key.x)
					.ThenBy(kvp => kvp.Key.y)
					.Aggregate(0, (x, y) => HashCode.Combine(x, y.GetHashCode()))),
			costComparer: default);
		PartB = energy.ToString();
	}
}
