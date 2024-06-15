namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 01, CodeType.Fastest)]
public class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span[..^1];

		var sum = 0;

		var last = span[^1];
		foreach (var c in span)
		{
			if (c == last)
				sum += c - '0';
			last = c;
		}

		var partA = sum;

		sum = 0;
		for (int i = 0, j = span.Length / 2; j < span.Length; i++, j++)
		{
			if (span[i] == span[j])
				sum += span[i] - '0';
		}

		var partB = sum << 1;

		return (partA.ToString(), partB.ToString());
	}
}
