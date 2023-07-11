namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 06, CodeType.Original)]
public class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var words = input.Lines;

		static char ProcessLetter(IEnumerable<char> characters, int multiplier) =>
			characters
				.GroupBy(
					c => c,
					(c, _) => new { c, cnt = _.Count(), })
				.OrderBy(c => c.cnt * multiplier)
				.Select(c => c.c)
				.First();

		var partA =
			string.Join("",
				Enumerable.Range(0, words[0].Length)
					.Select(i => words.Select(w => w[i]))
					.Select(characters => ProcessLetter(characters, -1)));

		var partB =
			string.Join("",
				Enumerable.Range(0, words[0].Length)
					.Select(i => words.Select(w => w[i]))
					.Select(characters => ProcessLetter(characters, 1)));

		return (partA, partB);
	}
}
