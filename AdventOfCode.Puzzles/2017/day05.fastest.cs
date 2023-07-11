namespace AdventOfCode;

public class Day_2017_05_Fastest : Day
{
	public override int Year => 2017;
	public override int DayNumber => 5;
	public override CodeType CodeType => CodeType.Fastest;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	protected unsafe override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var nums1 = stackalloc int[input.Length / 4];
		var nums2 = stackalloc int[input.Length / 4];
		var count = 0;
		for (int i = 0, neg = 0, n = 0; i < input.Length; i++)
		{
			var c = input[i];
			if (c == '\n')
			{
				nums1[count] = nums2[count] = neg == 1 ? -n : n;
				count++;
				n = neg = 0;
			}
			else if (c == '-')
				neg = 1;
			else if (c >= '0')
				n = n * 10 + c - '0';
		}

		{
			var start = nums1;
			var end = &start[count];
			var steps = 0;
			for (var p = start; p >= start && p < end; steps++)
				p += (*p)++;
			PartA = steps.ToString();
		}

		{
			var start = nums2;
			var end = &start[count];
			var steps = 0;
			for (var p = start; p >= start && p < end; steps++)
			{
				var j = *p;
				*p += -(((j - 3) >> 31) << 1) - 1;
				p += j;
			}
			PartB = steps.ToString();
		}
	}
}
