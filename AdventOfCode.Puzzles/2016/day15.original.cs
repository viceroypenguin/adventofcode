namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 15, CodeType.Original)]
public partial class Day_15_Original : IPuzzle
{
	[GeneratedRegex("Disc #(?<disc_num>\\d+) has (?<num_positions>\\d+) positions; at time=0, it is at position (?<initial_position>\\d+).", RegexOptions.ExplicitCapture)]
	private static partial Regex DiscRegex();

	private sealed record Disc(int NumPositions, int InitialPosition);

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = DiscRegex();

		var discs =
			input.Lines
				.Select(s => regex.Match(s))
				.Select(m => new Disc(
					Convert.ToInt32(m.Groups["num_positions"].Value),
					Convert.ToInt32(m.Groups["initial_position"].Value)))
				.ToList();

		var partA = GetDiscTime(discs);

		discs.Add(new(11, 0));
		var partB = GetDiscTime(discs);

		return (partA.ToString(), partB.ToString());
	}

	private static int GetDiscTime(List<Disc> discs)
	{
		for (var initialTime = 0; ; initialTime++)
		{
			var flag = true;
			for (var capsulePosition = 1; flag && capsulePosition <= discs.Count; capsulePosition++)
			{
				var disc = discs[capsulePosition - 1];
				var discPosition = (initialTime + capsulePosition + disc.InitialPosition) % disc.NumPositions;
				if (discPosition != 0) flag = false;
			}

			if (flag)
			{
				return initialTime;
			}
		}
	}
}
