namespace AdventOfCode;

public class Day_2019_18_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 18;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input
			.Batch(input.Index().First(kvp => kvp.Value == '\n').Key + 1)
			.Select(arr => arr.ToArray())
			.ToArray();
		PartA = DoPartA(map).ToString();
	}

	private int DoPartA(byte[][] map)
	{
		var importantItems = BuildDistanceCache(map);

		var allKeys =
			importantItems
				.Select(kvp => kvp.Key)
				.Where(k => k != '@')
				.Aggregate(0UL, (a, i) => a | (1UL << (i - (byte)'a')));

		var (_, _, distance) = Helpers.Dijkstra(
			(keys: 0UL, pos: (byte)'@'),
			getNeighbors,
			(d, s) => s.keys == allKeys);

		IEnumerable<((ulong, byte), int)> getNeighbors((ulong keys, byte pos) state)
		{
			foreach (var (_key, steps, requiredKeys) in importantItems[state.pos])
			{
				var key = 1UL << (_key - (byte)'a');
				if ((state.keys & key) != 0)
					continue;
				if (~(~requiredKeys | state.keys) != 0)
					continue;

				yield return (
					(state.keys | key, _key),
					steps);
			}
		}

		return distance;
	}

	private static Dictionary<byte, List<(byte key, int steps, ulong requiredKeys)>> BuildDistanceCache(byte[][] map) =>
		map
			.SelectMany((r, y) => r
				.Select((c, x) => (y, x, c)))
			.Where(p => (p.c >= 'a' && p.c <= 'z') || p.c == '@')
			.ToDictionary(
				p => p.c,
				p =>
				{
					var visited = new HashSet<(int x, int y)>();
					var destinations = new List<(byte key, int steps, ulong requiredKeys)>();
					MoreEnumerable.TraverseBreadthFirst(
						(pos: (p.x, p.y), type: p.c, requiredKeys: 0UL, steps: 0),
						state =>
						{
							var @null = Array.Empty<((int x, int y) pos, byte type, ulong requiredKeys, int steps)>();
							if (visited.Contains(state.pos))
								return @null;
							visited.Add(state.pos);

							var (x, y) = state.pos;
							var type = map[y][x];
							if (type == '#')
								return @null;

							if (type >= 'a' && type <= 'z' && type != p.c)
							{
								destinations.Add(
									(type, state.steps, state.requiredKeys));
							}

							var newKeys = (type >= 'A' && type <= 'Z')
								? state.requiredKeys | (1UL << (type - 'A'))
								: state.requiredKeys;

							return new[]
							{
								((x + 1, y), type, newKeys, state.steps + 1),
								((x - 1, y), type, newKeys, state.steps + 1),
								((x, y + 1), type, newKeys, state.steps + 1),
								((x, y - 1), type, newKeys, state.steps + 1),
							};
						})
						.Consume();

					return destinations;
				});

	private int DoPartB(byte[][] map)
	{
		return 0;
	}
}
