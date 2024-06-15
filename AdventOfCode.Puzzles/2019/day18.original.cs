namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 18, CodeType.Original)]
public class Day_18_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		return (
			DoPartA(map),
			DoPartB(map));
	}

	private string DoPartA(byte[][] map)
	{
		var importantItems = BuildDistanceCache(map);

		var allKeys =
			importantItems
				.Select(kvp => kvp.Key)
				.Where(k => k != '@')
				.Aggregate(0UL, (a, i) => a | (1UL << (i - (byte)'a')));

		var comparer = ProjectionEqualityComparer.Create<(ulong keys, byte pos)>(
			(a, b) => (a.keys == allKeys && b.keys == allKeys)
				|| EqualityComparer<(ulong, byte)>.Default.Equals(a, b));
		var distance = SuperEnumerable.GetShortestPathCost<(ulong keys, byte pos), int>(
			(keys: 0UL, pos: (byte)'@'),
			GetNeighbors,
			(allKeys, pos: 0),
			stateComparer: comparer,
			costComparer: null);

		IEnumerable<((ulong, byte), int)> GetNeighbors((ulong keys, byte pos) state, int cost)
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
					cost + steps);
			}
		}

		return distance.ToString();
	}

	private static Dictionary<byte, List<(byte key, int steps, ulong requiredKeys)>> BuildDistanceCache(byte[][] map) =>
		map
			.SelectMany((r, y) => r
				.Select((c, x) => (y, x, c)))
			.Where(p => p.c is
				(>= (byte)'a' and <= (byte)'z') or (byte)'@'
				or (byte)'$'
				or (byte)'%'
				or (byte)'&'
				or (byte)'\'')
			.ToDictionary(
				p => p.c,
				p =>
				{
					var visited = new HashSet<(int x, int y)>();
					var destinations = new List<(byte key, int steps, ulong requiredKeys)>();
					SuperEnumerable.TraverseBreadthFirst(
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

							var newKeys = (type is >= (byte)'A' and <= (byte)'Z')
								? state.requiredKeys | (1UL << (type - 'A'))
								: state.requiredKeys;

							return
							[
								((x + 1, y), type, newKeys, state.steps + 1),
								((x - 1, y), type, newKeys, state.steps + 1),
								((x, y + 1), type, newKeys, state.steps + 1),
								((x, y - 1), type, newKeys, state.steps + 1),
							];
						})
						.Consume();

					return destinations;
				});

	private string DoPartB(byte[][] map)
	{
		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] == (byte)'@')
				{
					map[y][x] = (byte)'#';
					map[y - 1][x] = (byte)'#';
					map[y + 1][x] = (byte)'#';
					map[y][x - 1] = (byte)'#';
					map[y][x + 1] = (byte)'#';
					map[y + 1][x + 1] = (byte)'$';
					map[y - 1][x + 1] = (byte)'%';
					map[y + 1][x - 1] = (byte)'&';
					map[y - 1][x - 1] = (byte)'\'';
					goto @out;
				}
			}
		}

@out:
		var importantItems = BuildDistanceCache(map);

		var allKeys =
			importantItems
				.Select(kvp => kvp.Key)
				.Where(k => k >= 'a')
				.Aggregate(0UL, (a, i) => a | (1UL << (i - (byte)'a')));

		var comparer = ProjectionEqualityComparer.Create<(ulong keys, uint pos)>(
			(a, b) => (a.keys == allKeys && b.keys == allKeys)
				|| EqualityComparer<(ulong, uint)>.Default.Equals(a, b));
		var distance = SuperEnumerable.GetShortestPathCost<(ulong keys, uint pos), int>(
			(keys: 0UL, pos: 0x24_25_26_27u),
			GetNeighbors,
			(allKeys, 0),
			stateComparer: comparer,
			costComparer: null);

		IEnumerable<((ulong, uint), int)> GetNeighbors((ulong keys, uint pos) state, int cost)
		{
			var positions = BitConverter.GetBytes(state.pos);
			for (var i = 0; i < positions.Length; i++)
			{
				var basePos = positions[i];
				foreach (var (_key, steps, requiredKeys) in importantItems[basePos])
				{
					var key = 1UL << (_key - (byte)'a');
					if ((state.keys & key) != 0)
						continue;
					if (~(~requiredKeys | state.keys) != 0)
						continue;

					positions[i] = _key;
					yield return (
						(state.keys | key, BitConverter.ToUInt32(positions)),
						cost + steps);
				}

				positions[i] = basePos;
			}
		}

		return distance.ToString();
	}
}
