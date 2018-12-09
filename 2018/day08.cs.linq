<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

//var data = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2"
var data = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day08.input.txt"))
	.Trim()
	.Split()
	.Select(l => Convert.ToInt32(l))
	.ToList();

int index = 0;
int GetMetadataValue()
{
	var childNodes = data[index++];
	var metadataNodes = data[index++];

	return
		Enumerable.Range(0, childNodes)
			.Sum(_ => GetMetadataValue()) +
		Enumerable.Range(0, metadataNodes)
			.Sum(_ => data[index++]);
}

GetMetadataValue().Dump("Part A");

index = 0;
int GetNodeValue()
{
	var childNodes = data[index++];
	var metadataNodes = data[index++];

	if (childNodes == 0)
		return Enumerable.Range(0, metadataNodes)
			.Sum(_ => data[index++]);

	var nodes = Enumerable.Range(0, childNodes)
		.Select(_ => GetNodeValue())
		.ToList();

	return Enumerable.Range(0, metadataNodes)
		.Select(_ => data[index++])
		.Sum(i => i <= nodes.Count
			? nodes[i - 1]
			: 0);
}

GetNodeValue().Dump("Part B");
