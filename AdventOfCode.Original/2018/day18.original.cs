namespace AdventOfCode;

public class Day_2018_18_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 18;
	public override CodeType CodeType => CodeType.Original;

	char[][] map;
	string GetmapState() =>
		new string(map.SelectMany(l => l).ToArray());

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		map = input.GetLines()
			.Select(s => s.ToArray())
			.ToArray();

		for (int i = 0; i < 10; i++)
		{
			DoIteration();
		}

		var cellTypes = map.SelectMany(l => l)
			.GroupBy(c => c)
			.ToDictionary(c => c.Key, c => c.Count());
		Dump('A', cellTypes['|'] * cellTypes['#']);

		var maxIter = 1_000_000_000;
		var flag = false;
		var seenStates = new Dictionary<string, int>();
		for (int i = 10; i < maxIter; i++)
		{
			DoIteration();

			if (!flag)
			{
				var state = GetmapState();
				if (seenStates.ContainsKey(state))
				{
					var cycleLength = i - seenStates[state];
					i = maxIter - ((maxIter - i) % cycleLength);
					flag = true;
				}
				else
					seenStates[state] = i;
			}
		}

		cellTypes = map.SelectMany(l => l)
			.GroupBy(c => c)
			.ToDictionary(c => c.Key, c => c.Count());
		Dump('B', cellTypes['|'] * cellTypes['#']);
	}

	private void DoIteration()
	{
		map = Enumerable.Range(0, map.Length)
			.Select(x => Enumerable.Range(0, map[0].Length)
				.Select(y => GetNewValue(x, y))
				.ToArray())
			.ToArray();
	}

	char GetNewValue(int x, int y)
	{
		var neighbors = (
			from x2 in Enumerable.Range(x - 1, 3)
			where x2 >= 0 && x2 < map.Length
			from y2 in Enumerable.Range(y - 1, 3)
			where y2 >= 0 && y2 < map[0].Length
			where x2 != x || y2 != y
			group new { } by map[x2][y2])
			.ToDictionary(c => c.Key, c => c.Count());

		switch (map[x][y])
		{
			case '.':
				return neighbors.ContainsKey('|') && neighbors['|'] >= 3
					? '|' : '.';

			case '|':
				return neighbors.ContainsKey('#') && neighbors['#'] >= 3
					? '#' : '|';

			case '#':
				return neighbors.ContainsKey('|') && neighbors['|'] >= 1 &&
					neighbors.ContainsKey('#') && neighbors['#'] >= 1
					? '#' : '.';

			default:
				throw new InvalidOperationException();
		}

	}
}
