namespace AdventOfCode;

public class Day_2017_17_Fastest : Day
{
	public override int Year => 2017;
	public override int DayNumber => 17;
	public override CodeType CodeType => CodeType.Fastest;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day17.c
		int key = 0;
		for (int i = 0; i < input.Length && input[i] >= '0'; i++)
			key = key * 10 + input[i] - '0';

		var position = 0;
		// Find the index of the 2017th insertion
		for (int i = 1; i <= 2017; i++)
			position = (position + key) % i + 1;

		// Reverse the simulation to find the value which follows
		int nextValue, nextPosition = (position + 1) % 2017;
		for (nextValue = 2017; position != nextPosition; nextValue--)
		{
			if (position < nextPosition)
				nextPosition--;
			if ((position -= key + 1) < 0)
				position += nextValue;
		}

		PartA = nextValue.ToString();

		var position1 = position = 0;
		// loop runs in O(log i)
		for (var i = 0; i < 50_000_000; position++)
		{
			if (position == 1)
				position1 = i;

			// use n * (n + 1) concept to skip processing every item in loop
			var skip = (i - position) / key + 1;
			position += skip * (key + 1) - 1;
			position %= (i += skip);
		}

		PartB = position1.ToString();
	}
}
