<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day01.input.txt"))
	.Select(x => (int)x - (int)'0')
	.ToList();

(input.Zip(input.Skip(1), (a, b) => new { a, b })
	.Where(x => x.a == x.b)
	.Select(x => x.a)
	.Sum() + input.Last())
	.Dump("Part A");
	
var rotInput = input.Skip(input.Count / 2).Concat(input.Take(input.Count / 2));
input.Zip(rotInput, (a, b) => new { a, b })
	.Where(x => x.a == x.b)
	.Select(x => x.a)
	.Sum()
	.Dump("Part B");