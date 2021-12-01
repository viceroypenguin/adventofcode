using System.Collections;

namespace AdventOfCode;

public class Day_2021_01_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		PartA = input.GetLines()
			.Select(l => Convert.ToInt32(l))
			.Window(2)
			.Where(x => x.Last() > x.First())
			.Count()
			.ToString();

		PartB = input.GetLines()
			.Select(l => Convert.ToInt32(l))
			.Window(3)
			.Select(x => x.Sum())
			.Window(2)
			.Where(x => x.Last() > x.First())
			.Count()
			.ToString();
	}
}
