namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 7, CodeType.Original)]
public class Day_2021_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var crabs = input.Text.Split(',').Select(int.Parse);

		var part1 =
			Enumerable.Range(0, crabs.Max())
				// for each position, get the sum of the
				// absolute difference between each crab
				// and that position
				.Select(c => crabs.Sum(c2 => Math.Abs(c2 - c)))
				// get the minimum total sum
				.Min()
				.ToString();

		var part2 =
			// start with all possible positions
			Enumerable.Range(0, crabs.Max())
				// for each position, get the sum of the fuel used
				.Select(c => crabs
					// absolute difference
					.Select(c2 => Math.Abs(c2 - c))
					// fuel: sum of numbers 1..n
					.Sum(n => n * (n + 1) / 2))
				// minimum total sum
				.Min()
				.ToString();

		return (part1, part2);
	}
}
