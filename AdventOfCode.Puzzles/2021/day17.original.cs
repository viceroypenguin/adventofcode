using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 17, CodeType.Original)]
public partial class Day_17_Original : IPuzzle
{
	[GeneratedRegex(@"target area: x=(\d+)..(\d+), y=(-\d+)..(-\d+)")]
	private static partial Regex TargetRegex();

	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var match = TargetRegex().Match(input.Text);
		var x1 = Convert.ToInt32(match.Groups[1].Value);
		var x2 = Convert.ToInt32(match.Groups[2].Value);
		var y1 = Convert.ToInt32(match.Groups[3].Value);
		var y2 = Convert.ToInt32(match.Groups[4].Value);

		IEnumerable<int> GetCandidatesForY(int y) =>
			// start at t = 0
			Enumerable.Range(0, 1000)
				// what is the velocity at time t?
				.Select(t => y - t)
				// start with y = 0, t = 0
				// return every position y and time t
				// using the velocity to adjust y
				.Scan((py: 0, t: 0), (py, vy) => (py.py + vy, py.t + 1))
				// stop when we get past y1
				.TakeWhile(x => x.py >= y1)
				// and exclude points before we get to y2
				.Where(x => x.py <= y2)
				// we only care about the original vy and
				// which times are valid for that vy
				.Select(x => x.t);

		// search every possible y1
		var yCandidates = Enumerable.Range(y1, (-y1 * 2) + 2)
			// get valid times for this vy
			.SelectMany(vy => GetCandidatesForY(vy).Select(t => (vy, t)))
			// group by the times
			.ToLookup(x => x.t, x => x.vy);

		// min vx is root of quadratic n * (n + 1) / 2
		var minVX = (int)Math.Ceiling(-0.5 + Math.Sqrt(0.25 + (2 * x1)));
		var maxVX = x2 + 1;

		// for each valid time
		var velocities = yCandidates
			// search the vx space for any vx
			// that lands in the box for the time t
			.SelectMany(g => Enumerable.Range(minVX, maxVX - minVX + 1)
				// start at time 0 and go to time t
				.Where(vx => Enumerable.Range(0, g.Key)
					// vx is decreasing to 0
					// sum all the vx
					.Sum(t => Math.Max(vx - t, 0))
					// is the sum between x1 and x2
					.Between(x1, x2))
				// we have valid vx and vy, pair them up
				.SelectMany(vx => g, (vx, vy) => (vx, vy)))
			// in case we accidentally match the vx/vy
			// more than once...
			.Distinct()
			.ToList();

		// how high can we get?
		var maxVY = velocities
			.Select(x => x.vy)
			.Max();
		var part1 = (maxVY * (maxVY + 1) / 2).ToString();

		// how many pairs did we get?
		var part2 = velocities.Count.ToString();

		return (part1, part2);
	}
}
