namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 01, CodeType.Fastest)]
public partial class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;
		foreach (var l in input.Span.EnumerateLines())
		{
			if (l.Length == 0)
				continue;

			var firstDigitIndex = l.IndexOfAnyInRange((byte)'0', (byte)'9');
			var lastDigitIndex = l.LastIndexOfAnyInRange((byte)'0', (byte)'9');

			var tens = (l[firstDigitIndex] - '0') * 10;
			var ones = l[lastDigitIndex] - '0';
			part1 += tens + ones;

			var span = l[..firstDigitIndex];
			while (span.Length >= 3)
			{
				(tens, var success) = span[0] switch
				{
					(byte)'o' when span.StartsWith("one"u8) => (10, true),
					(byte)'t' when span.StartsWith("two"u8) => (20, true),
					(byte)'t' when span.StartsWith("three"u8) => (30, true),
					(byte)'f' when span.StartsWith("four"u8) => (40, true),
					(byte)'f' when span.StartsWith("five"u8) => (50, true),
					(byte)'s' when span.StartsWith("six"u8) => (60, true),
					(byte)'s' when span.StartsWith("seven"u8) => (70, true),
					(byte)'e' when span.StartsWith("eight"u8) => (80, true),
					(byte)'n' when span.StartsWith("nine"u8) => (90, true),
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
					(byte)'e' when span.EndsWith("one"u8) => (1, true),
					(byte)'o' when span.EndsWith("two"u8) => (2, true),
					(byte)'e' when span.EndsWith("three"u8) => (3, true),
					(byte)'r' when span.EndsWith("four"u8) => (4, true),
					(byte)'e' when span.EndsWith("five"u8) => (5, true),
					(byte)'x' when span.EndsWith("six"u8) => (6, true),
					(byte)'n' when span.EndsWith("seven"u8) => (7, true),
					(byte)'t' when span.EndsWith("eight"u8) => (8, true),
					(byte)'e' when span.EndsWith("nine"u8) => (9, true),
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
