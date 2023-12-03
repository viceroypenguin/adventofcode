namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 02, CodeType.Fastest)]
public partial class Day_02_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;
		var span = input.Span;

		var id = 1;
		while (span.Length > 0)
		{
			span = span.Slice(
				id switch { > 100 => 10, > 10 => 9, _ => 8, });

			var maxRed = 0;
			var maxGreen = 0;
			var maxBlue = 0;

			while (true)
			{
				var (num, n) = span.AtoI();
				span = span.Slice(n + 1);

				switch (span[0])
				{
					case (byte)'r':
					{
						maxRed = Math.Max(maxRed, num);
						span = span.Slice(3);
						break;
					}
					case (byte)'b':
					{
						maxBlue = Math.Max(maxBlue, num);
						span = span.Slice(4);
						break;
					}
					case (byte)'g':
					{
						maxGreen = Math.Max(maxGreen, num);
						span = span.Slice(5);
						break;
					}
				}

				if (span[0] == (byte)'\n')
					break;

				span = span.Slice(2);
			}

			span = span.Slice(1);

			if (maxRed <= 12
				&& maxGreen <= 13
				&& maxBlue <= 14)
			{
				part1 += id;
			}

			part2 += maxRed * maxGreen * maxBlue;
		}

		return (part1.ToString(), part2.ToString());
	}
}
