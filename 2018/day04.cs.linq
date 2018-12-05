<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

const int BeginShift = 1;
const int FallsAsleep = 2;
const int WakesUp = 3;

var regex = new Regex(
	@"^\[(?<date>\d{4}-\d{2}-\d{2} \d{2}\:\d{2})\] ((Guard #(?<id>\d+) begins shift)|(?<asleep>falls asleep)|(?<awake>wakes up))",
	RegexOptions.Compiled | RegexOptions.ExplicitCapture);

var sleeps = File
	.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day04.input.txt"))
	.Select(l => l.Trim())
	.Select(l => regex.Match(l))
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
		var id = shift.First().id.Value;
		return shift
			.Skip(1)
			.Batch(2)
			.Select(s => new
			{
				id = id,
				start = s.First().date,
				end = s.Last().date,
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

(guardMostSleep * minutes.Single(g => g.id == guardMostSleep).minute)
	.Dump("Part A");

minutes
	.OrderByDescending(g => g.times)
	.Select(g => g.id * g.minute)
	.First()
	.Dump("Part B");
	