namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 05, CodeType.Original)]
public class Day_05_Original : IPuzzle
{
	private static bool HasThreeVowels(string str) => str
		.Where(c => c is 'a' or 'e' or 'i' or 'o' or 'u')
		.Take(3)
		.Count() == 3;

	private static bool HasPair(string str) =>
		str.Lead(1, (f, s) => f == s).Any(b => b);

	private static readonly string[] s_evilStrings = ["ab", "cd", "pq", "xy",];
	private static bool HasEvilStrings(string str) =>
		s_evilStrings.Any(str.Contains);

	private static bool IsNicePartA(string str) =>
		!HasEvilStrings(str)
		&& HasPair(str)
		&& HasThreeVowels(str);

	private static bool HasRepeatLetter(string str) =>
		str.Lead(2, (f, s) => f == s).Any(b => b);

	private static bool HasDuplicatePair(string str) =>
		Enumerable.Range(0, str.Length - 1)
			.Select(i => new { index = i, pair = str.Substring(i, 2) })
			.GroupBy(_ => _.pair)
			.Where(g => g.Count() > 1)
			.Any(g => g.Last().index - g.First().index > 1);

	private static bool IsNicePartB(string str) =>
		HasDuplicatePair(str)
		&& HasRepeatLetter(str);

	public (string, string) Solve(PuzzleInput input)
	{
		var lines = input.Lines;

		return (
			lines.Count(IsNicePartA).ToString(),
			lines.Count(IsNicePartB).ToString());
	}
}
