<Query Kind="Statements" />

var regex = new Regex(@"^(?<prog_a>\w+) \<-\> ((?<prog_b>\w+)(,\s*)?)*$", RegexOptions.Compiled);
var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day12.input.txt"))
	.Select(l => regex.Match(l))
	.Select(m => new int[] { Convert.ToInt32(m.Groups["prog_a"].Value), }
		.Concat(m.Groups["prog_b"].Captures.OfType<Capture>().Select(c => Convert.ToInt32(c.Value)))
		.ToList())
	.ToList();
	
var groups = new List<HashSet<int>>();
foreach (var m in input)
{
	var existingL = groups.Where(g => m.Any(id => g.Contains(id))).ToList();
	if (existingL.Count > 1)
	{
		var g = existingL[0];
		foreach (var g2 in existingL.Skip(1))
		{
			g.UnionWith(g2);
			groups.Remove(g2);
		}
		existingL = new List<HashSet<int>>() { g };
	}
	
	var existing = existingL.SingleOrDefault();
	if (existing != null)
		existing.UnionWith(m);
	else
		groups.Add(new HashSet<int>(m));
}

groups
	.Where(g => g.Contains(0))
	.Single()
	.Count
	.Dump("Part A");
groups
	.Count
	.Dump("Part B");