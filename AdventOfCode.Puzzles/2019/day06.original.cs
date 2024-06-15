namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 06, CodeType.Original)]
public class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var directOrbits = input.Lines
			.Select(s => s.Split(')'))
			.Select(s => (orbiter: s[1], orbited: s[0]))
			.ToList();

		var lookup1 = directOrbits.ToDictionary(s => s.orbiter);
		var lookup2 = directOrbits.ToLookup(s => s.orbited);

		var part1 = directOrbits
			.Select(s =>
			{
				var count = 1;
				while (true)
				{
					if (!lookup1.TryGetValue(s.orbited, out var o))
						return count;
					count++;
					s = o;
				}
			})
			.Sum()
			.ToString();

		var visited = new Dictionary<string, int>();
		var (_, steps) = SuperEnumerable.TraverseBreadthFirst(
				(orbiter: lookup1["YOU"].orbited, steps: -1),
				o =>
				{
					if (visited.ContainsKey(o.orbiter))
						return [];

					visited[o.orbiter] = o.steps;

					var tmp = lookup2[o.orbiter]
						.Select(r => (r.orbiter, o.steps + 1));
					if (lookup1.TryGetValue(o.orbiter, out var x))
						tmp = tmp.Append((x.orbited, o.steps + 1));
					return tmp;
				})
			.FirstOrDefault(x => x.orbiter == "SAN");
		var part2 = steps.ToString();
		return (part1, part2);
	}
}
