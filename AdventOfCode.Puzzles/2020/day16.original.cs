namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 16, CodeType.Original)]
public class Day_16_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var segments = input.Lines
			.Segment(s => string.IsNullOrWhiteSpace(s))
			.ToArray();

		var rulesDescriptions = segments[0];
		var myTicket = segments[1].Last().Split(',').Select(int.Parse).ToArray();
		var otherTickets = segments[2].Skip(2).Select(l => l.Split(',').Select(int.Parse).ToArray()).ToArray();

		var rules = rulesDescriptions
			.Select(l => (
				name: l[..l.IndexOf(':')],
				values: Regex.Matches(l, @"(\d+)-(\d+)").OfType<Match>()
					.Select(m => (lo: int.Parse(m.Groups[1].Value), hi: int.Parse(m.Groups[2].Value)))
					.ToArray()))
			.ToArray();

		var validNumbers = rules.SelectMany(x => x.values).ToArray();
		var part1 = otherTickets
			.SelectMany(t => t)
			.Where(n => !validNumbers.Any(x => n.Between(x.lo, x.hi)))
			.Sum()
			.ToString();

		var validTickets = otherTickets
			.Where(t => t.All(n => validNumbers.Any(x => n.Between(x.lo, x.hi))))
			.ToArray();

		var candidates = rules
			.Select(r => (
				r.name,
				indexes: Enumerable.Range(0, validTickets[0].Length)
					.Where(i => validTickets.All(t => r.values.Any(v => t[i].Between(v.lo, v.hi))))
					.ToList()))
			.ToList();

		var map = new List<(string name, int index)>();
		while (map.Count < rules.Length)
		{
			var nextCandidate = candidates.First(c => c.indexes.Count == 1);
			var index = nextCandidate.indexes[0];
			map.Add((nextCandidate.name, index));
			candidates.RemoveAll(c => c.name == nextCandidate.name);
			foreach (var (_, indexes) in candidates)
				indexes.Remove(index);
		}

		var part2 = map
			.Where(r => r.name.StartsWith("departure"))
			.Select(r => myTicket[r.index])
			.Aggregate(1L, (l, r) => l * r)
			.ToString();

		return (part1, part2);
	}
}
