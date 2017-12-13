<Query Kind="Statements" />

var regex = new Regex(@"^(?<depth>\d+): (?<range>\d+)$", RegexOptions.Compiled);
var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day13.input.txt"))
	.Select(l => regex.Match(l))
	.Select(m => new
	{
		depth = Convert.ToInt32(m.Groups["depth"].Value),
		range = Convert.ToInt32(m.Groups["range"].Value),
	})
	.ToList();

input
	.Where(f => (f.depth % ((f.range - 1) * 2)) == 0)
	.Select(f => f.depth * f.range)
	.Sum()
	.Dump("Part A");

Enumerable.Range(0, int.MaxValue)
	.Select(i =>
	{
		var any = input
			.Where(f => ((f.depth + i) % ((f.range - 1) * 2)) == 0)
			.Any();
		return new { i, any, };

	})
	.Where(x => !x.any)
	.First()
	.i
	.Dump("Part B");
