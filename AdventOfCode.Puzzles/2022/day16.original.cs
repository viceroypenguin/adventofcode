using System.Collections.Immutable;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 16, CodeType.Original)]
public partial class Day_16_Original : IPuzzle
{
	[GeneratedRegex(@"^Valve (?<id>\w+) has flow rate=(?<flow>\d+); tunnels? leads? to valves? ((?<dest>\w+)(, )?)+$", RegexOptions.ExplicitCapture)]
	private static partial Regex ValveRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = ValveRegex();
		var valves = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => (
				id: m.Groups["id"].Value,
				flow: int.Parse(m.Groups["flow"].Value),
				dest: m.Groups["dest"].Captures.Select(c => c.Value).ToList()))
			.ToDictionary(x => x.id, StringComparer.Ordinal);

		var distanceMap = BuildDistanceMap(valves);

		var allValves = ImmutableHashSet<(string id, int flow)>.Empty;
		foreach (var v in valves.Values
				.Where(v => v.flow > 0)
				.Select(v => (v.id, v.flow)))
		{
			allValves = allValves.Add(v);
		}

		var (part1, _) = DoPart1(
			distanceMap,
			[new("AA", allValves),],
			30);

		var (you, state) = DoPart1(
			distanceMap,
			[new("AA", allValves),],
			26);
		var (ele, _) = DoPart1(
			distanceMap,
			[new("AA", state.ClosedValves),],
			26);
		var part2 = you + ele;

		return (part1.ToString(), part2.ToString());
	}

	private static Dictionary<(string from, string to), int> BuildDistanceMap(
		Dictionary<string, (string id, int flow, List<string> tunnels)> valves)
	{
		var distanceMap = new Dictionary<(string from, string to), int>();
		foreach (var (key, _) in valves)
		{
			var map = SuperEnumerable.GetShortestPaths<string, int>(
				key,
				(f, c) => valves[f].tunnels.Select(t => (t, c + 1)));
			foreach (var kvp in map)
			{
				distanceMap[(key, kvp.Key)] = kvp.Value.cost;
			}
		}
		return distanceMap;
	}

	private sealed record State1(
		string Valve,
		ImmutableHashSet<(string id, int flow)> ClosedValves);
	private static (int maxPressure, State1 state) DoPart1(
		Dictionary<(string from, string to), int> distanceMap,
		List<State1> states,
		int timeRemaining)
	{
		var (valve, closedValves) = states[^1];
		if (timeRemaining <= 0) return (0, states[^1]);

		var remainingValves = closedValves
			.Select(v =>
			{
				var distance = distanceMap[(valve, v.id)];
				var timeOpen = timeRemaining - 1 - distance;
				return (v, t: timeOpen, p: v.flow * timeOpen);
			})
			.OrderByDescending(x => x.p)
			.ToList();

		var best = (pressure: 0, state: states[^1]);
		foreach (var (v, t, pressure) in remainingValves)
		{
			states.Add(new(v.id, closedValves.Remove(v)));
			var p = DoPart1(distanceMap, states, t);
			p = (p.maxPressure + pressure, p.state);
			if (p.maxPressure > best.pressure)
				best = p;
			states.RemoveAt(states.Count - 1);
		}

		return best;
	}
}
