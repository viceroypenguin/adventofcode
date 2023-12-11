namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 11, CodeType.Original)]
public partial class Day_11_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var blankRows = input.Lines.Index()
			.Where(x => x.item.All(b => b == '.'))
			.Select(x => x.index)
			.ToList();

		var blankCols = Enumerable.Range(0, input.Lines[0].Length)
			.Where(i => input.Lines.All(l => l[i] == '.'))
			.ToList();

		var stars = input.Bytes.GetMap()
			.GetMapPoints()
			.Where(p => p.item == '#')
			.Select(p => p.p)
			.ToList();

		var part1 = 0;
		for (var x = 0; x < stars.Count; x++)
		{
			var p1 = stars[x];
			for (var y = x + 1; y < stars.Count; y++)
			{
				var p2 = stars[y];

				var baseManhattan = Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
				baseManhattan += blankRows.Count(r => r.Between(p1.y, p2.y));
				baseManhattan += blankCols.Count(r => r.Between(p1.x, p2.x));

				part1 += baseManhattan;
			}
		}

		var part2 = 0L;
		for (var x = 0; x < stars.Count; x++)
		{
			var p1 = stars[x];
			for (var y = x + 1; y < stars.Count; y++)
			{
				var p2 = stars[y];

				long baseManhattan = Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
				baseManhattan += blankRows.Count(r => r.Between(p1.y, p2.y)) * 999_999L;
				baseManhattan += blankCols.Count(r => r.Between(p1.x, p2.x)) * 999_999L;

				part2 += baseManhattan;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
