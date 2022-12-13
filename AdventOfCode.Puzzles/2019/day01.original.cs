namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		static IEnumerable<int> fuelValues(int start) =>
			SuperEnumerable.Generate(start, s => Math.Max(s / 3 - 2, 0))
				.Skip(1)
				.TakeWhile(s => s != 0);

		var numbers = input.Lines
			.Select(s => Convert.ToInt32(s))
			.ToList();

		var part1 = numbers
			.Select(s => fuelValues(s).First())
			.Sum()
			.ToString();

		var part2 = numbers
			.Select(s => fuelValues(s).Sum())
			.Sum()
			.ToString();

		return (part1, part2);
	}
}
