namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 20, CodeType.Original)]
public class Day_20_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var mapWidth = input.Bytes.AsSpan().IndexOf((byte)'\n') + 1;

		var distances = new List<(string from, string to, int distance)>();
		var seenPositions = new HashSet<int>();
		var pos = mapWidth * 2;
		for (; pos < input.Bytes.Length; pos++)
		{
			if (seenPositions.Contains(pos))
				continue;

			if (input.Bytes[pos] != '.')
				continue;

			var destinations = new HashSet<(string, int)>();
			TraverseMap(
				input.Bytes,
				mapWidth,
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
					input.Bytes,
					mapWidth,
					start,
					static _ => { },
					(dest, _, dist) => distances.Add((source, dest, dist)));
			}
		}

		var map = distances
			.Where(x => x.from != x.to)
			.ToLookup(x => x.from, x => (x.to, x.distance));

		string DoPart1()
		{
			var totalDistance = SuperEnumerable.GetShortestPathCost<string, int>(
				"AAe",
				(l, d) => map[l].Select(x => (x.to, x.distance + d)),
				"ZZe");
			return totalDistance.ToString();
		}

		string DoPart2()
		{
			var totalDistance = SuperEnumerable.GetShortestPathCost<(string dest, int level), int>(
				(dest: "AAe", level: 0),
				(l, d) => map[l.dest]
					.Select(t =>
						t.to[..2] == l.dest[..2]
							? (Element: (t.to, level: t.to[^1] == 'e' ? l.level + 1 : l.level - 1), d + 1)
							: (Element: (t.to, l.level), d + t.distance))
					.Where(t =>
						t.Element.to[^1] == 'i'
						|| (t.Element.to != "ZZe" && t.Element.to != "AAe") == (t.Element.level != 0)),
				("ZZe", 0));
			return totalDistance.ToString();
		}

		return (DoPart1(), DoPart2());
	}

	private void TraverseMap(
		byte[] input,
		int mapWidth,
		int start,
		Action<int> visitor,
		Action<string, int, int> destinationVisitor)
	{
		var seen = new HashSet<int>();
		SuperEnumerable
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
			{
				yield break;
			}
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
					? new string([otherLetter, letter, isExternal ? 'e' : 'i'])
					: new string([letter, otherLetter, isExternal ? 'e' : 'i']);
				destinationVisitor(destination, prev, distance - 1);
			}
		}
	}
}
