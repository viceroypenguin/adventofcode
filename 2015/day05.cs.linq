<Query Kind="Program" />

char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u', };
bool HasThreeVowels(string str)
{
	return str
		.Where(c => vowels.Contains(c))
		.Take(3)
		.Count() == 3;
}

bool HasPair(string str)
{
	return str.Zip(
			str.Skip(1),
			(f, s) => f == s)
		.Any(b => b);
}

string[] evilStrings = new string[] { "ab", "cd", "pq", "xy", };
bool HasEvilStrings(string str)
{
	return evilStrings
		.Any(s => str.Contains(s));
}

bool IsNicePartA(string str)
{
	return 
		!HasEvilStrings(str) &&
		HasPair(str) &&
		HasThreeVowels(str);
}

bool HasRepeatLetter(string str)
{
	return str.Zip(
			str.Skip(2),
			(f, s) => f == s)
		.Any(b => b);
}

bool HasDuplicatePair(string str)
{
	return
		Enumerable.Range(0, str.Length - 1)
			.Select(i => new { index = i, pair = str.Substring(i, 2) })
			.GroupBy(_ => _.pair)
			.Where(g => g.Count() > 1)
			.Where(g => g.Last().index - g.First().index > 1)
			.Any();
}

bool IsNicePartB(string str)
{
	return
		HasDuplicatePair(str) &&
		HasRepeatLetter(str);
}

void Main()
{
	var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day05.input.txt"))
		.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.ToArray();

	input
		.Count(s => IsNicePartA(s))
		.Dump("Part A");
	input
		.Count(s => IsNicePartB(s))
		.Dump("Part B");
}

// Define other methods and classes here