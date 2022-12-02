namespace AdventOfCode;

public class Day_2015_25_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 25;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		Func<ulong, ulong> step = x => (x * 252533) % 33554393;

		var row = 2947;
		var col = 3029;

		Func<int, int> totalNums = n => n * (n - 1) / 2;

		var stepCount = totalNums(row + col) - (row - 1);

		var num = 20151125UL;
		foreach (var x in Enumerable.Range(0, stepCount - 1))
			num = step(num);

		Dump('A', num);
	}
}
