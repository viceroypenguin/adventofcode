namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 04, CodeType.Original)]
public partial class Day_04_Original : IPuzzle
{
	[GeneratedRegex("(?<name>[a-z-]+)-(?<id>\\d+)[[](?<checksum>\\w{5})[]]", RegexOptions.Compiled)]
	private static partial Regex RoomRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = RoomRegex();

		var validNames =
			input.Lines
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

		var partA = validNames.Sum(x => x.id);

		var partB = validNames
			.Select(x => new
			{
				x.name,
				x.id,
				newName = string.Join("", x.name
					.Select(c => c == '-' ? ' ' : (char)(((c - 'a' + x.id) % 26) + 'a')))
			})
			.First(x => x.newName.StartsWith("north"))
			.id;

		return (partA.ToString(), partB.ToString());
	}
}
