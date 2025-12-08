namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 08, CodeType.Original)]
public partial class Day_08_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var coordinates = input.Lines
			.Select(l => CoordinateRegex.Match(l))
			.Select(m => (
				x: int.Parse(m.Groups["x"].Value),
				y: int.Parse(m.Groups["y"].Value),
				z: int.Parse(m.Groups["z"].Value)
			))
			.ToList();

		var distances = coordinates
			.SelectMany(
				a => coordinates,
				(a, b) => (a, b, dist: Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2)))
			)
			.Where(x => x.dist > 0)
			.OrderBy(x => x.dist)
			.TakeEvery(2)
			.ToList();

		var circuits = new List<HashSet<(int x, int y, int z)>>();
		var count = 0;
		var part1 = 0L;
		var part2 = 0L;
		foreach (var (a, b, dist) in distances)
		{
			var aCir = circuits.FirstOrDefault(c => c.Contains(a));
			var bCir = circuits.FirstOrDefault(c => c.Contains(b));

			switch (aCir, bCir)
			{
				case (null, null):
					circuits.Add([a, b]);
					break;

				case ({ }, null):
					aCir.Add(b);
					break;

				case (null, { }):
					bCir.Add(a);
					break;

				case ({ }, { }):
					if (!ReferenceEquals(aCir, bCir))
					{
						circuits.Remove(bCir);
						aCir.UnionWith(bCir);
					}
					break;
			}

			if (++count == 1000)
			{
				part1 = circuits
					.OrderByDescending(x => x.Count)
					.Take(3)
					.Aggregate(1L, (a, b) => a * b.Count);
			}

			if (circuits[0].Count == 1000)
			{
				part2 = (long)a.x * b.x;
				break;
			}
		}

		return (part1.ToString(), part2.ToString());
	}

	[GeneratedRegex(@"^(?<x>\d+),(?<y>\d+),(?<z>\d+)$")]
	private static partial Regex CoordinateRegex { get; }
}
