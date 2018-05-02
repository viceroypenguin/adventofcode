<Query Kind="Program">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

class Component
{
	public int PortA { get; set; }
	public int PortB { get; set; }
}

void Main()
{
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day24.input.txt"))
		.Select(x => x.Split('/'))
		.Select(x => new Component { PortA = Convert.ToInt32(x[0]), PortB = Convert.ToInt32(x[1]), })
		.ToList();
	
	var map = 
		input.Select(x => new { i = x.PortA, x, })
			.Concat(input
				.Where(x => x.PortA != x.PortB)
				.Select(x => new { i = x.PortB, x, }))
			.ToLookup(
				x => x.i,
				x => x.x);
	
	var strength = CalculateStrength(map, ImmutableList<Component>.Empty, 0, (0, 0, 0));
			
	strength.maxStrength.Dump("Part A");
	strength.maxLongestPath.Dump("Part B");
}

(int maxStrength, int longestPath, int maxLongestPath)
	CalculateStrength(ILookup<int, Component> map, ImmutableList<Component> path, int openConnection, (int maxStrength, int longestPath, int maxLongestPath) x)
{
	var list = map[openConnection]
		.Where(c => !path.Contains(c))
		.Select(c => CalculateStrength(
			map, 
			path.Add(c), 
			openConnection == c.PortA ? c.PortB : c.PortA, 
			(maxStrength: x.maxStrength + c.PortA + c.PortB, x.longestPath, x.maxLongestPath)))
		.ToList();
		
	if (list.Any())
		return (
			list.Max(y => y.maxStrength), 
			list.Max(y => y.longestPath), 
			list.OrderByDescending(y => y.longestPath).ThenByDescending(y => y.maxLongestPath).First().maxLongestPath);
		
	return (x.maxStrength, path.Count, x.maxStrength);
}
