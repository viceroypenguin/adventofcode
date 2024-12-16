namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 16, CodeType.Original)]
public partial class Day_16_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();
		var start = map.GetMapPoints().First(p => p.item == 'S').p;
		var end = map.GetMapPoints().First(p => p.item == 'E').p;

		IEnumerable<(((int x, int y) p, int d) state, int cost)> GetNeighbors(
			((int x, int y) p, int d) state,
			int cost
		) => (state.d switch
		{
			0 => new[]
			{
				(state: (p: (x: state.p.x, y: state.p.y - 1), 0), cost + 1),
				(state: (p: (x: state.p.x + 1, y: state.p.y), 1), cost + 1_001),
				(state: (p: (x: state.p.x - 1, y: state.p.y), 3), cost + 1_001),
			},

			1 => new[]
			{
				(state: (p: (x: state.p.x, y: state.p.y - 1), 0), cost + 1_001),
				(state: (p: (x: state.p.x + 1, y: state.p.y), 1), cost + 1),
				(state: (p: (x: state.p.x, y: state.p.y + 1), 2), cost + 1_001),
			},

			2 => new[]
			{
				(state: (p: (x: state.p.x + 1, y: state.p.y), 1), cost + 1_001),
				(state: (p: (x: state.p.x, y: state.p.y + 1), 2), cost + 1),
				(state: (p: (x: state.p.x - 1, y: state.p.y), 3), cost + 1_001),
			},

			3 or _ => new[]
			{
				(state: (p: (x: state.p.x, y: state.p.y - 1), 0), cost + 1_001),
				(state: (p: (x: state.p.x, y: state.p.y + 1), 2), cost + 1_001),
				(state: (p: (x: state.p.x - 1, y: state.p.y), 3), cost + 1),
			},
		})
			.Where(q => q.state.p.IsValid(map)
				&& map[q.state.p.y][q.state.p.x] != '#');

		var paths = SuperEnumerable.GetShortestPaths<
			((int x, int y) p, int d),
			int
		>(
			(p: start, d: 1),
			GetNeighbors
		);

		var endState = paths.First(p => p.Key.p == end);

		var part1 = endState.Value.cost;

		var queue = new Queue<(((int x, int y) p, int d) previousState, int cost)>([endState.Value]);
		var part2 = new HashSet<(int x, int y)>();

		while (queue.TryDequeue(out var q))
		{
			if (!part2.Add(q.previousState.p))
				continue;

			if (q.previousState == default)
				break;

			map[q.previousState.p.y][q.previousState.p.x] = (byte)'O';

			var prev = paths[q.previousState];
			queue.Enqueue(prev);

			var otherPaths = paths
				.Where(
					p =>
						p.Key.p == q.previousState.p
						&& p.Value.cost + 1 == q.cost
				)
				.ToList();

			foreach (var (key, value) in otherPaths)
				queue.Enqueue(value);
		}

		return (part1.ToString(), part2.Count.ToString());
	}
}
