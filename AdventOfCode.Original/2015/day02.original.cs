namespace AdventOfCode;

public class Day_2015_02_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 2;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"(?<l>\d+)x(?<w>\d+)x(?<h>\d+)", RegexOptions.Compiled);
		var boxes = input.GetLines()
			.Select(l => regex.Match(l))
			.Select(m => new[]
			{
					Convert.ToInt32(m.Groups["l"].Value),
					Convert.ToInt32(m.Groups["w"].Value),
					Convert.ToInt32(m.Groups["h"].Value),
			}.OrderBy(l => l).ToArray())
			.ToList();

		var totalWrappingPaper =
			boxes
				.Select(b => new[] { b[0] * b[1], b[0] * b[2], b[1] * b[2], }.OrderBy(a => a).ToArray())
				.Select(a => 3 * a[0] + 2 * a[1] + 2 * a[2])
				.Sum();

		var totalRibbonLength =
			boxes
				.Select(b => b[0] * b[1] * b[2] + 2 * b[0] + 2 * b[1])
				.Sum();

		Dump('A', totalWrappingPaper);
		Dump('B', totalRibbonLength);
	}
}
