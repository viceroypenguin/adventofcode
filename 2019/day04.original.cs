namespace AdventOfCode;

public class Day_2019_04_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 4;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var range = input.GetString().Split('-');
		var min = Convert.ToInt32(range[0]);
		var max = Convert.ToInt32(range[1]);

		PartA = Enumerable.Range(min, max - min + 1)
			.Where(i => i.ToString().Window(2).All(x => x[0] <= x[1]))
			.Where(i => i.ToString().GroupAdjacent(c => c).Any(g => g.Count() >= 2))
			.Count()
			.ToString();

		PartB = Enumerable.Range(min, max - min + 1)
			.Where(i => i.ToString().Window(2).All(x => x[0] <= x[1]))
			.Where(i => i.ToString().GroupAdjacent(c => c).Any(g => g.Count() == 2))
			.Count()
			.ToString();
	}
}
