namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 02, CodeType.Fastest)]
public partial class Day_02_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;
		var span = input.Span;

		var id = 0;
		while (span.Length > 0)
		{
			id++;
			span = span[(id switch { >= 100 => 10, >= 10 => 9, _ => 8, })..];

			var maxRed = 0;
			var maxGreen = 0;
			var maxBlue = 0;

			while (true)
			{
				var (num, n) = span.AtoI();
				span = span[(n + 1)..];

				switch (span[0])
				{
					case (byte)'r':
					{
						maxRed = Math.Max(maxRed, num);
						span = span[3..];
						break;
					}
					case (byte)'b':
					{
						maxBlue = Math.Max(maxBlue, num);
						span = span[4..];
						break;
					}
					case (byte)'g':
					{
						maxGreen = Math.Max(maxGreen, num);
						span = span[5..];
						break;
					}
					default:
						break;
				}

				if (span[0] == (byte)'\n')
					break;

				span = span[2..];
			}

			span = span[1..];

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
