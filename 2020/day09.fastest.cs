namespace AdventOfCode;

public class Day_2020_09_Fastest : Day
{
	public override int Year => 2020;
	public override int DayNumber => 9;
	public override CodeType CodeType => CodeType.Fastest;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var span = new ReadOnlySpan<byte>(input);
		Span<long> arr = stackalloc long[input.Length / 8];
		int maxIndex = 0;

		for (int i = 0; i < span.Length;)
		{
			var (x, y) = span[i..].AtoL();
			arr[maxIndex++] = x;
			i += y + 1; // plus 1 to ignore next char
		}

		var invalidNumber = 0L;
		for (int i = 25; i < maxIndex; i++)
		{
			for (int j = i - 25; j < i; j++)
				for (int k = j + 1; k < i; k++)
					if (arr[j] + arr[k] == arr[i])
						goto found_match;

			PartA = (invalidNumber = arr[i]).ToString();
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

		PartB = (min + max).ToString();
	}
}
