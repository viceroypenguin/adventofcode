namespace AdventOfCode;

public class Day_2019_01_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		static IEnumerable<int> fuelValues(int start) =>
			MoreEnumerable.Generate(start, s => Math.Max(s / 3 - 2, 0))
				.Skip(1)
				.TakeWhile(s => s != 0);

		var numbers = input
			.GetLines()
			.Select(s => Convert.ToInt32(s))
			.ToList();

		PartA = numbers
			.Select(s => fuelValues(s).First())
			.Sum()
			.ToString();

		PartB = numbers
			.Select(s => fuelValues(s).Sum())
			.Sum()
			.ToString();
	}
}
