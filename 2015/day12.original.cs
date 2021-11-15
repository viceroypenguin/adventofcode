namespace AdventOfCode;

public class Day_2015_12_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 12;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var str = input.GetString();

		var regex = new Regex("[,:[](-?\\d+)");
		Dump('A',
			regex.Matches(str)
				.OfType<Match>()
				.Select(c => c.Groups[1])
				.Select(c => c.Value)
				.Select(c => Convert.ToInt32(c))
				.Sum());

		var redsRegex = new Regex("{[^{}]*(((?<before>{)[^{}]*)+((?<-before>})[^{}]*)+)*(?(before)(?!))[^{}]*:\"red\"[^{}]*(((?<before>{)[^{}]*)+((?<-before>})[^{}]*)+)*(?(before)(?!))[^{}]*}");
		str = redsRegex.Replace(str, "");

		Dump('B',
			regex.Matches(str)
				.OfType<Match>()
				.Select(c => c.Groups[1])
				.Select(c => c.Value)
				.Select(c => Convert.ToInt32(c))
				.Sum());
	}
}
