<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var regex = new Regex(@"^#(?<id>\d+)\s+@\s+(?<el>\d+),(?<et>\d+): (?<wide>\d+)x(?<tall>\d+)$", RegexOptions.Compiled);

List<int> dGetOrAdd(Dictionary<(int x, int y), List<int>> d, (int x, int y) key)
{
	if (d.TryGetValue(key, out var l))
		return l;
	return d[key] = new List<int>();
}

var claims = File
	.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day03.input.txt"))
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

fabric
	.Count(kvp => kvp.Value.Count > 1)
	.Dump("Part A");

var claimsCountById = claims
	.ToDictionary(
		g => g.id,
		g => g.wide * g.tall);
		
var soloFabrics = fabric
	.Where(kvp => kvp.Value.Count == 1)
	.GroupBy(kvp => kvp.Value[0])
	.Select(g => (id: g.Key, count: g.Count()))
	.ToList();
		
soloFabrics
	.Single(f => f.count == claimsCountById[f.id])
	.id
	.Dump("Part B");
