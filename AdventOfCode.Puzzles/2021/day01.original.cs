namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 1, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var depths = input.Lines.Select(int.Parse);

		var part1 = depths.Window(2)
			.Where(x => x.Last() > x.First())
			.Count()
			.ToString();

		var part2 = depths.Window(4)
			.Where(x => x.Last() > x.First())
			.Count()
			.ToString();

		return (part1, part2);
	}
}
