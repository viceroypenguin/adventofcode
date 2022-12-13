namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 04, CodeType.Original)]
public class Day_04_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var range = input.Text.Split('-');
		var min = Convert.ToInt32(range[0]);
		var max = Convert.ToInt32(range[1]);

		var part1 = Enumerable.Range(min, max - min + 1)
			.Where(i => i.ToString().Window(2).All(x => x[0] <= x[1]))
			.Where(i => i.ToString().GroupAdjacent(c => c).Any(g => g.Count() >= 2))
			.Count()
			.ToString();

		var part2 = Enumerable.Range(min, max - min + 1)
			.Where(i => i.ToString().Window(2).All(x => x[0] <= x[1]))
			.Where(i => i.ToString().GroupAdjacent(c => c).Any(g => g.Count() == 2))
			.Count()
			.ToString();

		return (part1, part2);
	}
}
