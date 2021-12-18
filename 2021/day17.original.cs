namespace AdventOfCode;

public class Day_2021_17_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 17;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var match = Regex.Match(input.GetString(), @"target area: x=(\d+)..(\d+), y=(-\d+)..(-\d+)");
		var x1 = Convert.ToInt32(match.Groups[1].Value);
		var x2 = Convert.ToInt32(match.Groups[2].Value);
		var y1 = Convert.ToInt32(match.Groups[3].Value);
		var y2 = Convert.ToInt32(match.Groups[4].Value);

		IEnumerable<(int y, int t)> GetCandidatesForY(int y) =>
			Enumerable.Range(0, 1000)
				.Select(t => (t, vy: y - t))
				.Scan((py: 0, t: 0), (py, t) => (py.py + t.vy, py.t + 1))
				.TakeWhile(x => x.py >= y1)
				.Where(x => x.py <= y2);

		var yCandidates = Enumerable.Range(y1, -y1 * 2 + 2)
			.SelectMany(GetCandidatesForY)
			.ToLookup(x => x.t, x => x.y);

		var minVX = (int)Math.Ceiling(-0.5 + Math.Sqrt(0.25 + 2 * x1));
		var maxVX = x2 + 1;

		var velocities = yCandidates
			.SelectMany(g => Enumerable.Range(minVX, maxVX - minVX + 1)
				.Where(vx => Enumerable.Range(0, g.Key)
					.Sum(t => Math.Max(vx - t, 0))
					.Between(x1, x2))
				.SelectMany(vx => g, (vx, vy) => (vx, vy)))
			.Distinct()
			.ToList();

		var maxVY = velocities
			.Select(x => x.vy)
			.Max();
		PartA = (maxVY * (maxVY + 1) / 2).ToString();

		PartB = velocities.Count.ToString();
	}
}
