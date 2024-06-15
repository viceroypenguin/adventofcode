namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 19, CodeType.Original)]
public partial class Day_19_Original : IPuzzle
{
	[GeneratedRegex("Each (?<type>\\w+) robot costs ((?<cost>\\d+ \\w+)( and )?)+.", RegexOptions.ExplicitCapture)]
	private static partial Regex RobotRegex();

	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var regex = RobotRegex();

		var blueprints = input.Lines
			.Select((bp, i) =>
			{
				var robots = regex.Matches(bp)
					.Select(m => (
						type: m.Groups["type"].Value,
						costs: m.Groups["cost"].Captures
							.Select(c => c.Value.Split())
							.Select(s => (n: int.Parse(s[0]), type: s[1]))
							.ToDictionary(s => s.type, s => s.n)))
					.ToDictionary(x => x.type, x => x.costs);

				return (id: i + 1, robots);
			})
			.ToList();

		var part1 = blueprints
			.Select(bp => bp.id * GetMaxGeodes(bp.robots, 24))
			.Sum()
			.ToString();

		var part2 = blueprints
			.Take(3)
			.Select(bp => GetMaxGeodes(bp.robots, 32))
			.Aggregate(1, (p, b) => p * b)
			.ToString();
		return (part1, part2);
	}

	private readonly record struct State(
		int Minute,
		int OreCount = 0,
		int ClayCount = 0,
		int ObsidianCount = 0,
		int GeodeCount = 0,
		int OreRobots = 1,
		int ClayRobots = 0,
		int ObsidianRobots = 0,
		int GeodeRobots = 0);

	private static int GetMaxGeodes(Dictionary<string, Dictionary<string, int>> robots, int minutes)
	{
		var yield = 0;
		var states = 0;

		var maxOreCost = robots.Values.Select(v => v.GetValueOrDefault("ore")).Max();
		var maxClayCost = robots.Values.Select(v => v.GetValueOrDefault("clay")).Max();
		var maxObsidianCost = robots.Values.Select(v => v.GetValueOrDefault("obsidian")).Max();

		var seen = new HashSet<State>();
		void GetMaxYield(State s)
		{
			states++;

			// if it's not even possible for us to beat yield in a perfect world...
			if (s.GeodeCount + (s.GeodeRobots * s.Minute) + (s.Minute * (s.Minute + 1) / 2) < yield)
				return;

			// reduce robots to increase state overlap
			// can only produce one 1 robot per minute, so no need for more material production than we can consume in a minute
			s = s with
			{
				OreRobots = Math.Min(s.OreRobots, maxOreCost),
				ClayRobots = Math.Min(s.ClayRobots, maxClayCost),
				ObsidianRobots = Math.Min(s.ObsidianRobots, maxObsidianCost),
			};

			// reduce ore to increase state overlap
			// only need ore to produce robots, so need for more material than we can consume before end of remaining time
			s = s with
			{
				OreCount = Math.Min(s.OreCount, (maxOreCost * s.Minute) - (s.OreRobots * (s.Minute - 1))),
				ClayCount = Math.Min(s.ClayCount, (maxClayCost * s.Minute) - (s.ClayRobots * (s.Minute - 1))),
				ObsidianCount = Math.Min(s.ObsidianCount, (maxObsidianCost * s.Minute) - (s.ObsidianRobots * (s.Minute - 1))),
			};

			// have we been here before?
			if (!seen.Add(s))
				return;

			// are we done? how many did we produce?
			if (s.Minute == 0)
			{
				yield = Math.Max(yield, s.GeodeCount);
				return;
			}

			if (s.OreRobots < maxOreCost)
			{
				var time = 1 + (robots["ore"]["ore"] - s.OreCount).DivRoundUp(s.OreRobots);
				if (time < s.Minute)
				{
					GetMaxYield(s with
					{
						Minute = s.Minute - time,
						OreCount = s.OreCount + (s.OreRobots * time) - robots["ore"]["ore"],
						ClayCount = s.ClayCount + (s.ClayRobots * time),
						ObsidianCount = s.ObsidianCount + (s.ObsidianRobots * time),
						GeodeCount = s.GeodeCount + (s.GeodeRobots * time),
						OreRobots = s.OreRobots + 1,
					});
				}
			}

			if (s.ClayCount < maxClayCost)
			{
				var time = 1 + (robots["clay"]["ore"] - s.OreCount).DivRoundUp(s.OreRobots);
				if (time < s.Minute)
				{
					GetMaxYield(s with
					{
						Minute = s.Minute - time,
						OreCount = s.OreCount + (s.OreRobots * time) - robots["clay"]["ore"],
						ClayCount = s.ClayCount + (s.ClayRobots * time),
						ObsidianCount = s.ObsidianCount + (s.ObsidianRobots * time),
						GeodeCount = s.GeodeCount + (s.GeodeRobots * time),
						ClayRobots = s.ClayRobots + 1,
					});
				}
			}

			if (s.ClayRobots > 0 && s.ObsidianRobots < maxObsidianCost)
			{
				var time = 1 + Math.Max(
					(robots["obsidian"]["ore"] - s.OreCount).DivRoundUp(s.OreRobots),
					(robots["obsidian"]["clay"] - s.ClayCount).DivRoundUp(s.ClayRobots));
				if (time < s.Minute)
				{
					GetMaxYield(s with
					{
						Minute = s.Minute - time,
						OreCount = s.OreCount + (s.OreRobots * time) - robots["obsidian"]["ore"],
						ClayCount = s.ClayCount + (s.ClayRobots * time) - robots["obsidian"]["clay"],
						ObsidianCount = s.ObsidianCount + (s.ObsidianRobots * time),
						GeodeCount = s.GeodeCount + (s.GeodeRobots * time),
						ObsidianRobots = s.ObsidianRobots + 1,
					});
				}
			}

			if (s.ObsidianRobots > 0)
			{
				var time = 1 + Math.Max(
					(robots["geode"]["ore"] - s.OreCount).DivRoundUp(s.OreRobots),
					(robots["geode"]["obsidian"] - s.ObsidianCount).DivRoundUp(s.ObsidianRobots));
				if (time < s.Minute)
				{
					GetMaxYield(s with
					{
						Minute = s.Minute - time,
						OreCount = s.OreCount + (s.OreRobots * time) - robots["geode"]["ore"],
						ClayCount = s.ClayCount + (s.ClayRobots * time),
						ObsidianCount = s.ObsidianCount + (s.ObsidianRobots * time) - robots["geode"]["obsidian"],
						GeodeCount = s.GeodeCount + (s.GeodeRobots * time),
						GeodeRobots = s.GeodeRobots + 1,
					});
				}
			}

			if (s.GeodeRobots > 0)
			{
				GetMaxYield(s with
				{
					Minute = 0,
					GeodeCount = s.GeodeCount + (s.GeodeRobots * s.Minute),
				});
			}
		}

		GetMaxYield(new(minutes));
		return yield;
	}
}
