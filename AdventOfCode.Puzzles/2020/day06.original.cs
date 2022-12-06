namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 6, CodeType.Original)]
public class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var answerSets = input.Lines
			.Segment(string.IsNullOrWhiteSpace)
			.ToArray();

		var part1 = answerSets
			.Sum(l => l.SelectMany(c => c)
				.Distinct()
				.Count())
			.ToString();

		var part2 = answerSets
			.Sum(l =>
			{
				var numPeople = l.Where(s => !string.IsNullOrWhiteSpace(s)).Count();
				return l.SelectMany(c => c)
					.GroupBy(
						c => c,
						(c, _) => _.Count())
					.Count(x => x == numPeople);
			})
			.ToString();

		return (part1, part2);
	}
}
