namespace AdventOfCode;

public class Day_2019_06_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 6;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var directOrbits = input.GetLines()
			.Select(s => s.Split(')'))
			.Select(s => (orbiter: s[1], orbited: s[0]))
			.ToList();

		var lookup1 = directOrbits.ToDictionary(s => s.orbiter);
		var lookup2 = directOrbits.ToLookup(s => s.orbited);

		PartA = directOrbits
			.Select(s =>
			{
				var count = 1;
				while (true)
				{
					if (!lookup1.TryGetValue(s.orbited, out var o))
						return count;
					count++;
					s = o;
				}
			})
			.Sum()
			.ToString();

		var visited = new Dictionary<string, int>();
		var (_, steps) = SuperEnumerable.TraverseBreadthFirst(
				(orbiter: lookup1["YOU"].orbited, steps: -1),
				o =>
				{
					if (visited.ContainsKey(o.orbiter))
						return Array.Empty<(string, int)>();

					visited[o.orbiter] = o.steps;

					var tmp = lookup2[o.orbiter]
						.Select(r => (r.orbiter, o.steps + 1));
					if (lookup1.TryGetValue(o.orbiter, out var x))
						tmp = tmp.Append((x.orbited, o.steps + 1));
					return tmp;
				})
			.FirstOrDefault(x => x.orbiter == "SAN");
		PartB = steps.ToString();
	}
}
