using System.Collections;

namespace AdventOfCode;

public class Day_2022_01_Fastest : Day
{
	public override int Year => 2022;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Fastest;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		Span<int> numbers = stackalloc int[3];
		var elf = 0;

		var span = new ReadOnlySpan<byte>(input);
		for (int i = 0; i < span.Length;)
		{
			if (span[i] == '\n')
			{
				if (elf > numbers[2])
					numbers[2] = elf;
				if (elf > numbers[1])
					(numbers[1], numbers[2]) =
						(elf, numbers[1]);
				if (elf > numbers[0])
					(numbers[0], numbers[1]) =
						(elf, numbers[0]);

				elf = 0;
				i++;
			}

			var (value, numChars) = span[i..].AtoI();
			i += numChars + 1;

			elf += value;
		}

		PartA = numbers[0].ToString();
		PartB = (numbers[0] + numbers[1] + numbers[2]).ToString();
	}
}
