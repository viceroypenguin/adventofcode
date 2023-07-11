namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 20, CodeType.Original)]
public class Day_20_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var number = Convert.ToUInt32(input.Text);

		return (
			DoPartA(number).ToString(),
			DoPartB(number).ToString());
	}

	private static int DoPartA(uint number)
	{
		var numberToBeat = number / 10;

		var numbers = new List<Tuple<int, Dictionary<int, int>>>()
		{
			Tuple.Create(0, new Dictionary<int, int>()             ),
			Tuple.Create(1, new Dictionary<int, int>() { { 1, 1 } }),
			Tuple.Create(3, new Dictionary<int, int>() { { 2, 1 } }),
		};
		for (var num = 3; ; num++)
		{
			var factors = new Dictionary<int, int>();

			var numDiv = num;
			for (var factor = (int)Math.Sqrt(num); factor > 1; factor--)
			{
				if (factor * (numDiv / factor) == numDiv)
				{
					foreach (var x in numbers[factor].Item2)
						factors[x.Key] = factors.GetValueOrDefault(x.Key) + x.Value;

					numDiv /= factor;
					factor = numDiv + 1;
				}
			}

			if (!factors.Any())
				factors.Add(num, 1);

			var sum = factors.Select(x => Enumerable.Range(0, x.Value + 1).Select(y => (int)Math.Pow(x.Key, y)).Sum()).ToList();
			var prod = 1;
			foreach (var x in sum) prod *= x;

			numbers.Add(Tuple.Create(prod, factors));

			if (prod >= numberToBeat)
				break;
		}

		return numbers.Count - 1;
	}

	private static int DoPartB(uint numberToBeat)
	{
		var numbers = new List<int>();

		var candidateIdx = default(int?);

		for (var factor = 1; !candidateIdx.HasValue || factor <= candidateIdx; factor++)
		{
			foreach (var mul in Enumerable.Range(1, 50).Reverse().Select(x => x * factor))
			{
				if (mul > candidateIdx)
					continue;

				if (numbers.Count < mul)
					numbers.AddRange(Enumerable.Range(numbers.Count, mul - numbers.Count + 1).Select(_ => 0));

				numbers[mul] += factor;

				if (numbers[mul] * 11 >= numberToBeat &&
					(!candidateIdx.HasValue || (mul < candidateIdx.Value)))
				{
					candidateIdx = mul;
				}
			}
		}

		return candidateIdx!.Value;
	}
}
