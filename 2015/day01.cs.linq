<Query Kind="Statements" />

var txt = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day01.input.txt"));

var level = 0;
var basement = 0;
foreach ((var c, var i) in txt.Select((c, i) => (c, i+1)))
{
	level += (c == '(' ? +1 : -1);
	if (basement == 0 && level == -1)
		basement = i;
}

level.Dump("Part A");
basement.Dump("Part B");
