﻿namespace AdventOfCode;

public class Day_2019_18_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 18;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input
			.Batch(input.Index().First(kvp => kvp.item == '\n').index + 1)
			.Select(arr => arr.ToArray())
			.ToArray();

		DoPartA(map);
		DoPartB(map);
	}

	private void DoPartA(byte[][] map)
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
			getNeighbors,
			(allKeys, pos: (byte)0),
			stateComparer: comparer,
			costComparer: null);

		IEnumerable<((ulong, byte), int)> getNeighbors((ulong keys, byte pos) state, int cost)
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

		PartA = distance.ToString();
	}

	private static Dictionary<byte, List<(byte key, int steps, ulong requiredKeys)>> BuildDistanceCache(byte[][] map) =>
		map
			.SelectMany((r, y) => r
				.Select((c, x) => (y, x, c)))
			.Where(p => 
				(p.c >= 'a' && p.c <= 'z') 
				|| p.c == '@'
				|| p.c == '$'
				|| p.c == '%'
				|| p.c == '&'
				|| p.c == '\'')
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

	private void DoPartB(byte[][] map)
	{
		for (int y = 0; y < map.Length; y++)
			for (int x = 0; x < map[y].Length; x++)
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
			getNeighbors,
			(allKeys, 0),
			stateComparer: comparer,
			costComparer: null);

		IEnumerable<((ulong, uint), int)> getNeighbors((ulong keys, uint pos) state, int cost)
		{
			var positions = BitConverter.GetBytes(state.pos);
			for (int i = 0; i < positions.Length; i++)
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

		PartB = distance.ToString();
	}
}
