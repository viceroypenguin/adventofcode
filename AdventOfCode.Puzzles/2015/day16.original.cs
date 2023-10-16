namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 16, CodeType.Original)]
public partial class Day_16_Original : IPuzzle
{
	[GeneratedRegex(@"Sue (\d+):( \w+: \d+,?)+")]
	private static partial Regex SueIngredientRegex();

	[GeneratedRegex(@" ?(\w+): (\d+)")]
	private static partial Regex IngredientRegex();

	private static readonly char[] newline = ['\r', '\n'];

	public (string, string) Solve(PuzzleInput input)
	{
		var giftInput =
@"children: 3
cats: 7
samoyeds: 2
pomeranians: 3
akitas: 0
vizslas: 0
goldfish: 5
trees: 3
cars: 2
perfumes: 1";

		var regex = SueIngredientRegex();
		var detailRegex = IngredientRegex();

		var sues = input.Lines
			.Select(x => regex.Match(x))
			.Select(x => new
			{
				number = Convert.ToInt32(x.Groups[1].Value),
				details = x.Groups[2].Captures.OfType<Capture>()
					.Select(c => detailRegex.Match(c.Value))
					.Select(c => new
					{
						detailType = c.Groups[1].Value,
						detailValue = Convert.ToInt32(c.Groups[2].Value),
					})
					.ToList(),
			})
			.ToList();

		var giftDetails = giftInput.Split(newline, StringSplitOptions.RemoveEmptyEntries)
			.Select(c => detailRegex.Match(c))
			.Select(c => new
			{
				detailType = c.Groups[1].Value,
				detailValue = Convert.ToInt32(c.Groups[2].Value),
			})
			.ToDictionary(x => x.detailType);

		var partA = 0;
		foreach (var s in sues)
		{
			var flag = !s.details
				.Any(d =>
					!giftDetails.ContainsKey(d.detailType)
					|| giftDetails[d.detailType].detailValue != d.detailValue);

			if (flag)
				partA = s.number;
		}

		var partB = 0;
		foreach (var s in sues)
		{
			var flag = true;
			foreach (var d in s.details.Where(d => giftDetails.ContainsKey(d.detailType)))
			{
				if (d.detailType is "cats" or "trees")
				{
					// gift detail is minimum value of aunt
					if (giftDetails[d.detailType].detailValue >= d.detailValue)
					{
						flag = false;
						break;
					}
				}
				else if (d.detailType is "pomeranians" or "goldfish")
				{
					// gift detail is maximum value of aunt
					if (giftDetails[d.detailType].detailValue <= d.detailValue)
					{
						flag = false;
						break;
					}
				}
				else
				{
					if (giftDetails[d.detailType].detailValue != d.detailValue)
					{
						flag = false;
						break;
					}
				}
			}

			if (flag)
				partB = s.number;
		}

		return (partA.ToString(), partB.ToString());
	}
}
