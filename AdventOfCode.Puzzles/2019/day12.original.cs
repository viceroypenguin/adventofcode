using static AdventOfCode.Common.Helpers;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 12, CodeType.Original)]
public partial class Day_12_Original : IPuzzle
{
	[GeneratedRegex("<x=(?<x>-?\\d+), y=(?<y>-?\\d+), z=(?<z>-?\\d+)>")]
	private static partial Regex PositionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = PositionRegex();

		var positions = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => (
				x: Convert.ToInt32(m.Groups["x"].Value),
				y: Convert.ToInt32(m.Groups["y"].Value),
				z: Convert.ToInt32(m.Groups["z"].Value)))
			.ToArray();

		var moonsX = positions.Select(m => (position: m.x, velocity: 0)).ToArray();
		var moonsY = positions.Select(m => (position: m.y, velocity: 0)).ToArray();
		var moonsZ = positions.Select(m => (position: m.z, velocity: 0)).ToArray();

		var moons = new[] { moonsX, moonsY, moonsZ };

		var cycleLengths = Enumerable.Range(0, moons.Length).Select(_ => -1).ToArray();

		var part1 = string.Empty;
		for (var i = 1; cycleLengths.Any(x => x < 0); i++)
		{
			moons = moons.Select(Timestep).ToArray();

			for (var d = 0; d < moons.Length; d++)
				if (cycleLengths[d] == -1
					&& Enumerable.Range(0, moons[d].Length)
						.All(m => moons[d][m].velocity == 0))
					cycleLengths[d] = i * 2;

			if (i == 1000)
				part1 = Enumerable.Range(0, moons[0].Length)
					.Sum(m =>
					{
						var (pos, vel) = Enumerable.Range(0, moons.Length)
							.Aggregate(
								(pos: 0, vel: 0),
								(agg, i) =>
								{
									var (dpos, dvel) = moons[i][m];
									return (agg.pos + Math.Abs(dpos), agg.vel + Math.Abs(dvel));
								});
						return pos * vel;
					})
					.ToString();
		}

		var part2 = cycleLengths
			.Aggregate(1L, (l, cl) => lcm(l, cl))
			.ToString();

		return (part1, part2);
	}

	static (int position, int velocity)[] Timestep(
		(int position, int velocity)[] current) =>
		Enumerable.Range(0, current.Length)
			.Select(pi =>
			{
				var (pos, vel) = current[pi];
				foreach (var si in Enumerable.Range(0, current.Length).Where(si => si != pi))
					vel += Math.Sign(current[si].position - pos);

				return (pos + vel, vel);
			})
			.ToArray();
}
