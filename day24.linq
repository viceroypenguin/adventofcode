<Query Kind="Program">
  <GACReference>System.Collections.Immutable, Version=1.1.37.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <GACReference>System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

void Main()
{
	var input =
@"1
3
5
11
13
17
19
23
29
31
37
41
43
47
53
59
67
71
73
79
83
89
97
101
103
107
109
113

";
	var weights = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(i => Convert.ToInt32(i))
		.OrderByDescending(i=>i)
		.ToList();
		
	totalWeight = weights.Sum();
	groupWeight = totalWeight / 3;

	GetSubsets(weights);
	Potentials
		.Select(l => new
		{
			List = l,
			QE = l.Aggregate(1ul, (p, w) => p * (ulong)w),
		})
		.OrderBy(l => l.QE)
		.Dump();
}

int totalWeight;
int groupWeight;

IList<IList<int>> Potentials = new List<IList<int>>();

void GetSubsets(IList<int> weights)
{
	_GetSubsets(
		ImmutableList<int>.Empty.AddRange(weights),
		ImmutableList<int>.Empty,
		0,
		int.MaxValue);
}

int _GetSubsets(ImmutableList<int> weights, ImmutableList<int> setA, int startIndex, int minLength)
{
	var setTotalSoFar = setA.Sum();
	foreach (var i in Enumerable.Range(startIndex, weights.Count - startIndex))
    {
		if (setA.Count >= minLength)
			break;

		var val = weights[i];
		var newTotal = setTotalSoFar + val;
		if (newTotal > groupWeight)
			continue;

		if (newTotal == groupWeight)
		{
			if (setA.Count + 1 < minLength)
			{
				minLength = setA.Count + 1;
				Potentials.Clear();
			}
			Potentials.Add(setA.Add(val));
		}

		minLength = _GetSubsets(
			weights.RemoveAt(i),
			setA.Add(val),
			i,
			minLength);
	}
	
	return minLength;
}