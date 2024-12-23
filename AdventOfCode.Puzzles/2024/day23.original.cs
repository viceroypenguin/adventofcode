namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 23, CodeType.Original)]
public partial class Day_23_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var connections = input.Lines
			.Select(l => l.Split('-'))
			.SelectMany(s => new[]
			{
				(s[0], s[1]),
				(s[1], s[0]),
			})
			.ToLookup(x => x.Item1, x => x.Item2);

		var part1 = DoPart1(connections);
		var part2 = DoPart2(connections);

		return (part1, part2);
	}

	private static string DoPart1(ILookup<string, string> connections)
	{
		var groups = new HashSet<(string, string, string)>();
		foreach (var node in connections)
		{
			foreach (var secondary in node)
			{
				foreach (var z in connections[secondary].Intersect(node)
					.Select(x => new[] { node.Key, secondary, x }.Order().ToList())
					.Select(x => (x[0], x[1], x[2])))
				{
					groups.Add(z);
				}
			}
		}

		return groups
			.Count(x => x.Item1.StartsWith('t') || x.Item2.StartsWith('t') || x.Item3.StartsWith('t'))
			.ToString();
	}

	private static string DoPart2(ILookup<string, string> connections)
	{
		var groups = new List<List<string>>();

		foreach (var n in connections)
		{
			var next = n.Key;
			foreach (var g in groups)
			{
				if (g.TrueForAll(node => connections[node].Contains(next)))
					g.Add(next);
			}

			groups.Add([next]);
		}

		return string.Join(
			',',
			groups
				.MaxBy(g => g.Count)!
				.Order()
		);
	}
}
