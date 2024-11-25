namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 20, CodeType.Original)]
public partial class Day_20_Original : IPuzzle
{
	[GeneratedRegex("(?<from>\\d+)-(?<to>\\d+)", RegexOptions.ExplicitCapture)]
	private static partial Regex AddressRangeRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var max = uint.MaxValue;
		var regex = AddressRangeRegex();

		var blocked =
			input.Lines
				.Select(s => regex.Match(s))
				.Select(m => new { from = Convert.ToUInt32(m.Groups["from"].Value), to = Convert.ToUInt32(m.Groups["to"].Value), })
				.OrderBy(b => b.from)
				.ToList();

		var first = 0u;
		foreach (var b in blocked)
		{
			if (b.from > first)
				break;

			first = Math.Max(b.to + 1, first);
		}

		var partA = first;

		first = 0u;
		var allowed = blocked.Take(0).ToList();
		foreach (var b in blocked)
		{
			if (b.from > first)
				allowed.Add(new { from = first, to = b.from - 1 });

			if (b.to == max)
				break;

			first = Math.Max(Math.Max(b.to, b.to + 1), first);
		}

		var partB = allowed.Select(b => (int)(b.to - b.from + 1)).Sum();

		return (partA.ToString(), partB.ToString());
	}
}
