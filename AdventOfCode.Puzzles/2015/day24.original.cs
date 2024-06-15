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

		_totalWeight = weights.Sum();
		_groupWeight = _totalWeight / 3;

		GetSubsets(weights);
		var partA =
			_potentials
				.Select(l => new
				{
					List = l,
					QE = l.Aggregate(1ul, (p, w) => p * (ulong)w),
				})
				.OrderBy(l => l.QE)
				.First()
				.QE;

		_groupWeight = _totalWeight / 4;
		_potentials = [];

		GetSubsets(weights);
		var partB =
			_potentials
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

	private int _totalWeight;
	private int _groupWeight;

	private List<ImmutableList<int>> _potentials = [];

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
			if (newTotal > _groupWeight)
				continue;

			if (newTotal == _groupWeight)
			{
				if (setA.Count + 1 < minLength)
				{
					minLength = setA.Count + 1;
					_potentials.Clear();
				}

				_potentials.Add(setA.Add(val));
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
