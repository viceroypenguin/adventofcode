<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var numGenerations = 150;

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day12.input.txt"));
var initialStateStr = lines[0].Replace("initial state: ", "");

var bits = new BitArray(initialStateStr.Length + numGenerations * 2 + 8);
foreach (var (c, i) in initialStateStr.Select((c, i) => (c, i)))
	bits[i + numGenerations + 2] = c == '#';

int BuildInt(IList<bool> arr)
{
	var ret = 0;
	for (int i = 0; i < arr.Count; i++)
		ret |= (arr[i] ? 1 << i : 0);
	return ret;
}

var map = lines
	.Skip(2)
	.Select(l => l.Split(new[] { " => " }, StringSplitOptions.None))
	.ToDictionary(
		l => BuildInt(l[0].Select(c => c == '#').ToList()),
		l => l[1] == "#");

bool GetValue(IList<bool> arr)
{
	var key = BuildInt(arr);
	if (!map.TryGetValue(key, out var ret))
		return false;
	return ret;
}

void DumpArray() =>
	new string(bits.OfType<bool>().Select(b => b ? '#' : '.').ToArray()).Dump();

for (int gen = 1; gen <= 20; gen++)
{
	var nextBits = new BitArray(bits.Count);
	foreach (var (w, i) in bits
		.OfType<bool>()
		.Window(5)
		.Select((x, i) => (x, i)))
	{
		nextBits[i + 2] = GetValue(w);
	}

	bits = nextBits;
}

Enumerable.Range(0, bits.Count)
	.Select(i => (idx: i - (numGenerations + 2), val: bits[i]))
	.Where(x => x.val)
	.Sum(x => x.idx)
	.Dump("Part A");
	
for (int gen = 20; gen <= numGenerations; gen++)
{
	var nextBits = new BitArray(bits.Count);
	foreach (var (w, i) in bits
		.OfType<bool>()
		.Window(5)
		.Select((x, i) => (x, i)))
	{
		nextBits[i + 2] = GetValue(w);
	}

	bits = nextBits;
}

Enumerable.Range(0, bits.Count)
	.Select(i => (idx: i - (numGenerations + 2), val: bits[i]))
	.Where(x => x.val)
	.Sum(x => x.idx + (50_000_000_000L - numGenerations - 1))
	.Dump("Part B");
