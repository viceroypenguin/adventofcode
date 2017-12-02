<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day02.input.txt"))
	.Select(x => x.Split().Select(s => Convert.ToInt32(s)).ToList())
	.ToList();

input
	.Select(x => x.Max() - x.Min())
	.Sum()
	.Dump("Part A");
	
input
	.Select(arr =>
	{
		return (
			from num in arr
			from div in arr
			where num != div
			where (num / div) * div == num
			select num / div).Single();
	})
	.Sum()
	.Dump("Part B");
