namespace AdventOfCode;

public class Day_2020_25_Original : Day
{
	public override int Year => 2020;
	public override int DayNumber => 25;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var span = new ReadOnlySpan<byte>(input);
		var x = span.AtoI();
		var key1 = x.value;

		span = span[(x.numChars + 1)..];
		x = span.AtoI();
		var key2 = x.value;

		var loopSize = GetLoopSize(key1);
		var eKey = GetKey(key2, loopSize);

		PartA = eKey.ToString();
	}

	private static int GetLoopSize(int publicKey)
	{
		var sn = 7L;
		var value = 1L;
		int i = 0;

		while (value != publicKey)
		{
			value = (value * sn) % 20201227;
			i++;
		}
		return i;
	}

	private static int GetKey(int sn, int loopSize)
	{
		var value = 1L;
		for (int i = 0; i < loopSize; i++)
			value = (value * sn) % 20201227;

		return (int)value;
	}
}
