<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day03.input.txt"));

var current = (x: 0, y: 0);
var santaHouses = input
	.Select(c =>
	{
		if (c == '>') current = (current.x + 1, current.y);
		if (c == '<') current = (current.x - 1, current.y);
		if (c == '^') current = (current.x, current.y + 1);
		if (c == 'v') current = (current.x, current.y - 1);
		return current;
	})
	.Concat(new[] { (0, 0) })
	.Distinct()
	.Count();
	
current = (x: 0, y: 0);
var other = current;
var bothHouses = input
	.Select(c =>
	{
		var t = other;
		if (c == '>') other = (current.x + 1, current.y);
		if (c == '<') other = (current.x - 1, current.y);
		if (c == '^') other = (current.x, current.y + 1);
		if (c == 'v') other = (current.x, current.y - 1);
		current = t;
		return other;
	})
	.Concat(new[] { (0, 0) })
	.Distinct()
	.Count();

santaHouses.Dump("Part A");
bothHouses.Dump("Part B");
