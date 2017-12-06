<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day06.input.txt"))
	.Split()
	.Where(x => !string.IsNullOrWhiteSpace(x))
	.Select(x => Convert.ToInt32(x))
	.ToList();

var history = new List<int[]>
{
	input.ToArray(),
};

while (true)
{
	var minElement =
		input.Select((x, i) => new { x, i, })
			.OrderByDescending(x => x.x)
			.ThenBy(x => x.i)
			.First();

	var allInc = minElement.x / input.Count;
	var extraInc = minElement.x % input.Count;

	input[minElement.i] = 0;
	for (int i = 1; i <= extraInc; i++)
		input[(minElement.i + i) % input.Count] += allInc + 1;
	for (int i = extraInc + 1; i <= input.Count; i++)
		input[(minElement.i + i) % input.Count] += allInc;

	var oldInput = history
		.Select((old, idx) => new { idx, isMatch = old.Zip(input, (o, i) => o == i).All(b => b), })
		.Where(x => x.isMatch)
		.FirstOrDefault();
	if (oldInput != null)
	{
		history.Count.Dump("Part A");
		(history.Count - oldInput.idx).Dump("Part B");
		break;
	}
	
	history.Add(input.ToArray());
}

