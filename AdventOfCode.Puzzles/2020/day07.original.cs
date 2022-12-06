namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 7, CodeType.Original)]
public partial class Day_07_Original : IPuzzle
{
	[GeneratedRegex("^(?<container>[\\w ]+?) bags contain ((?<none>no other bags)|((?<contained>[\\w ]+) bags?,? ?)+).$", RegexOptions.ExplicitCapture)]
	private static partial Regex BagsRegex();

	[GeneratedRegex("^(\\d+) (.*)$")]
	private static partial Regex ContainedRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = BagsRegex();

		var bagRules = input.Lines
			.Select(l => regex.Match(l))
			.ToDictionary(
				m => m.Groups["container"].Value,
				m => m.Groups["contained"].Captures
					.Select(c => ContainedRegex().Match(c.Value))
					.Select(m => (
						count: Convert.ToInt32(m.Groups[1].Value),
						color: m.Groups[2].Value))
					.ToArray());

		var reverse = bagRules
			.SelectMany(
				kvp => kvp.Value,
				(kvp, c) => (from: kvp.Key, c.color))
			.ToLookup(x => x.color, x => x.from);

		var visited = new HashSet<string>();
		void visitReverse(string color)
		{
			if (visited.Contains(color))
				return;
			visited.Add(color);
			foreach (var c in reverse[color])
				visitReverse(c);
		}

		visitReverse("shiny gold");
		var part1 = (visited.Count - 1).ToString();

		int bagTotal(string color) =>
			1 + bagRules[color].Sum(x => x.count * bagTotal(x.color));
		var part2 = (bagTotal("shiny gold") - 1).ToString();

		return (part1, part2);
	}
}
