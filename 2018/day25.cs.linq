<Query Kind="Statements">
  <NuGetReference>MedallionPriorityQueue</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>Medallion.Collections</Namespace>
  <Namespace>MoreLinq</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day25.input.txt"))
	.Select(l => l.Split(','))
	.Select(l => (
		x: Convert.ToInt32(l[0]),
		y: Convert.ToInt32(l[1]),
		z: Convert.ToInt32(l[2]),
		t: Convert.ToInt32(l[3])))
	.ToArray();

var constellations =
	input.Select((_, i) => -i - 1).ToArray();
var constellationNumber = 0;

int ManhattanDistance((int x, int y, int z, int t) a, (int x, int y, int z, int t) b) =>
	Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) + Math.Abs(a.t - b.t);

for (int i = 0; i < input.Length; i++)
{
	for (int j = 0; j < input.Length; j++)
	{
		if (i == j) continue;
		if (ManhattanDistance(input[i], input[j]) <= 3)
		{
			if (constellations[i] < 0)
			{
				if (constellations[j] < 0)
				{
					constellations[i] = constellationNumber;
					constellations[j] = constellationNumber;
					constellationNumber++;
				}
				else
					constellations[i] = constellations[j];
			}
			else
			{
				if (constellations[j] < 0)
					constellations[j] = constellations[i];
				else if (constellations[i] != constellations[j])
				{
					var oldId = constellations[j];
					var newId = constellations[i];
					for (int k = 0; k < input.Length; k++)
						if (constellations[k] == oldId)
							constellations[k] = newId;
				}
			}
		}
	}
}

constellations
	.Distinct()
	.Count()
	.Dump("Part A");
