namespace AdventOfCode;

public class Day_2016_20_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 20;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var max = uint.MaxValue;
		var regex = new Regex(@"(?<from>\d+)-(?<to>\d+)", RegexOptions.Compiled);

		var blocked =
			input.GetLines()
				.Select(s => regex.Match(s))
				.Select(m => new { from = Convert.ToUInt32(m.Groups["from"].Value), to = Convert.ToUInt32(m.Groups["to"].Value), })
				.OrderBy(b => b.from)
				.ToList();

		var first = 0u;
		foreach (var b in blocked)
		{
			if (b.from > first)
				break;

			first = Math.Max(b.to + 1, first);
		}

		Dump('A', first);

		first = 0u;
		var allowed = blocked.Take(0).ToList();
		foreach (var b in blocked)
		{
			if (b.from > first)
			{
				allowed.Add(new { from = first, to = b.from - 1 });
			}

			if (b.to == max)
			{
				break;
			}

			first = Math.Max(Math.Max(b.to, b.to + 1), first);
		}

		Dump('B', allowed.Select(b => (int)(b.to - b.from + 1)).Sum());
	}
}
