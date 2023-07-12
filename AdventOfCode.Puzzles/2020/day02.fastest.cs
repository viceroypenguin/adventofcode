namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 2, CodeType.Original)]
public class Day_02_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		int part1 = 0, part2 = 0;

		var span = input.GetSpan();
		for (int i = 0; i < span.Length;)
		{
			var x = span[i..].AtoI();
			var min = x.value;
			i += x.numChars + 1;

			x = span[i..].AtoI();
			var max = x.value;
			i += x.numChars + 1;

			var chr = span[i];
			i += 2;

			part2 += ((span[i + min] == chr) ^ (span[i + max] == chr)) ? 1 : 0;
			i++;

			var cnt = 0;
			for (; i < span.Length && span[i] != '\n'; i++)
				cnt += span[i] == chr ? 1 : 0;
			part1 += cnt >= min && cnt <= max ? 1 : 0;

			i++;
		}

		return (
			part1.ToString(),
			part2.ToString());
	}
}
