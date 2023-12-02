namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 01, CodeType.Fastest)]
public partial class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;
		foreach (var l in input.Text.AsSpan().EnumerateLines())
		{
			if (l.Length == 0)
				continue;

			var firstDigitIndex = l.IndexOfAnyInRange('0', '9');
			var lastDigitIndex = l.LastIndexOfAnyInRange('0', '9');

			var tens = (l[firstDigitIndex] - '0') * 10;
			var ones = l[lastDigitIndex] - '0';
			part1 += tens + ones;

			var span = l[..firstDigitIndex];
			while (span.Length >= 3)
			{
				(tens, var success) = span[0] switch
				{
					'o' when span.StartsWith("one") => (10, true),
					't' when span.StartsWith("two") => (20, true),
					't' when span.StartsWith("three") => (30, true),
					'f' when span.StartsWith("four") => (40, true),
					'f' when span.StartsWith("five") => (50, true),
					's' when span.StartsWith("six") => (60, true),
					's' when span.StartsWith("seven") => (70, true),
					'e' when span.StartsWith("eight") => (80, true),
					'n' when span.StartsWith("nine") => (90, true),
					_ => (tens, false),
				};
				if (success) break;

				span = span[1..];
			}

			span = l[(lastDigitIndex + 1)..];
			while (span.Length >= 3)
			{
				(ones, var success) = span[^1] switch
				{
					'e' when span.EndsWith("one") => (1, true),
					'o' when span.EndsWith("two") => (2, true),
					'e' when span.EndsWith("three") => (3, true),
					'r' when span.EndsWith("four") => (4, true),
					'e' when span.EndsWith("five") => (5, true),
					'x' when span.EndsWith("six") => (6, true),
					'n' when span.EndsWith("seven") => (7, true),
					't' when span.EndsWith("eight") => (8, true),
					'e' when span.EndsWith("nine") => (9, true),
					_ => (ones, false),
				};
				if (success) break;

				span = span[..^1];
			}

			part2 += tens + ones;
		}

		return (part1.ToString(), part2.ToString());
	}
}
