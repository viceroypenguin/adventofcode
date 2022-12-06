namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 19, CodeType.Original)]
public class Day_19_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var segments = input.Lines
			.Segment(string.IsNullOrWhiteSpace)
			.ToArray();

		var rulesBase = segments[0]
			.Select(x => x.Split(':', StringSplitOptions.TrimEntries))
			.ToDictionary(x => x[0], x => x[1]);
		var processed = new Dictionary<string, string>();

		string BuildRegex(string input)
		{
			if (processed.TryGetValue(input, out var s))
				return s;

			var orig = rulesBase[input];
			if (orig.StartsWith('\"'))
				return processed[input] = orig.Replace("\"", "");

			if (!orig.Contains("|"))
				return processed[input] = string.Join("", orig.Split().Select(BuildRegex));

			return processed[input] =
				"(" +
				string.Join("", orig.Split().Select(x => x == "|" ? x : BuildRegex(x))) +
				")";
		}

		var regex = new Regex("^" + BuildRegex("0") + "$");
		var part1 = segments[1].Count(regex.IsMatch).ToString();

		regex = new Regex($@"^({BuildRegex("42")})+(?<open>{BuildRegex("42")})+(?<close-open>{BuildRegex("31")})+(?(open)(?!))$");
		var part2 = segments[1].Count(regex.IsMatch).ToString();

		return (part1, part2);
	}
}
