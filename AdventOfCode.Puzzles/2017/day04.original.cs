namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 04, CodeType.Original)]
public class Day_04_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var lines = input.Lines
			.Select(x => x.Split())
			.ToList();

		var partA =
			lines
				.Where(l => l.Distinct().Count() == l.Length)
				.Count();

		var partB =
			lines
				.Where(l => l.Select(s => new string(s.OrderBy(c => c).ToArray())).Distinct().Count() == l.Length)
				.Count();

		return (partA.ToString(), partB.ToString());
	}
}
