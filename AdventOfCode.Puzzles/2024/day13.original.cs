namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 13, CodeType.Original)]
public partial class Day_13_Original : IPuzzle
{
	[GeneratedRegex(@"^(Button [AB]|Prize): X[+=](?<x>\d+), Y[+=](?<y>\d+)$")]
	private static partial Regex PuzzleLineRegex { get; }

	public (string, string) Solve(PuzzleInput input)
	{
		static (int x, int y) TransformLine(string line)
		{
			var match = PuzzleLineRegex.Match(line);
			return (int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));
		}

		var machines = input.Lines.Split(string.Empty)
			.Select(g =>
				(
					A: TransformLine(g[0]),
					B: TransformLine(g[1]),
					C: TransformLine(g[2])
				)
			)
			.ToList();

		var (part1, part2) = machines
			.Aggregate(
				(0, 0L),
				(agg, m) =>
				{
					var det = (m.A.x * m.B.y) - (m.A.y * m.B.x);
					if (det == 0)
						return agg;

					var e1 = m.C.x;
					var f1 = m.C.y;

					var a1 = ((e1 * m.B.y) - (m.B.x * f1)) / det;
					var b1 = ((f1 * m.A.x) - (m.A.y * e1)) / det;

					var p1 =
						!a1.Between(1, 100) || !b1.Between(1, 100)
						|| (a1 * m.A.x) + (b1 * m.B.x) != e1
						|| (a1 * m.A.y) + (b1 * m.B.y) != f1
						? 0
						: (3 * a1) + b1;

					var e2 = m.C.x + 10_000_000_000_000L;
					var f2 = m.C.y + 10_000_000_000_000L;

					var a2 = ((e2 * m.B.y) - (m.B.x * f2)) / det;
					var b2 = ((f2 * m.A.x) - (m.A.y * e2)) / det;

					var p2 =
						a2 <= 0 || b2 <= 0
						|| (a2 * m.A.x) + (b2 * m.B.x) != e2
						|| (a2 * m.A.y) + (b2 * m.B.y) != f2
						? 0
						: (3 * a2) + b2;

					return (agg.Item1 + p1, agg.Item2 + p2);
				}
			);

		return (part1.ToString(), part2.ToString());
	}
}
