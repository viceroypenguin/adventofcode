<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var changes = File
	.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day01.input.txt"))
	.Select(l => Convert.ToInt32(l))
	.ToList();

changes
	.Sum()
	.Dump("Part A");
	
var seen = new HashSet<int>();
MoreEnumerable.Repeat(changes)
	.Scan((acc, next) => acc + next)
	.First(f =>
	{
		if (seen.Contains(f))
			return true;
		seen.Add(f);
		return false;
	})
	.Dump("Part B");
