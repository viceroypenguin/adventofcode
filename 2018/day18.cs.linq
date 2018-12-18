<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

char[][] input;
DumpContainer dumper;

void DumpMap() => dumper.UpdateContent(
	string.Join(Environment.NewLine,
		input.Select(s => string.Join("", s.AsEnumerable())).Concat(new[] { "" })));

string GetInputState() =>
	new string(input.SelectMany(l => l).ToArray());

void Main()
{
	input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day18.input.txt"))
		.Select(s => s.ToArray())
		.ToArray();

	dumper = new DumpContainer().Dump();
	DumpMap();

	for (int i = 0; i < 10; i++)
	{
		DoIteration();
		DumpMap();
	}

	var cellTypes = input.SelectMany(l => l)
		.GroupBy(c => c)
		.ToDictionary(c => c.Key, c => c.Count());
	(cellTypes['|'] * cellTypes['#']).Dump("Part A");

	var maxIter = 1_000_000_000;
	var flag = false;
	var seenStates = new Dictionary<string, int>();
	for (int i = 10; i < maxIter; i++)
	{
		DoIteration();
		DumpMap();

		if (!flag)
		{
			var state = GetInputState();
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

	cellTypes = input.SelectMany(l => l)
		.GroupBy(c => c)
		.ToDictionary(c => c.Key, c => c.Count());
	(cellTypes['|'] * cellTypes['#']).Dump("Part B");
}

private void DoIteration()
{
	input = Enumerable.Range(0, input.Length)
		.Select(x => Enumerable.Range(0, input[0].Length)
			.Select(y => GetNewValue(x, y))
			.ToArray())
		.ToArray();
}

char GetNewValue(int x, int y)
{
	var neighbors = (
		from x2 in Enumerable.Range(x - 1, 3)
		where x2 >= 0 && x2 < input.Length
		from y2 in Enumerable.Range(y - 1, 3)
		where y2 >= 0 && y2 < input[0].Length
		where x2 != x || y2 != y
		group new { } by input[x2][y2])
		.ToDictionary(c => c.Key, c => c.Count());

	switch (input[x][y])
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
