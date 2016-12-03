<Query Kind="Statements" />

var input = File.ReadAllText(@"C:\Users\stuart.turner\desktop\input.txt");

var redsRegex = new Regex("{[^{}]*(((?<before>{)[^{}]*)+((?<-before>})[^{}]*)+)*(?(before)(?!))[^{}]*:\"red\"[^{}]*(((?<before>{)[^{}]*)+((?<-before>})[^{}]*)+)*(?(before)(?!))[^{}]*}");
input = redsRegex.Replace(input, "");

var regex = new Regex("[,:[](-?\\d+)");

regex.Matches(input)
	.OfType<Match>()
	.Select(c=>c.Groups[1])
	.Select(c=>c.Value)
	.Select(c=>Convert.ToInt32(c))
	.Sum()
	.Dump();
