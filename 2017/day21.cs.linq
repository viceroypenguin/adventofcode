<Query Kind="Statements">
  <Namespace>System.Collections.Specialized</Namespace>
</Query>

// [ 0, 1 ]
// [ 2, 3 ]
var rotateSize2 = new[] { 2, 0, 3, 1, };
var flipSize2 = new[] { 1, 0, 3, 2, };

// [ 0, 1, 2 ]
// [ 3, 4, 5 ]
// [ 6, 7, 8 ]
var rotateSize3 = new[] { 6, 3, 0, 7, 4, 1, 8, 5, 2, };
var flipSize3 = new[] { 2, 1, 0, 5, 4, 3, 8, 7, 6 };

bool[] ParseString(string str) =>
	str.Split('/')
		.SelectMany(l => l.Select(c => c == '#'))
		.ToArray();

var isSize3 = 1 << 31;

BitVector32 ConvertArray(bool[] arr)
{
	var bv = new BitVector32();
	foreach (var x in arr.Select((b, i) => (b, i)))
		bv[1 << x.i] = x.b;
	bv[isSize3] = arr.Length == 9;
	return bv;
}

IList<BitVector32> ConvertOutput(bool[] arr)
{
	if (arr.Length == 9)
		return new[] { ConvertArray(arr), };

	return new[]
	{
		ConvertArray(new[]{ arr[ 0], arr[ 1], arr[ 4], arr[ 5], }),
		ConvertArray(new[]{ arr[ 2], arr[ 3], arr[ 6], arr[ 7], }),
		ConvertArray(new[]{ arr[ 8], arr[ 9], arr[12], arr[13], }),
		ConvertArray(new[]{ arr[10], arr[11], arr[14], arr[15], }),
	};
}

BitVector32 FlipState(BitVector32 state)
{
	var bv = new BitVector32();
	var flipArr = state[isSize3] ? flipSize3 : flipSize2;
	foreach (var x in flipArr.Select((old, @new) => (old, @new)))
		bv[1 << x.@new] = state[1 << x.old];
	bv[isSize3] = state[isSize3];
	return bv;
}

BitVector32 RotateState(BitVector32 state)
{
	var bv = new BitVector32();
	var rotateArr = state[isSize3] ? rotateSize3 : rotateSize2;
	foreach (var x in rotateArr.Select((old, @new) => (old, @new)))
		bv[1 << x.@new] = state[1 << x.old];
	bv[isSize3] = state[isSize3];
	return bv;
}

IEnumerable<BitVector32> GetStates(BitVector32 initial)
{
	yield return initial;
	for (int i = 0; i < 3; i++)
	{
		initial = RotateState(initial);
		yield return initial;
	}
	initial = FlipState(initial);
	yield return initial;
	for (int i = 0; i < 3; i++)
	{
		initial = RotateState(initial);
		yield return initial;
	}
}

var rules = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day21.input.txt"))
	.Select(s => s.Split(' '))
	.ToDictionary(
		x => ConvertArray(ParseString(x[0])),
		x => ConvertOutput(ParseString(x[2])));

int CountEnabled(BitVector32 state)
{
	var size = state[isSize3] ? 9 : 4;
	return Enumerable.Range(0, size)
		.Where(i => state[1 << i])
		.Count();
}

IList<BitVector32> TransitionState(BitVector32 state)
{
	foreach (var s in GetStates(state))
		if (rules.ContainsKey(s))
		{
			return rules[s];
		}
	throw new InvalidOperationException();
}

