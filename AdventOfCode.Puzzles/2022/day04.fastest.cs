namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 4, CodeType.Fastest)]
public partial class Day_04_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = new ReadOnlySpan<byte>(input.Bytes);

		var part1 = 0;
		var part2 = 0;
		for (var i = 0; i < span.Length;)
		{
			var (lo1, c) = span[i..].AtoI();
			i += c + 1;
			(var hi1, c) = span[i..].AtoI();
			i += c + 1;
			(var lo2, c) = span[i..].AtoI();
			i += c + 1;
			(var hi2, c) = span[i..].AtoI();
			i += c + 1;

			if (lo1 <= lo2 && hi2 <= hi1
				|| lo2 <= lo1 && hi1 <= hi2)
			{
				part1++;
			}

			if (hi1 >= lo2 && lo1 <= hi2
				|| hi2 >= lo1 && lo2 <= hi1)
			{
				part2++;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
