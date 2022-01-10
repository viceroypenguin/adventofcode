using System.Collections;

namespace AdventOfCode;

public class Day_2021_01_Fastest : Day
{
	public override int Year => 2021;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Fastest;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		Span<int> numbers = stackalloc int[4];
		numbers[0] = numbers[1] = numbers[2] = numbers[3] = int.MaxValue;
		int numA = 0, numB = 0;

		var span = new ReadOnlySpan<byte>(input);
		for (int i = 0; i < span.Length;)
		{
			var (value, numChars) = span[i..].AtoI();
			i += numChars + 1;

			(numbers[0], numbers[1], numbers[2], numbers[3]) =
				(numbers[1], numbers[2], numbers[3], value);

			if (numbers[3] > numbers[2]) numA++;
			if (numbers[3] > numbers[0]) numB++;
		}

		PartA = numA.ToString();
		PartB = numB.ToString();
	}
}
