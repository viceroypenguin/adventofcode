namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 22, CodeType.Original)]
public partial class Day_22_Original : IPuzzle
{
	[GeneratedRegex(@"/dev/grid/node-x(?<x>\d+)-y(?<y>\d+)\s+(?<total>\d+)T\s+(?<used>\d+)T\s+(?<avail>\d+)T\s+\d+%")]
	private static partial Regex GridRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = GridRegex();

		var nodes =
			input.Lines
				.Skip(2)
				.Select(s => regex.Match(s))
				.Select(m => new
				{
					x = Convert.ToInt32(m.Groups["x"].Value),
					y = Convert.ToInt32(m.Groups["y"].Value),
					total = Convert.ToInt32(m.Groups["total"].Value),
					used = Convert.ToInt32(m.Groups["used"].Value),
					avail = Convert.ToInt32(m.Groups["avail"].Value),
				})
				.ToList();

		var partA =
			(
				from a in nodes
				where a.used != 0
				from b in nodes
				where a.x != b.x || a.y != b.y
				where a.used < b.avail
				select new { a, b }
			).Count();

		// Apparently, I completed part b by hand or something?
		return (partA.ToString(), string.Empty);
	}
}
