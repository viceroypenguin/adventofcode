namespace AdventOfCode;

public class Day_2015_05_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 5;
	public override CodeType CodeType => CodeType.Original;

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

	protected override void ExecuteDay(byte[] input)
	{
		var lines = input.GetLines();

		Dump('A',
			lines.Count(s => IsNicePartA(s)));
		Dump('B',
			lines
				.Count(s => IsNicePartB(s)));
	}
}
