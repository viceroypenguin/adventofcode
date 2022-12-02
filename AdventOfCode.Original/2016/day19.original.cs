namespace AdventOfCode;

public class Day_2016_19_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 19;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		Func<int, int> nextPowerOfTwo = (n) =>
		{
			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;

			return n + 1;
		};

		Func<int, int> partAElfHoldingPresents = (n) =>
			(n * 2) % nextPowerOfTwo(n) + 1;

		Func<int, int> nextPowerOfThree = (n) =>
		{
			int x = 3;
			for (; x < n; x *= 3)
				;
			return x;
		};

		Func<int, int> partBElfHoldingPresents = (n) =>
		{
			var roundUp = nextPowerOfThree(n);
			var roundDown = roundUp / 3;
			if (n <= roundDown * 2) return n - roundDown;
			if (n == roundUp) return n;
			return (n * 2) % roundUp;
		};

		var num = Convert.ToInt32(input.GetString());

		Dump('A', partAElfHoldingPresents(num));
		Dump('B', partBElfHoldingPresents(num));
	}
}
