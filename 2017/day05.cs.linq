<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day05.input.txt"))
	.Select(x => Convert.ToInt32(x))
	.ToList();
	
var instructions = input.ToList();
var ptr = 0;
var count = 0;

while (ptr >= 0 && ptr < instructions.Count)
{
	var adjust = instructions[ptr];
	instructions[ptr] = adjust + 1;
	ptr += adjust;
	count++;
}

count.Dump("Part A");

instructions = input.ToList();
ptr = 0;
count = 0;

while (ptr >= 0 && ptr < instructions.Count)
{
	var adjust = instructions[ptr];
	instructions[ptr] = 
		adjust >= 3 
			? adjust - 1
			: adjust + 1;
	ptr += adjust;
	count++;
}

count.Dump("Part B");

