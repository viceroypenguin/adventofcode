using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_20_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 20;
	public override CodeType CodeType => CodeType.Original;

	private byte[] input;
	private int mapWidth;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		this.input = input;

		mapWidth = 0;
		for (; mapWidth < input.Length; mapWidth++)
			if (input[mapWidth] == '\n')
				break;
		mapWidth++;

		var distances = new List<(string from, string to, int distance)>();
		var seenPositions = new HashSet<int>();
		var pos = mapWidth * 2;
		for (; pos < input.Length; pos++)
		{
			if (seenPositions.Contains(pos))
				continue;

			if (input[pos] != '.')
				continue;

			var destinations = new HashSet<(string, int)>();
			TraverseMap(
				pos,
				i => seenPositions.Add(i),
				(destination, pos, _) => destinations.Add((destination, pos)));
			foreach (var (d, _) in destinations)
			{
				if (d[^1] == 'e')
					distances.Add((d, d[..2] + "i", 1));
				else
					distances.Add((d, d[..2] + "e", 1));
			}

			foreach (var (source, start) in destinations)
			{
				TraverseMap(
					start,
					static _ => { },
					(dest, _, dist) => distances.Add((source, dest, dist)));
			}
		}

		var map = distances
			.Where(x => x.from != x.to)
			.ToLookup(x => x.from, x => (x.to, x.distance));

		{
			var totalDistance = new Dictionary<string, int>();
			var pq = new PriorityQueue<string, int>();
			pq.Enqueue("AAe", 0);
			while (!totalDistance.ContainsKey("ZZe"))
			{
				pq.TryPeek(out var dest, out var dist);
				pq.Dequeue();

				totalDistance[dest] = dist;
				pq.EnqueueRange(map[dest]
					.Where(d => !totalDistance.ContainsKey(d.to))
					.Select(d => (d.to, d.distance + dist)));
			}

			PartA = totalDistance["ZZe"].ToString();
		}

		{
			var totalDistance = new Dictionary<(string dest, int level), int>();
			var pq = new PriorityQueue<(string dest, int level), int>();
			pq.Enqueue(("AAe", 0), 0);
			while (!totalDistance.ContainsKey(("ZZe", 0)))
			{
				pq.TryPeek(out var p, out var dist);
				pq.Dequeue();

				totalDistance[p] = dist;

				pq.EnqueueRange(map[p.dest]
					.Select(t =>
						t.to[..2] == p.dest[..2]
							? (Element: (t.to, level: t.to[^1] == 'e' ? p.level + 1 : p.level - 1), dist + 1)
							: (Element: (t.to, p.level), dist + t.distance))
					.Where(t => 
						t.Element.to[^1] == 'i'
						|| (t.Element.to != "ZZe" && t.Element.to != "AAe") == (t.Element.level != 0))
					.Where(d => !totalDistance.ContainsKey(d.Element)));
			}

			PartB = totalDistance[("ZZe", 0)].ToString();
		}
	}

	private void TraverseMap(
		int start,
		Action<int> visitor,
		Action<string, int, int> destinationVisitor)
	{
		var seen = new HashSet<int>();
		MoreEnumerable
			.TraverseBreadthFirst(
				(start, start, 0),
				Traverse)
			.Consume();

		IEnumerable<(int, int, int)> Traverse((int, int, int) _)
		{
			var (cur, prev, distance) = _;

			if (seen.Contains(cur))
				yield break;
			seen.Add(cur);

			if (input[cur] == '.')
			{
				visitor(cur);

				yield return (cur - mapWidth, cur, distance + 1); // n
				yield return (cur + mapWidth, cur, distance + 1); // s
				yield return (cur - 1, cur, distance + 1); // e
				yield return (cur + 1, cur, distance + 1); // w
			}
			else if (input[cur] == '#')
				yield break;
			else
			{
				var isExternal = cur < mapWidth * 3;
				isExternal = isExternal || cur > input.Length - (mapWidth * 3);
				if (!isExternal)
				{
					var col = cur % mapWidth;
					if (col <= 3 || col >= mapWidth - 4)
						isExternal = true;
				}

				var letter = (char)input[cur];
				var otherLetter = (char)input[cur + cur - prev];
				var destination =
					prev > cur
					? new string(new[] { otherLetter, letter, isExternal ? 'e' : 'i' })
					: new string(new[] { letter, otherLetter, isExternal ? 'e' : 'i' });
				destinationVisitor(destination, prev, distance - 1);
			}
		}
	}
}
