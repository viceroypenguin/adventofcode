using System.Collections;

namespace AdventOfCode;

public class Day_2021_06_Fastest : Day
{
	public override int Year => 2021;
	public override int DayNumber => 6;
	public override CodeType CodeType => CodeType.Fastest;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		Span<long> fish = stackalloc long[9];

		var span = new ReadOnlySpan<byte>(input);
		for (int i = 0; i < span.Length;)
		{
			var (value, numChars) = span[i..].AtoI();
			fish[value]++;
			i += numChars + 1;
		}

		// comment and idea copied from @ensce (https://www.reddit.com/r/adventofcode/comments/r9z49j/2021_day_6_solutions/hnfhi24/)
		// we will model a circular shift register, with an additional feedback:
		//       0123456           78 
		//   ┌──[       ]─<─(+)───[  ]──┐
		//   └──────>────────┴─────>────┘

		for (int i = 0; i < 80; i++)
			fish[(i + 7) % 9] += fish[i % 9];

		var totalFish = 0L;
		foreach (var f in fish)
			totalFish += f;
		PartA = totalFish.ToString();


		for (int i = 80; i < 256; i++)
			fish[(i + 7) % 9] += fish[i % 9];

		totalFish = 0L;
		foreach (var f in fish)
			totalFish += f;
		PartB = totalFish.ToString();
	}
}
