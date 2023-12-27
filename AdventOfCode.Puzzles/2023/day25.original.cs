using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 25, CodeType.Original)]
public partial class Day_25_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var connections = new Dictionary<string, List<string>>();
		foreach (var line in input.Lines)
		{
			var l = line.Split();
			var from = l[0][..^1];

			foreach (var to in l.Skip(1))
			{
				connections.GetOrAdd(from, _ => new())
					.Add(to);
				connections.GetOrAdd(to, to => new())
					.Add(from);
			}
		}

		var part1 = 0;
		var keys = connections.Keys.ToList();
		foreach (var key in keys.Skip(1))
		{
			var setSize = GetComponentSize(connections, keys[0], key);
			if (setSize > 0)
			{
				part1 = setSize * (keys.Count - setSize);
				break;
			}
		}

		return (part1.ToString(), string.Empty);
	}

	private static int GetComponentSize(
		Dictionary<string, List<string>> connections, string start, string end)
	{
		var flows = new Dictionary<(string from, string to), int>();
		var numFlows = 0;
		while (true)
		{
			var componentSize = GetComponentSize(connections, flows, start, end);
			if (componentSize == 0)
				numFlows++;
			else if (numFlows == 3)
				return componentSize;
			else
				break;
		}
		return 0;
	}

	private static int GetComponentSize(
		Dictionary<string, List<string>> connections,
		Dictionary<(string from, string to), int> flows,
		string start, string end)
	{
		var from = new Dictionary<string, string>()
		{
			[start] = start,
		};

		var steps = 0;

		var queue = new Queue<string>();
		queue.Enqueue(start);

		while (queue.TryDequeue(out var current) && !from.ContainsKey(end))
		{
			steps++;

			var list = connections[current];
			foreach (var dest in connections.Keys)
			{
				var rate = (list.Contains(dest) ? 1 : 0) - flows.GetValueOrDefault((current, dest));
				if (rate > 0 && !from.ContainsKey(dest))
				{
					from[dest] = current;
					queue.Enqueue(dest);
				}
			}
		}

		if (!from.ContainsKey(end))
			return steps;

		var cur = end;
		while (cur != start)
		{
			CollectionsMarshal.GetValueRefOrAddDefault(
				flows, (from[cur], cur), out _) += 1;
			CollectionsMarshal.GetValueRefOrAddDefault(
				flows, (cur, from[cur]), out _) -= 1;

			cur = from[cur];
		}

		return 0;
	}
}
