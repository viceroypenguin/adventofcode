namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 05, CodeType.Original)]
public partial class Day_05_Original : IPuzzle
{
	[GeneratedRegex(@"(\w+)-to-(\w+)")]
	private static partial Regex SegmentNameRegex();
	[GeneratedRegex(@"\d+")]
	private static partial Regex IntRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var segments = input.Lines
			.Split(string.Empty)
			.ToList();

		var mapNameRegex = SegmentNameRegex();
		var intRegex = IntRegex();

		var maps = segments
			.Skip(1)
			.Select(s =>
			{
				var match = mapNameRegex.Match(s[0]);
				var from = match.Groups[1].Value;
				var to = match.Groups[2].Value;

				var ranges = s.Skip(1)
					.Select(r => intRegex.Matches(r)
						.OfType<Match>()
						.Select(m => long.Parse(m.Value))
						.ToList())
					.Select(r => (
						adjust: r[0] - r[1],
						source: r[1],
						to: r[1] + r[2] - 1))
					.OrderBy(x => x.source)
					.ToList();

				return new
				{
					from,
					to,
					ranges,
				};
			})
			.ToLookup(x => x.from);

		var seeds = intRegex.Matches(segments[0][0])
			.OfType<Match>()
			.Select(m => long.Parse(m.Value))
			.ToList();

		long ProcessSeed(long s)
		{
			var values = SuperEnumerable
				.TraverseBreadthFirst(
					(type: "seed", value: s),
					x => maps[x.type]
						.Select(y => (y.to, y.ranges
							.Where(z => x.value.Between(z.source, z.to))
							.Select(z => x.value + z.adjust)
							.FirstOrDefault(x.value))))
				.ToDictionary(x => x.type, x => x.value);
			return values["location"];
		}

		var locations = seeds
			.Select(ProcessSeed)
			.ToList();

		var part1 = locations.Min();

		var part2 = seeds
			.Batch(2)
			.Select(b =>
			{
				var type = "seed";
				var ranges = new List<(long from, long to)>() { (from: b.First(), to: b.First() + b.Last() - 1), };

				while (type != "location")
				{
					var nextType = maps[type].First();

					var nextRanges = new List<(long from, long to)>();
					foreach (var r in ranges)
					{
						var (from, to) = r;
						foreach (var mapRanges in nextType.ranges)
						{
							if (from > mapRanges.to)
								continue;

							if (to < mapRanges.source)
							{
								nextRanges.Add((from, to));
								(from, to) = (0, 0);
								break;
							}

							if (from < mapRanges.source)
							{
								nextRanges.Add((from + mapRanges.adjust, mapRanges.source - 1 + mapRanges.source));
								from = mapRanges.source;
							}

							var end = Math.Min(to, mapRanges.to);
							nextRanges.Add((from + mapRanges.adjust, end + mapRanges.adjust));

							if (to > mapRanges.to)
							{
								from = mapRanges.to + 1;
							}
							else
							{
								(from, to) = (0, 0);
								break;
							}
						}

						if (from != 0 && to != 0)
							nextRanges.Add((from, to));
					}

					nextRanges.Sort();

					ranges = nextRanges;
					type = nextType.to;
				}

				return ranges[0].from;
			})
			.Min();

		return (part1.ToString(), part2.ToString());
	}
}
