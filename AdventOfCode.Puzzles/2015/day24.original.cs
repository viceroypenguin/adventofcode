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
		Potentials = [];

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

	private List<ImmutableList<int>> Potentials = [];

	private void GetSubsets(IList<int> weights)
	{
		_ = DoGetSubsets(
			[.. weights],
			[],
			0,
			int.MaxValue);
	}

	private int DoGetSubsets(ImmutableList<int> weights, ImmutableList<int> setA, int startIndex, int minLength)
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

			minLength = DoGetSubsets(
				weights.RemoveAt(i),
				setA.Add(val),
				i,
				minLength);
		}

		return minLength;
	}
}
