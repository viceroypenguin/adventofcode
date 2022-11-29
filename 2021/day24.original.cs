namespace AdventOfCode;

public class Day_2021_24_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 24;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var groups = input.GetLines()
			.Batch(18)
			.Select(g =>
			{
				var a = Convert.ToInt32(g[4].Split()[^1]);
				var b = Convert.ToInt32(g[5].Split()[^1]);
				var c = Convert.ToInt32(g[15].Split()[^1]);
				return (a: a == 26, b, c);
			})
			.Index();

		var stack = new Stack<(int i, int c)>();
		var highDigits = new int[14];
		var lowDigits = new int[14];
		foreach (var (i, (a, b, c)) in groups)
		{
			if (a)
			{
				var (j, d) = stack.Pop();
				var diff = b + d;
				if (diff > 0)
				{
					highDigits[i] = 9;
					highDigits[j] = 9 - diff;

					lowDigits[j] = 1;
					lowDigits[i] = 1 + diff;
				}
				else
				{
					highDigits[j] = 9;
					highDigits[i] = 9 + diff;

					lowDigits[i] = 1;
					lowDigits[j] = 1 - diff;
				}
			}
			else
				stack.Push((i, c));
		}

		PartA = string.Join("", highDigits);
		PartB = string.Join("", lowDigits);
	}
}
