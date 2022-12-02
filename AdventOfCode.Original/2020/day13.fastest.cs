using static AdventOfCode.Helpers;

namespace AdventOfCode;

public class Day_2020_13_Fastest : Day
{
	public override int Year => 2020;
	public override int DayNumber => 13;
	public override CodeType CodeType => CodeType.Fastest;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var span = new ReadOnlySpan<byte>(input);
		var (myTime, i) = span.AtoI();
		i++;

		var minTimeAfter = (id: 0, timeAfter: int.MaxValue);
		var busNumber = -1;
		long time = 1, increment = 1;
		while (i < span.Length)
		{
			busNumber++;
			if (span[i] == 'x')
			{
				i += 2;
				continue;
			}

			var (id, x) = span[i..].AtoI();
			i += x + 1;

			var valueAfter = id - (myTime % id);
			if (valueAfter < minTimeAfter.timeAfter)
				minTimeAfter = (id, valueAfter);

			if (busNumber == 0)
			{
				time = increment = id;
				continue;
			}

			var modValue = id - (busNumber % id);
			while (time % id != modValue)
				time += increment;
			increment = lcm(increment, id);
		}

		PartA = (minTimeAfter.id * minTimeAfter.timeAfter).ToString();
		PartB = time.ToString();
	}
}
