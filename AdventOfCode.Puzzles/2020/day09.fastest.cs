namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 9, CodeType.Fastest)]
public class Day_09_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;
		Span<long> arr = stackalloc long[input.Bytes.Length / 8];
		int maxIndex = 0;

		for (int i = 0; i < span.Length;)
		{
			var (x, y) = span[i..].AtoL();
			arr[maxIndex++] = x;
			i += y + 1; // plus 1 to ignore next char
		}

		var invalidNumber = 0L;
		var part1 = string.Empty;
		for (int i = 25; i < maxIndex; i++)
		{
			for (int j = i - 25; j < i; j++)
				for (int k = j + 1; k < i; k++)
					if (arr[j] + arr[k] == arr[i])
						goto found_match;

			part1 = (invalidNumber = arr[i]).ToString();
			break;

found_match:
			;
		}

		int start = 0, end = 0;
		var sum = arr[0];
		while (true)
		{
			if (sum == invalidNumber)
				break;
			else if (sum > invalidNumber)
				sum -= arr[start++];
			else if (sum < invalidNumber)
				sum += arr[++end];
		}

		long min = arr[start], max = arr[start];
		for (int i = start + 1; i <= end; i++)
			(min, max) = (
				Math.Min(min, arr[i]),
				Math.Max(max, arr[i]));

		var part2 = (min + max).ToString();

		return (part1, part2);
	}
}
