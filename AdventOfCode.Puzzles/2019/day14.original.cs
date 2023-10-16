namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 14, CodeType.Original)]
public partial class Day_14_Original : IPuzzle
{
	private const long OneTrillion = 1_000_000_000_000L;

	[GeneratedRegex("^((?<in>\\d+ \\w+),? )+=> (?<out>\\d+ \\w+)$", RegexOptions.ExplicitCapture)]
	private static partial Regex ReactionRegex();
	[GeneratedRegex("(\\d+) (\\w+)")]
	private static partial Regex MaterialRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = ReactionRegex();
		var materialRegex = MaterialRegex();

		(int amt, string mat) ParseMaterialDefinition(string definition)
		{
			var match = materialRegex.Match(definition);
			return (amt: int.Parse(match.Groups[1].Value), mat: match.Groups[2].Value);
		}

		var recipes = input.Lines
			.Select(r => regex.Match(r))
			.Select(m =>
			{
				var output = ParseMaterialDefinition(m.Groups["out"].Value);

				var inp = m.Groups["in"]
					.Captures
					.OfType<Capture>()
					.Select(c => ParseMaterialDefinition(c.Value))
					.ToList();
				return (inp, output);
			})
			.ToDictionary(x => x.output.mat);

		var ore = CalculateOreRequirement(recipes, (1, "FUEL"));
		var part1 = ore.ToString();

		var guess = OneTrillion / ore;
		while (true)
		{
			ore = CalculateOreRequirement(recipes, (guess, "FUEL"));
			var newGuess = guess + (guess * (OneTrillion - ore) / OneTrillion);
			if (newGuess == guess)
				break;
			guess = newGuess;
		}

		var part2 = guess.ToString();
		return (part1, part2);
	}

	private static long CalculateOreRequirement(
		Dictionary<string, (List<(int amt, string mat)> inp, (int amt, string mat) output)> recipes,
		(long, string) requirement)
	{
		var materials = new Queue<(long amt, string mat)>();
		materials.Enqueue(requirement);

		var excess = new Dictionary<string, long>();
		var ore = 0L;

		while (materials.Count != 0)
		{
			var (amt, mat) = materials.Dequeue();
			if (mat == "ORE")
			{
				ore += amt;
				continue;
			}

			if (excess.TryGetValue(mat, out var exAmt))
			{
				var used = Math.Min(exAmt, amt);
				amt -= used;
				excess[mat] = exAmt - used;
			}

			if (amt == 0)
				continue;

			var (inp, output) = recipes[mat];
			var factor = ((amt - 1) / output.amt) + 1;
			exAmt = (factor * output.amt) - amt;
			if (exAmt != 0)
				excess[mat] = exAmt;

			foreach (var (qAmt, qMat) in inp)
				materials.Enqueue((qAmt * factor, qMat));
		}

		return ore;
	}
}
