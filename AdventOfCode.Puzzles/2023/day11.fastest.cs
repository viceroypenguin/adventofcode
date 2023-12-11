namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 11, CodeType.Fastest)]
public sealed partial class Day_11_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		var width = span.IndexOf((byte)'\n');
		Span<int> cols = stackalloc int[width];

		var part1 = 0;
		var part2 = 0L;

		var galaxies = 0;
		var yDistanceP1 = 0;
		var yDistanceP2 = 0L;
		foreach (var line in span.EnumerateLines(width))
		{
			span = line;

			var x = 0;
			var gap = true;

			while (true)
			{
				var n = span.IndexOf((byte)'#');
				if (n < 0)
					break;

				part1 += yDistanceP1;
				part2 += yDistanceP2;

				gap = false;
				galaxies++;
				cols[x + n]++;
				x += n + 1;
				span = span.Slice(n + 1);
			}

			yDistanceP1 += galaxies;
			yDistanceP2 += galaxies;

			if (gap)
			{
				yDistanceP1 += galaxies;
				yDistanceP2 += galaxies * 999_999L;
			}
		}

		galaxies = 0;
		var xDistanceP1 = 0;
		var xDistanceP2 = 0L;
		foreach (var col in cols)
		{
			part1 += xDistanceP1 * col;
			part2 += xDistanceP2 * col;

			galaxies += col;
			xDistanceP1 += galaxies;
			xDistanceP2 += galaxies;

			if (col == 0)
			{
				xDistanceP1 += galaxies;
				xDistanceP2 += galaxies * 999_999L;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
