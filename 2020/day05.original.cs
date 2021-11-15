namespace AdventOfCode;

public class Day_2020_05_Original : Day
{
	public override int Year => 2020;
	public override int DayNumber => 5;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var ids = input.GetLines()
			.Select(s => s
				.Select(c => c == 'B' || c == 'R')
				.Select((b, idx) => (b, idx))
				.Aggregate(0, (i, b) => i | ((b.b ? 1 : 0) << (s.Length - b.idx - 1))))
			.OrderBy(x => x)
			.ToArray();

		PartA = ids.Last()
			.ToString();

		var potentials = ids
			.Window(2)
			.Where(a => a[1] - a[0] == 2)
			.ToArray();

		if (potentials.Length == 1)
			PartB = (potentials[0][0] + 1).ToString();
	}
}
