namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 22, CodeType.Original)]
public partial class Day_22_Original : IPuzzle
{
	[GeneratedRegex(@"^(?<x1>\d+),(?<y1>\d+),(?<z1>\d+)~(?<x2>\d+),(?<y2>\d+),(?<z2>\d+)$")]
	private static partial Regex BrickRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var brickRegex = BrickRegex();

		var bricks = input.Lines
			.Select(l => brickRegex.Match(l))
			.Select(m => (
				x1: int.Parse(m.Groups["x1"].Value),
				y1: int.Parse(m.Groups["y1"].Value),
				z1: int.Parse(m.Groups["z1"].Value),
				x2: int.Parse(m.Groups["x2"].Value),
				y2: int.Parse(m.Groups["y2"].Value),
				z2: int.Parse(m.Groups["z2"].Value)))
			.OrderBy(b => Math.Min(b.z1, b.z2))
			.ToList();

		var reference = new Dictionary<(int x, int y, int z), int>();
		var isOnlySupport = new bool[bricks.Count];
		var supporting = new List<int>[bricks.Count];
		var supports = new HashSet<int>[bricks.Count];

		for (var i = 0; i < bricks.Count; i++)
		{
			var (x1, y1, z1, x2, y2, z2) = bricks[i];

			var xys =
				x1 != x2 ? SuperEnumerable.Sequence(x1, x2).Select(x => (x, y1)) :
				y1 != y2 ? SuperEnumerable.Sequence(y1, y2).Select(y => (x1, y)) :
				[(x1, y1)];

			supports[i] = [];
			while (z1 > 0)
			{
				foreach (var (x, y) in xys)
				{
					if (reference.TryGetValue((x, y, z1 - 1), out var idx))
						_ = supports[i].Add(idx);
				}

				if (supports[i].Count > 0)
					break;

				z1 -= 1;
				z2 -= 1;
			}

			bricks[i] = (x1, y1, z1, x2, y2, z2);
			if (supports[i].Count == 1)
				isOnlySupport[supports[i].Single()] = true;

			foreach (var s in supports[i])
				(supporting[s] ??= []).Add(i);

			foreach (var (x, y) in xys)
			{
				foreach (var z in SuperEnumerable.Sequence(z1, z2))
					reference[(x, y, z)] = i;
			}
		}

		var part1 = isOnlySupport.Count(x => !x);

		var part2 = 0;
		for (var i = 0; i < bricks.Count; i++)
		{
			if (!isOnlySupport[i])
				continue;

			var queue = new Queue<int>();
			foreach (var s in supporting[i])
				queue.Enqueue(s);

			var seen = new HashSet<int>() { i };
			while (queue.TryDequeue(out var j))
			{
				if (supports[j].Except(seen).Any())
					continue;

				if (!seen.Add(j))
					continue;

				foreach (var s in supporting[j] ?? [])
					queue.Enqueue(s);
			}
			part2 += seen.Count - 1;
		}

		return (part1.ToString(), part2.ToString());
	}
}
