<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day01.input.txt"));

(input.Zip(input.Skip(1), (a, b) => new { a, b })
	.Where(x => x.a == x.b)
	.Select(x => (int)x.a - (int)'0')
	.Sum() + ((int)input.Last() - (int)'0'))
	.Dump("Part A");
	
var rotInput = input.Skip(input.Length / 2).Concat(input.Take(input.Length / 2));
input.Zip(rotInput, (a, b) => new { a, b })
	.Where(x => x.a == x.b)
	.Select(x => (int)x.a - (int)'0')
	.Sum()
	.Dump("Part B");

