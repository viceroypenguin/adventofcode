namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 12, CodeType.Original)]
public partial class Day_12_Original : IPuzzle
{
	[GeneratedRegex("^(?<prog_a>\\w+) \\<-\\> ((?<prog_b>\\w+)(,\\s*)?)*$", RegexOptions.Compiled)]
	private static partial Regex ProgramRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = ProgramRegex();
		var instructions = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => new int[] { Convert.ToInt32(m.Groups["prog_a"].Value), }
				.Concat(m.Groups["prog_b"].Captures.OfType<Capture>().Select(c => Convert.ToInt32(c.Value)))
				.ToList())
			.ToList();

		var groups = new List<HashSet<int>>();
		foreach (var m in instructions)
		{
			var existingL = groups.Where(g => m.Any(id => g.Contains(id))).ToList();
			if (existingL.Count > 1)
			{
				var g = existingL[0];
				foreach (var g2 in existingL.Skip(1))
				{
					g.UnionWith(g2);
					_ = groups.Remove(g2);
				}
				existingL = new List<HashSet<int>>() { g };
			}

			var existing = existingL.SingleOrDefault();
			if (existing != null)
				existing.UnionWith(m);
			else
				groups.Add(new HashSet<int>(m));
		}

		var partA = groups.Single(g => g.Contains(0)).Count;
		var partB = groups.Count;

		return (partA.ToString(), partB.ToString());
	}
}
