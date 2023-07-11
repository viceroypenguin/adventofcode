using System.Collections.Immutable;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 24, CodeType.Original)]
public class Day_24_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var weights = input.Lines
			.Select(i => Convert.ToInt32(i))
			.OrderByDescending(i => i)
			.ToList();

		totalWeight = weights.Sum();
		groupWeight = totalWeight / 3;

		GetSubsets(weights);
		var partA = 
			Potentials
				.Select(l => new
				{
					List = l,
					QE = l.Aggregate(1ul, (p, w) => p * (ulong)w),
				})
				.OrderBy(l => l.QE)
				.First()
				.QE;

		groupWeight = totalWeight / 4;
		Potentials = new List<IList<int>>();

		GetSubsets(weights);
		var partB = 
			Potentials
				.Select(l => new
				{
					List = l,
					QE = l.Aggregate(1ul, (p, w) => p * (ulong)w),
				})
				.OrderBy(l => l.QE)
				.First()
				.QE;

		return (partA.ToString(), partB.ToString());
	}

	private int totalWeight;
	private int groupWeight;

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
}
