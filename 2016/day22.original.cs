namespace AdventOfCode;

public class Day_2016_22_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 22;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var regex = new Regex(@"/dev/grid/node-x(?<x>\d+)-y(?<y>\d+)\s+(?<total>\d+)T\s+(?<used>\d+)T\s+(?<avail>\d+)T\s+\d+%", RegexOptions.Compiled);

		var nodes =
			input.GetLines()
				.Skip(2)
				.Select(s => regex.Match(s))
				.Select(m => new
				{
					x = Convert.ToInt32(m.Groups["x"].Value),
					y = Convert.ToInt32(m.Groups["y"].Value),
					total = Convert.ToInt32(m.Groups["total"].Value),
					used = Convert.ToInt32(m.Groups["used"].Value),
					avail = Convert.ToInt32(m.Groups["avail"].Value),
				})
				.ToList();

		Dump('A',
			(
				from a in nodes
				where a.used != 0
				from b in nodes
				where a.x != b.x || a.y != b.y
				where a.used < b.avail
				select new { a, b }
			).Count());
	}
}
