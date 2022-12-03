namespace AdventOfCode;

public class Day_2022_03_Original : Day
{
	public override int Year => 2022;
	public override int DayNumber => 3;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		PartA = input.GetLines()
			.Select(x => x.Batch(x.Length / 2))
			.Select(x => x.First().Intersect(x.Last()))
			.SelectMany(x => x)
			.Select(x => char.IsLower(x) ? x - 'a' + 1 : x - 'A' + 27)
			.Sum()
			.ToString();

		PartB = input.GetLines()
			.Batch(3)
			.Select(x => x[0].Intersect(x[1])
				.Intersect(x[2]))
			.SelectMany(x => x)
			.Select(x => char.IsLower(x) ? x - 'a' + 1 : x - 'A' + 27)
			.Sum()
			.ToString();
	}
}
