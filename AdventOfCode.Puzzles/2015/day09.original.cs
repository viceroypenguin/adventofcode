namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 09, CodeType.Original)]
public class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var edges = input.Lines
			.Select(x => x.Split(new[] { " to ", " = " }, StringSplitOptions.None))
			.Select(x => new { Start = x[0], End = x[1], Distance = Convert.ToInt32(x[2]) })
			.OrderBy(x => x.Distance)
			.ToList();

		var points = edges.Select(x => x.Start).Concat(edges.Select(x => x.End)).Distinct().ToList();

		var paths = points
			.Permutations()
			.Select(p => p.Lead(1))
			.Select(p => p
				.Sum(e => edges.SingleOrDefault(_ => (_.Start == e.current && _.End == e.lead) || (_.Start == e.lead && _.End == e.current))?.Distance))
			.ToList();

		return (
			paths
				.Min()
				.ToString(),
			paths
				.Max()
				.ToString());
	}
}
