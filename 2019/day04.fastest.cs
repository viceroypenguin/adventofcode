namespace AdventOfCode;

public unsafe class Day_2019_04_Fastest : Day
{
	public override int Year => 2019;
	public override int DayNumber => 4;
	public override CodeType CodeType => CodeType.Fastest;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var cur = stackalloc byte[8];
		var flag = false;
		var prevChar = cur[0] = (byte)(input[0] - 0x30);
		SetChar(ref cur[1], ref flag, ref prevChar, input[1]);
		SetChar(ref cur[2], ref flag, ref prevChar, input[2]);
		SetChar(ref cur[3], ref flag, ref prevChar, input[3]);
		SetChar(ref cur[4], ref flag, ref prevChar, input[4]);
		SetChar(ref cur[5], ref flag, ref prevChar, input[5]);
		var curAsLong = (ulong*)cur;

		var rangeEnd = stackalloc byte[8];
		flag = false;
		prevChar = rangeEnd[0] = (byte)(input[7] - 0x30);
		SetChar(ref rangeEnd[1], ref flag, ref prevChar, input[8]);
		SetChar(ref rangeEnd[2], ref flag, ref prevChar, input[9]);
		SetChar(ref rangeEnd[3], ref flag, ref prevChar, input[10]);
		SetChar(ref rangeEnd[4], ref flag, ref prevChar, input[11]);
		SetChar(ref rangeEnd[5], ref flag, ref prevChar, input[12]);
		var endAsLong = (ulong*)rangeEnd;

		int part1Count = 0, part2Count = 0;
		while (true)
		{
			var (a, b) = CheckNumber(cur);
			part1Count += a;
			part2Count += b;

			IncreaseNumber(cur);
			if (*curAsLong == *endAsLong)
				break;
		}

		PartA = part1Count.ToString();
		PartB = part2Count.ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void SetChar(ref byte chr, ref bool flag, ref byte prevChar, byte input)
	{
		if (flag)
		{
			chr = prevChar;
		}
		else
		{
			var tmp = (byte)(input - 0x30);
			if (tmp < prevChar)
			{
				flag = true;
				chr = prevChar;
			}
			else
				prevChar = chr = tmp;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private (int, int) CheckNumber(byte* cur)
	{
		var digits = stackalloc byte[10];
		for (int i = 0; i < 10; i++)
			digits[i] = 0;

		for (int i = 0; i < 6; i++)
			digits[cur[i]]++;

		int hasMatch = 0, hasDouble = 0;
		for (int i = 0; i < 10; i++)
		{
			if (digits[i] == 2) hasDouble = 1;
			if (digits[i] >= 2) hasMatch = 1;
		}
		return (hasMatch, hasDouble);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private void IncreaseNumber(byte* cur)
	{
		for (int i = 5; i >= 0; i--)
		{
			// increment doesn't wrap to the next number
			cur[i]++;

			for (int j = i + 1; j <= 5; j++)
				cur[j] = cur[i];

			if (cur[i] < 0x0a)
				break;
		}
	}
}
