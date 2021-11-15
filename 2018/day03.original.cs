namespace AdventOfCode;

public class Day_2018_03_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 3;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var regex = new Regex(@"^#(?<id>\d+)\s+@\s+(?<el>\d+),(?<et>\d+): (?<wide>\d+)x(?<tall>\d+)$", RegexOptions.Compiled);

		List<int> dGetOrAdd(Dictionary<(int x, int y), List<int>> d, (int x, int y) key)
		{
			if (d.TryGetValue(key, out var l))
				return l;
			return d[key] = new List<int>();
		}

		var claims = input.GetLines()
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
			foreach (var x in Enumerable.Range(c.el, c.wide))
				foreach (var y in Enumerable.Range(c.et, c.tall))
					dGetOrAdd(fabric, (x, y)).Add(c.id);

		Dump('A', fabric.Count(kvp => kvp.Value.Count > 1));

		var claimsCountById = claims
			.ToDictionary(
				g => g.id,
				g => g.wide * g.tall);

		var soloFabrics = fabric
			.Where(kvp => kvp.Value.Count == 1)
			.GroupBy(kvp => kvp.Value[0])
			.Select(g => (id: g.Key, count: g.Count()))
			.ToList();

		Dump('B',
			soloFabrics
				.Single(f => f.count == claimsCountById[f.id])
				.id);
	}
}
