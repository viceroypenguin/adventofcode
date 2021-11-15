namespace AdventOfCode;

public class Day_2015_01_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var level = 0;
		var basement = 0;
		foreach ((var c, var i) in input.Select((c, i) => (c, i + 1)))
		{
			level += ((byte)'(' - c) * 2 + 1;
			if (basement == 0 && level == -1)
				basement = i;
		}

		Dump('A', level);
		Dump('B', basement);
	}
}
