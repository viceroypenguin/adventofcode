namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 02, CodeType.Original)]
public class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var lines = input.Lines
			.Select(x => x.Split().Select(s => Convert.ToInt32(s)).ToList())
			.ToList();

		var partA =
			lines
				.Select(x => x.Max() - x.Min())
				.Sum();

		var partB =
			lines
				.Select(arr => (
						from num in arr
						from div in arr
						where num != div
						where num % div == 0
						select num / div)
					.Single())
				.Sum();

		return (partA.ToString(), partB.ToString());
	}
}
