<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day12.input.txt"));
var regex = new Regex("[,:[](-?\\d+)");

regex.Matches(input)
	.OfType<Match>()
	.Select(c => c.Groups[1])
	.Select(c => c.Value)
	.Select(c => Convert.ToInt32(c))
	.Sum()
	.Dump("Part A");

var redsRegex = new Regex("{[^{}]*(((?<before>{)[^{}]*)+((?<-before>})[^{}]*)+)*(?(before)(?!))[^{}]*:\"red\"[^{}]*(((?<before>{)[^{}]*)+((?<-before>})[^{}]*)+)*(?(before)(?!))[^{}]*}");
input = redsRegex.Replace(input, "");

regex.Matches(input)
	.OfType<Match>()
	.Select(c => c.Groups[1])
	.Select(c => c.Value)
	.Select(c => Convert.ToInt32(c))
	.Sum()
	.Dump("Part B");
