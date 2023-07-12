namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Lines[0]
			.Select(x => x - '0')
			.ToList();

		var partA =
			data
				.Append(data[0])
				.Lead(1)
				.Where(x => x.current == x.lead)
				.Select(x => x.current)
				.Sum();

		var rotInput = data.Skip(data.Count / 2).Concat(data.Take(data.Count / 2));
		var partB =
			data.Zip(rotInput, (a, b) => new { a, b })
				.Where(x => x.a == x.b)
				.Select(x => x.a)
				.Sum();

		return (partA.ToString(), partB.ToString());
	}
}
