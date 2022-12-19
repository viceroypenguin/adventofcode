namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 02, CodeType.Original)]
public class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var (twos, threes) = input.Lines
			.Select(id =>
			{
				var chars = id
					.GroupBy(c => c)
					.Select(x => x.Count())
					.ToList();
				return (two: chars.Any(x => x == 2), three: chars.Any(x => x == 3));
			})
			.Aggregate((twos: 0, threes: 0), (acc, next) =>
				(acc.twos + (next.two ? 1 : 0), acc.threes + (next.three ? 1 : 0)));

		var part1 = (twos * threes).ToString();

		var part2 = 
			new string(
				input.Lines
					.OrderBy(x => x)
					.Window(2)
					.Select(pair => (
						pair,
						letters: pair[0].Zip(pair[1], (l, r) => (l, r))))
					.Where(x => x.letters.Count(y => y.l != y.r) == 1)
					.SelectMany(x => x.letters.Where(y => y.l == y.r))
					.Select(x => x.l)
					.ToArray());

		return (part1, part2);
	}
}
