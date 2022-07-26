namespace AdventOfCode;

public class Day_2018_01_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var changes = input.GetLines()
			.Select(l => Convert.ToInt32(l))
			.ToList();

		Dump('A', changes.Sum());

		var seen = new HashSet<int>();
		Dump('B',
			MoreEnumerable.Repeat(changes)
				.Scan((acc, next) => acc + next)
				.First(f =>
				{
					if (seen.Contains(f))
						return true;
					seen.Add(f);
					return false;
				}));
	}
}
