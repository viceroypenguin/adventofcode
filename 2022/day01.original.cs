using System.Collections;

namespace AdventOfCode;

public class Day_2022_01_Original : Day
{
	public override int Year => 2022;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		PartA = input.GetLines(StringSplitOptions.None)
			.Split(string.Empty)
			.Select(g => g.Select(int.Parse).Sum())
			.Max().ToString();

		PartB = input.GetLines(StringSplitOptions.None)
			.Split(string.Empty)
			.Select(g => g.Select(int.Parse).Sum())
			.PartialSort(3, OrderByDirection.Descending)
			.Sum()
			.ToString();
	}
}
