namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 13, CodeType.Original)]
public class Day_13_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var edges = input.Lines
			.Select(x =>
			{
				var splits = x.Split();
				return new { Start = splits[0], End = splits[10].TrimEnd('.'), Distance = Convert.ToInt32(splits[3]) * (splits[2] == "gain" ? +1 : -1) };
			})
			.OrderBy(x => x.Distance)
			.ToList();

		var points = edges.Select(x => x.Start).Concat(edges.Select(x => x.End)).Distinct().ToList();

		var best = points.Take(points.Count - 1)
			.Permutations()
			.Select(p => p.Prepend(points[^1]).Append(points[^1]))
			.Select(p => p.Lead(1))
			.Select(p => p.Sum(e =>
				edges
					.Where(_ => (_.Start == e.current && _.End == e.lead) || (_.Start == e.lead && _.End == e.current))
					.Sum(_ => _.Distance)))
			.Max();

		var partA = best;

		best = points
			.Permutations()
			.Select(p => p.Prepend("myself").Append("myself"))
			.Select(p => p.Lead(1))
			.Select(p => p.Sum(e =>
				edges
					.Where(_ => (_.Start == e.current && _.End == e.lead) || (_.Start == e.lead && _.End == e.current))
					.Sum(_ => _.Distance)))
			.Max();

		var partB = best;

		return (partA.ToString(), partB.ToString());
	}
}
