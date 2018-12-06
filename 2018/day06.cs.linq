<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var coordinates = File
	.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day06.input.txt"))
	.Select(s => s.Split(new[] { ", " }, StringSplitOptions.None))
	.Select(s => s.Select(i => Convert.ToInt32(i)).ToArray())
	.Select(s => (x: s[0], y: s[1]))
	.ToArray();

var maxX = coordinates.Max(c => c.x);
var maxY = coordinates.Max(c => c.y);

var grid = new int[maxX + 2, maxY + 2];
var safeCount = 0;

for (int x = 0; x <= maxX + 1; x++)
	for (int y = 0; y <= maxY + 1; y++)
	{
		var distances = coordinates
			.Select((c, i) => (i, dist: Math.Abs(c.x - x) + Math.Abs(c.y - y)))
			.OrderBy(c => c.dist)
			.ToArray();

		grid[x, y] = distances[1].dist != distances[0].dist
			? distances[0].i
			: -1;
			
		if (distances.Sum(c => c.dist) < 10000)
			safeCount++;
	}

var excluded = new List<int>();
var counts = Enumerable.Range(-1, coordinates.Length + 1).ToDictionary(i => i, _ => 0);

for (int x = 0; x <= maxX + 1; x++)
	for (int y = 0; y <= maxY + 1; y++)
	{
		if (x == 0 || y == 0 ||
			x == maxX + 1 || y == maxY + 1)
		{
			excluded.Add(grid[x, y]);
		}
		counts[grid[x, y]] += 1;
	}

excluded = excluded.Distinct().ToList();
counts
	.Where(kvp => !excluded.Contains(kvp.Key))
	.OrderByDescending(kvp => kvp.Value)
	.Dump("Part A");

safeCount.Dump("Part B");
