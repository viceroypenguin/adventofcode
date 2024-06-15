namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 03, CodeType.Original)]
public partial class Day_03_Original : IPuzzle
{
	[GeneratedRegex("^#(?<id>\\d+)\\s+@\\s+(?<el>\\d+),(?<et>\\d+): (?<wide>\\d+)x(?<tall>\\d+)$", RegexOptions.Compiled)]
	private static partial Regex ClaimsRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = ClaimsRegex();

		static List<int> GetOrAdd(Dictionary<(int x, int y), List<int>> d, (int x, int y) key) =>
			d.TryGetValue(key, out var l) ? l : (d[key] = []);

		var claims = input.Lines
			.Select(c => regex.Match(c))
			.Select(m => new
			{
				id = Convert.ToInt32(m.Groups["id"].Value),
				el = Convert.ToInt32(m.Groups["el"].Value),
				et = Convert.ToInt32(m.Groups["et"].Value),
				wide = Convert.ToInt32(m.Groups["wide"].Value),
				tall = Convert.ToInt32(m.Groups["tall"].Value),
			})
			.ToList();

		var fabric = new Dictionary<(int x, int y), List<int>>();
		foreach (var c in claims)
		{
			foreach (var x in Enumerable.Range(c.el, c.wide))
			{
				foreach (var y in Enumerable.Range(c.et, c.tall))
					GetOrAdd(fabric, (x, y)).Add(c.id);
			}
		}

		var part1 = fabric.Count(kvp => kvp.Value.Count > 1).ToString();

		var claimsCountById = claims
			.ToDictionary(
				g => g.id,
				g => g.wide * g.tall);

		var soloFabrics = fabric
			.Where(kvp => kvp.Value.Count == 1)
			.GroupBy(kvp => kvp.Value[0])
			.Select(g => (id: g.Key, count: g.Count()))
			.ToList();

		var part2 = soloFabrics
			.Single(f => f.count == claimsCountById[f.id])
			.id
			.ToString();

		return (part1, part2);
	}
}
