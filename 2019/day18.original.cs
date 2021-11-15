using Medallion.Collections;

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

	static DjikstraState allKeys;

	class DjikstraState : IEquatable<DjikstraState>, IComparable<DjikstraState>
	{
		public DjikstraState(IEnumerable<byte> keys, byte currentKey, int steps)
		{
			if (currentKey != '@' && currentKey != 0)
				keys = MoreEnumerable.Append(keys, currentKey);
			CollectedKeys = keys.OrderBy(k => k).ToArray();
			CurrentKey = currentKey;
			Steps = steps;

			var code = 0;
			foreach (var k in CollectedKeys)
				code = HashCode.Combine(code, k);

			Complete = currentKey == 0 || code == allKeys.GetHashCode();
			if (!Complete)
				code = HashCode.Combine(code, currentKey);

			this.hashCode = code;
		}

		public bool Complete { get; }
		public IReadOnlyList<byte> CollectedKeys { get; }
		public byte CurrentKey { get; }
		public int Steps { get; }

		private readonly int hashCode;

		public override int GetHashCode() => hashCode;

		public bool Equals(DjikstraState other) =>
			hashCode == other.hashCode
			&& CollectedKeys.Count == other.CollectedKeys.Count
			&& CollectedKeys
				.Zip(other.CollectedKeys, (a, b) => a == b)
				.All(b => b);

		public int CompareTo(DjikstraState other) =>
			Steps.CompareTo(other.Steps);
	}

	private int DoPartA(byte[][] map)
	{
		var importantItems = BuildDistanceCache(map);

		allKeys = new DjikstraState(
			importantItems
				.Select(kvp => kvp.Key)
				.Where(k => k != '@')
				.ToArray(),
			0,
			0);

		var best = new Dictionary<DjikstraState, int>();
		var queue = new PriorityQueue<DjikstraState>();
		queue.Enqueue(new DjikstraState(Array.Empty<byte>(), (byte)'@', 0));

		while (queue.Any())
		{
			var state = queue.Dequeue();

			if (best.GetValueOrDefault(state, int.MaxValue) < state.Steps)
				continue;

			foreach (var (key, steps, requiredKeys) in importantItems[state.CurrentKey])
			{
				if (state.CollectedKeys.Contains(key))
					continue;
				if (requiredKeys.Except(state.CollectedKeys).Any())
					continue;

				var newDjState = new DjikstraState(
					state.CollectedKeys,
					key,
					state.Steps + steps);
				if (newDjState.Steps < best.GetValueOrDefault(newDjState, int.MaxValue))
				{
					best[newDjState] = newDjState.Steps;
					queue.Enqueue(newDjState);
				}
			}
		}

		return best[allKeys];
	}

	private static Dictionary<byte, List<(byte key, int steps, byte[] requiredKeys)>> BuildDistanceCache(byte[][] map) =>
		map
			.SelectMany((r, y) => r
				.Select((c, x) => (y, x, c)))
			.Where(p => (p.c >= 'a' && p.c <= 'z') || p.c == '@')
			.ToDictionary(
				p => p.c,
				p =>
				{
					var visited = new HashSet<(int x, int y)>();
					var destinations = new List<(byte key, int steps, byte[] requiredKeys)>();
					MoreEnumerable.TraverseBreadthFirst(
						(pos: (p.x, p.y), type: p.c, requiredKeys: Array.Empty<byte>(), steps: 0),
						state =>
						{
							var @null = Array.Empty<((int x, int y) pos, byte type, byte[] requiredKeys, int steps)>();
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
								? MoreEnumerable.Append(state.requiredKeys, (byte)(type | 0x20)).ToArray()
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
