namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 08, CodeType.Original)]
public class Day_08_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var layers = input.Bytes
			.SkipLast(1)
			.Batch(25 * 6)
			.Select(s => s.ToList())
			.ToList();

		var zeroLayer = layers
			.OrderBy(x => x.Count(z => z == '0'))
			.First();

		var part1 = (zeroLayer.Count(z => z == '1') * zeroLayer.Count(z => z == '2')).ToString();

		var part2 = string.Join(Environment.NewLine,
			Enumerable.Range(0, 25 * 6)
				.Select(p => Enumerable.Range(0, layers.Count)
					.Select(l => layers[l][p])
					.Aggregate('2', (c, lc) => c != '2' ? c : lc == '0' ? ' ' : lc == '1' ? '█' : (char)lc))
				.Batch(25)
				.Select(b => string.Join("", b)));

		return (part1, part2);
	}
}
