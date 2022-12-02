namespace AdventOfCode;

public class Day_2016_04_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 4;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"(?<name>[a-z-]+)-(?<id>\d+)[[](?<checksum>\w{5})[]]", RegexOptions.Compiled);

		var validNames =
			input.GetLines()
				.Select(x => regex.Match(x))
				.Select(x => new
				{
					name = x.Groups["name"].Value,
					id = Convert.ToInt32(x.Groups["id"].Value),
					checksum = x.Groups["checksum"].Value,
				})
				.Where(x =>
				{
					var checksum = string.Join("", x.name
						.GroupBy(
							c => c,
							(c, _) => new { c, cnt = _.Count(), })
						.Where(c => c.c != '-')
						.OrderByDescending(c => c.cnt)
						.ThenBy(c => c.c)
						.Select(c => c.c)
						.Take(5));
					return checksum == x.checksum;
				})
				.ToList();

		Dump('A', validNames
			.Sum(x => x.id));

		Dump('B', validNames
			.Select(x => new
			{
				x.name,
				x.id,
				newName = string.Join("", x.name
					.Select(c =>
					{
						if (c == '-') return ' ';
						return (char)((c - 'a' + x.id) % 26 + 'a');
					})),
			})
			.First(x => x.newName.StartsWith("north"))
			.id);
	}
}
