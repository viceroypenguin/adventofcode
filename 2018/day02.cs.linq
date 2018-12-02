<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var ids = File
	.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day02.input.txt"));

var counts = ids
	.Select(id =>
	{
		var chars = id
			.GroupBy(c => c)
			.Select(x => x.Count())
			.ToList();
		return (two: chars.Any(x => x == 2), three: chars.Any(x => x == 3));
	})
	.Aggregate((twos: 0, threes: 0), (acc, next) => 
		(acc.twos + (next.two ? 1 : 0), acc.threes + (next.three ? 1 : 0)));
		
(counts.twos * counts.threes)
	.Dump("Part A");
	
new string(
	ids
		.OrderBy(x => x)
		.Window(2)
		.Select(pair => (
			pair,
			letters: pair[0].Zip(pair[1], (l, r) => (l, r))))
		.Where(x => x.letters.Count(y => y.l != y.r) == 1)
		.SelectMany(x => x.letters.Where(y => y.l == y.r))
		.Select(x => x.l)
		.ToArray())
	.Dump("Part B");