IEnumerable<BitVector32> DoConvert3To2(BitVector32[] bvs)
{
	// [ 0, 1, 2 ] [ 0, 1, 2 ]
	// [ 3, 4, 5 ] [ 3, 4, 5 ]
	// [ 6, 7, 8 ] [ 6, 7, 8 ]
	// [ 0, 1, 2 ] [ 0, 1, 2 ]
	// [ 3, 4, 5 ] [ 3, 4, 5 ]
	// [ 6, 7, 8 ] [ 6, 7, 8 ]
	yield return ConvertArray(new[] { bvs[0][1 << 0], bvs[0][1 << 1], bvs[0][1 << 3], bvs[0][1 << 4], });
	yield return ConvertArray(new[] { bvs[0][1 << 2], bvs[1][1 << 0], bvs[0][1 << 5], bvs[1][1 << 3], });
	yield return ConvertArray(new[] { bvs[1][1 << 1], bvs[1][1 << 2], bvs[1][1 << 4], bvs[1][1 << 5], });
	yield return ConvertArray(new[] { bvs[0][1 << 6], bvs[0][1 << 7], bvs[2][1 << 0], bvs[2][1 << 1], });
	yield return ConvertArray(new[] { bvs[0][1 << 8], bvs[1][1 << 6], bvs[2][1 << 2], bvs[3][1 << 0], });
	yield return ConvertArray(new[] { bvs[1][1 << 7], bvs[1][1 << 8], bvs[3][1 << 1], bvs[3][1 << 2], });
	yield return ConvertArray(new[] { bvs[2][1 << 3], bvs[2][1 << 4], bvs[2][1 << 6], bvs[2][1 << 7], });
	yield return ConvertArray(new[] { bvs[2][1 << 5], bvs[3][1 << 3], bvs[2][1 << 8], bvs[3][1 << 6], });
	yield return ConvertArray(new[] { bvs[3][1 << 4], bvs[3][1 << 5], bvs[3][1 << 7], bvs[3][1 << 8], });
}

IEnumerable<BitVector32> Convert3To2(IList<BitVector32> bvs)
{
	var n = (int)Math.Sqrt(bvs.Count);
	return Enumerable.Range(0, n / 2).SelectMany(_i =>
		Enumerable.Range(0, n / 2).SelectMany(_j =>
		{
			var i = _i * 2;
			var j = _j * 2;
			return DoConvert3To2(new[] { bvs[i * n + j], bvs[i * n + j + 1], bvs[(i + 1) * n + j], bvs[(i + 1) * n + j + 1], });
		}));
}

IList<BitVector32> TransitionStates(IList<BitVector32> bvs)
{
	var x = bvs.AsEnumerable();
	if (bvs.Count % 2 == 0 && bvs[0][isSize3])
		x = Convert3To2(bvs);
	return x.SelectMany(s => TransitionState(s)).ToList();
}

string Print(BitVector32 state)
{
	var size = state[isSize3] ? 3 : 2;
	return string.Join(
		Environment.NewLine,
		Enumerable.Range(0, size)
			.Select(i => i * size)
			.Select(i => string.Join(
				"",
				Enumerable.Range(0, size)
					.Select(j => state[1 << (i + j)] ? '#' : '_'))));
}

var map = rules
	.Where(kvp => kvp.Key[isSize3])
	.ToDictionary(
		kvp => kvp.Key,
		kvp =>
		{
			var initialCount = CountEnabled(kvp.Key);
			var state1 = TransitionState(kvp.Key);
			var state1Count = state1.Sum(CountEnabled);
	
			var state2 = TransitionStates(state1);
			var state2Count = state2.Sum(CountEnabled);
			
			var state3 = TransitionStates(state2)
				.Select(x => GetStates(x)
					.Where(rules.ContainsKey)
					.First())
				.GroupBy(x => x, (k, v) => (k, count: v.Count()))
				.ToList();
			
			return (initialCount, state1Count, state2Count, state3);
		});

var initialState = ConvertArray(ParseString(".#./..#/###"));

var gen3 = GetStates(initialState)
	.Where(map.ContainsKey)
	.Select(x => map[x])
	.Single()
	.state3;

var gen5total = gen3
	.Select(x => map[x.k].state2Count * x.count)
	.Sum()
	.Dump("Part A");

List<(BitVector32 k, int count)> ProceedThreeGenerations(
	List<(BitVector32 k, int count)> gen0)
{
	return gen0
		.SelectMany(x => map[x.k].state3.Select(s => (s.k, count: s.count * x.count)))
		.GroupBy(
			x => x.k,
			(k, v) => (k, count: v.Sum(s => s.count)))
		.ToList();
}

var nextGen = gen3;
var cnt = 3;
while (cnt < 18)
{
	nextGen = ProceedThreeGenerations(nextGen);
	cnt += 3;
}

var gen18Total = nextGen
	.Select(x => map[x.k].initialCount * x.count)
	.Sum()
	.Dump("Part B");
