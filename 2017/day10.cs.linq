<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day10.input.txt"))
	.Split(',')
	.Select(i => Convert.ToInt32(i))
	.ToList();

var listCount = 256;
var list = Enumerable.Range(0, listCount).ToArray();

void KnotHashRound()
{	
	var position = 0;
	foreach (var i in input.Select((len, skip) => (len, skip)))
	{
		var indexes = Enumerable.Range(position, i.len)
			.Select(idx => idx % listCount);
		
		var rev = indexes.Select(idx => list[idx]).Reverse().ToList();
		foreach (var x in indexes.Zip(rev, (idx, val) => (idx, val)))
		{
			list[x.idx] = x.val;
		}
		
		position = (position + i.len + i.skip) % listCount;
	}
}

KnotHashRound();
(list[0] * list[1]).Dump("Part A");

list = Enumerable.Range(0, listCount).ToArray();
input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day10.input.txt"))
	.Trim()
	.ToCharArray()
	.Select(c => (int)c)
	.Concat(new[]{ 17, 31, 73, 47, 23, })
	.ToList();

input = Enumerable.Repeat(input, 64)
	.SelectMany(i => i)
	.ToList();
	
KnotHashRound();

string.Join(
	"",
	list.Select((val, idx) => new { val, g = idx / 16, })
		.GroupBy(x => x.g)
		.Select(x => x.Aggregate(0, (a, v) => a ^ v.val))
		.Select(x => x.ToString("X2").ToLower()))
	.Dump("Part B");
	
