<Query Kind="Statements" />

var start = 20151125ul;

Func<ulong, ulong> step = x => (x * 252533) % 33554393;

var row = 2947;
var col = 3029;

Func<int, int> totalNums = n => n * (n - 1) / 2;

var stepCount = totalNums(row + col) - (row - 1);

var num = start;
foreach (var x in Enumerable.Range(0, stepCount - 1))
	num = step(num);

num.Dump();
