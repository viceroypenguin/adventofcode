<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day04.input.txt"))
	.Select(x => x.Split())
	.ToList();

input
	.Where(l => l.Distinct().Count() == l.Count())
	.Count()
	.Dump("Part A");

input
	.Where(l => l.Select(s => new string(s.OrderBy(c => c).ToArray())).Distinct().Count() == l.Count())
	.Count()
	.Dump("Part B");
