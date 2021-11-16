namespace AdventOfCode;

public class Day_2020_23_Original : Day
{
	public override int Year => 2020;
	public override int DayNumber => 23;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var cups = input.Where(x => x > 0x30).Select(x => x - 0x30).ToList();
		var maxValue = cups.Max();

		var curIndex = 0;

		static int incrementIndex(int value, int maxValue) =>
			value == maxValue - 1 ? 0 : value + 1;
		static int decrementValue(int value, int maxValue) =>
			value == 1 ? maxValue : value - 1;

		for (int _ = 1; _ <= 100; _++)
		{
			var value = decrementValue(cups[curIndex], maxValue);
			var i1 = incrementIndex(curIndex, cups.Count);
			var i2 = incrementIndex(i1, cups.Count);
			var i3 = incrementIndex(i2, cups.Count);

			var (v1, v2, v3) = (cups[i1], cups[i2], cups[i3]);
			while (v1 == value || v2 == value || v3 == value)
				value = decrementValue(value, maxValue);

			var destIdx = incrementIndex(i3, cups.Count);
			while (cups[destIdx] != value)
			{
				cups[i1] = cups[destIdx];
				i1 = incrementIndex(i1, cups.Count);
				destIdx = incrementIndex(destIdx, cups.Count);
			}
			cups[i1] = value;
			i1 = incrementIndex(i1, cups.Count);
			cups[i1] = v1;
			i1 = incrementIndex(i1, cups.Count);
			cups[i1] = v2;
			i1 = incrementIndex(i1, cups.Count);
			cups[i1] = v3;

			curIndex = incrementIndex(curIndex, cups.Count);
		}

		curIndex = 0;
		while (cups[curIndex] != 1)
			curIndex++;
		curIndex = incrementIndex(curIndex, cups.Count);

		Span<char> output = stackalloc char[cups.Count];
		var ptr = output;
		while (cups[curIndex] != 1)
		{
			ptr[0] = (char)(cups[curIndex] + 0x30);
			ptr = ptr[1..];
			curIndex = incrementIndex(curIndex, cups.Count);
		}

		PartA = new string(output);
	}
}
