namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 04, CodeType.Original)]
public partial class Day_04_Original : IPuzzle
{
	[GeneratedRegex(
		@"^\[(?<date>\d{4}-\d{2}-\d{2} \d{2}\:\d{2})\] ((Guard #(?<id>\d+) begins shift)|(?<asleep>falls asleep)|(?<awake>wakes up))",
		RegexOptions.ExplicitCapture
	)]
	private static partial Regex GuardShiftRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		const int BeginShift = 1;
		const int FallsAsleep = 2;
		const int WakesUp = 3;

		var sleeps = input.Lines
			.Select(l => l.Trim())
			.Select(l => GuardShiftRegex().Match(l))
			.Select(m => new
			{
				date = Convert.ToDateTime(m.Groups["date"].Value),
				id = m.Groups["id"].Success ? Convert.ToInt32(m.Groups["id"].Value) : default(int?),
				activity =
					m.Groups["id"].Success ? BeginShift :
					m.Groups["asleep"].Success ? FallsAsleep :
					m.Groups["awake"].Success ? WakesUp :
					-1,
			})
			.OrderBy(x => x.date)
			.Segment(x => x.activity == BeginShift)
			.SelectMany(shift =>
			{
				var id = shift[0].id.Value;
				return shift
					.Skip(1)
					.Batch(2)
					.Select(s => new
					{
						id,
						start = s[0].date,
						end = s[^1].date,
					});
			})
			.ToLookup(s => s.id);

		var guardMostSleep = sleeps
			.OrderByDescending(g => g.Sum(s => (s.end - s.start).TotalMinutes))
			.First()
			.Key;

		var minutes = sleeps
			.Select(g =>
			{
				var x = g
					.SelectMany(s => Enumerable.Range(s.start.Minute, (int)(s.end - s.start).TotalMinutes))
					.CountBy(i => i)
					.OrderByDescending(i => i.Value)
					.First();
				return new
				{
					id = g.Key,
					minute = x.Key,
					times = x.Value,
				};
			})
			.ToList();

		var part1 = guardMostSleep * minutes.Single(g => g.id == guardMostSleep).minute;

		var part2 = minutes
			.OrderByDescending(g => g.times)
			.Select(g => g.id * g.minute)
			.First();

		return (part1.ToString(), part2.ToString());
	}
}
