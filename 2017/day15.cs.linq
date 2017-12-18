<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var regex = new Regex(@"^Generator (?<gen>\w) starts with (?<init>\d+)$", RegexOptions.Compiled);
var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day15.input.txt"))
	.Select(l => regex.Match(l))
	.ToDictionary(
		m => m.Groups["gen"].Value,
		m => (ulong)Convert.ToInt32(m.Groups["init"].Value));

Enumerable.Range(0, 40_000_000)
	.Scan(
		(a:input["A"],b:input["B"]),
		(state, _) => ((state.a * 16807) % 2147483647, (state.b * 48271) % 2147483647))
	.Skip(1)
	.Where(s => (ushort)s.a == (ushort)s.b)
	.Count()
	.Dump("Part A");

var aGenerator = MoreEnumerable
	.Generate(input["A"], a => (a * 16807) % 2147483647)
	.Where(a => a % 4 == 0);
var bGenerator = MoreEnumerable
	.Generate(input["B"], b => (b * 48271) % 2147483647)
	.Where(b => b % 8 == 0);
	
aGenerator.Zip(bGenerator, (a, b) => (a, b))
	.Take(5_000_000)
	.Where(s => (ushort)s.a == (ushort)s.b)
	.Count()
	.Dump("Part B");
