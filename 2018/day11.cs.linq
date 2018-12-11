<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var serialNumber = 6303;
var cells = new int[301, 301];

for (int x = 1; x <= 300; x++)
{
	var rackId = x + 10;

	for (int y = 1; y <= 300; y++)
	{
		var powerLevel = (rackId * y + serialNumber) * rackId;
		cells[x, y] = (powerLevel % 1000) / 100 - 5;
	}
}

(
	from x in Enumerable.Range(1, 298)
	from y in Enumerable.Range(1, 298)
	select (x, y, sum: (
		from x2 in Enumerable.Range(x, 3)
		from y2 in Enumerable.Range(y, 3)
		select cells[x2, y2]).Sum())
)
	.OrderByDescending(x => x.sum)
	.First()
	.Dump("Part A");

(
	from size in Enumerable.Range(1, 30)
	from x in Enumerable.Range(1, 301 - size)
	from y in Enumerable.Range(1, 301 - size)
	select (x, y, size, sum: (
		from x2 in Enumerable.Range(x, size)
		from y2 in Enumerable.Range(y, size)
		select cells[x2, y2]).Sum())
)
	.OrderByDescending(x => x.sum)
	.First()
	.Dump("Part B");
