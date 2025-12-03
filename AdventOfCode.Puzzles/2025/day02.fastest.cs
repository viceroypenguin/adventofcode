namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 02, CodeType.Fastest)]
public partial class Day_02_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0L;
		var part2 = 0L;

		var span = input.Span;
		foreach (var rangeRange in span.Split((byte)','))
		{
			var rangeSpan = span[rangeRange];
			var (start, length) = rangeSpan.AtoL();
			var (end, maxLength) = rangeSpan[(length + 1)..].AtoL();

			part1 += SumRepeatsInRangeOfLength(start, end, 2);
			part2 += SumRepeatsInRange(start, end, maxLength);
		}

		return (part1.ToString(), part2.ToString());
	}

	private static long SumSequence(long x, long y) =>
		y > x ? (y + x - 1) * (y - x) / 2 : 0;

	private static long CalculatePattern(long @base, int r) =>
		(@base, r) switch
		{
			(10, 2) => 11,
			(10, 3) => 111,
			(10, 4) => 1111,
			(10, 5) => 11111,
			(10, 6) => 111111,
			(10, 7) => 1111111,
			(10, 8) => 11111111,
			(10, 9) => 111111111,
			(10, 10) => 1111111111,
			(100, 2) => 101,
			(100, 3) => 10101,
			(100, 4) => 1010101,
			(100, 5) => 101010101,
			(100, 6) => 10101010101,
			(100, 7) => 1010101010101,
			(100, 8) => 101010101010101,
			(100, 9) => 10101010101010101,
			(100, 10) => 1010101010101010101,
			(1000, 2) => 1001,
			(1000, 3) => 1001001,
			(1000, 4) => 1001001001,
			(1000, 5) => 1001001001001,
			(1000, 6) => 1001001001001001,
			(1000, 7) => 1001001001001001001,
			(10000, 2) => 10001,
			(10000, 3) => 100010001,
			(10000, 4) => 1000100010001,
			(10000, 5) => 10001000100010001,
			(100000, 2) => 100001,
			(100000, 3) => 10000100001,
			(100000, 4) => 1000010000100001,
			_ => 0,
		};

	private static long SumRepeatsOfLength(long n, int numDigits)
	{
		var result = 0L;
		var @base = 1L;
		var base10 = 10L;

		while (
			CalculatePattern(base10, numDigits) is not 0 and var pattern
			&& pattern * @base < n
		)
		{
			var min = Math.Min((n + pattern - 1) / pattern, base10);
			result += pattern * SumSequence(@base, min);

			@base = base10;
			base10 *= 10;
		}

		return result;
	}

	private static long SumRepeatsInRangeOfLength(long a, long b, int numDigits) =>
		SumRepeatsOfLength(b + 1, numDigits) - SumRepeatsOfLength(a, numDigits);

	private static long SumRepeatsInRange(long a, long b, int numDigits)
	{
		var result = 0L;

		foreach (var p in Primes)
		{
			if (p > numDigits)
				break;

			result += SumRepeatsInRangeOfLength(a, b, p);

			foreach (var q in Primes)
			{
				if (q >= p || p * q > numDigits)
					break;

				result -= SumRepeatsInRangeOfLength(a, b, p * q);
			}
		}

		return result;
	}

	private static ReadOnlySpan<int> Primes => [2, 3, 5, 7];
}
