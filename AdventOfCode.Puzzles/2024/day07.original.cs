using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 07, CodeType.Original)]
public partial class Day_07_Original : IPuzzle
{
	[GeneratedRegex(@"^(?<test>\d+):( (?<numbers>\d+))*$", RegexOptions.ExplicitCapture)]
	private static partial Regex EquationRegex { get; }

	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Lines
			.Select(l => EquationRegex.Match(l))
			.Select(m => new
			{
				TestValue = long.Parse(m.Groups[1].Value),
				Numbers = m.Groups[2].Captures.Select(c => int.Parse(c.Value)).ToList(),
			});

		var (part1, part2) = data
			.Aggregate(
				(0L, 0L),
				(x, d) =>
				{
					if (IsValid(d.TestValue, d.Numbers))
						return (x.Item1 + d.TestValue, x.Item2 + d.TestValue);

					if (IsValid2(d.TestValue, CollectionsMarshal.AsSpan(d.Numbers)[1..], d.Numbers[0]))
						return (x.Item1, x.Item2 + d.TestValue);

					return (x.Item1, x.Item2);
				}
			);

		return (part1.ToString(), part2.ToString());
	}

	private static bool IsValid(long testValue, List<int> numbers)
	{
		var maxBits = 1 << (numbers.Count - 1);

		for (var i = 0; i < maxBits; i++)
		{
			long value = numbers[0];

			for (var j = 0; j < numbers.Count - 1; j++)
			{
				if ((i & (1 << j)) != 0)
				{
					value *= numbers[j + 1];
				}
				else
				{
					value += numbers[j + 1];
				}
			}

			if (value == testValue)
				return true;
		}

		return false;
	}

	private static bool IsValid2(long testValue, Span<int> numbers, long value = 0)
	{
		if (numbers.Length == 0)
			return value == testValue;

		{
			var sum = value;
			foreach (var n in numbers)
				sum += n;

			if (testValue == sum)
				return true;

			if (testValue < sum)
				return false;
		}

		{
			var product = value;
			foreach (var n in numbers)
				product = (product * GetMultiplier(n)) + n;

			if (testValue == product)
				return true;

			if (testValue > product)
				return false;
		}

		return IsValid2(testValue, numbers[1..], value + numbers[0])
			|| IsValid2(testValue, numbers[1..], value * numbers[0])
			|| IsValid2(testValue, numbers[1..], (value * GetMultiplier(numbers[0])) + numbers[0]);
	}

	private static long GetMultiplier(long number) =>
		number switch
		{
			0 => 1,
			< 10 => 10,
			< 100 => 100,
			< 1_000 => 1_000,
			< 10_000 => 10_000,
			< 100_000 => 100_000,
			< 1_000_000 => 1_000_000,
			< 10_000_000 => 10_000_000,
			< 100_000_000 => 100_000_000,
			< 1_000_000_000 => 1_000_000_000,
			< 10_000_000_000 => 10_000_000_000,
			< 100_000_000_000 => 100_000_000_000,
			< 1_000_000_000_000 => 1_000_000_000_000,
			< 10_000_000_000_000 => 10_000_000_000_000,
			< 100_000_000_000_000 => 100_000_000_000_000,
			< 1_000_000_000_000_000 => 1_000_000_000_000_000,
			< 10_000_000_000_000_000 => 10_000_000_000_000_000,
			< 100_000_000_000_000_000 => 100_000_000_000_000_000,
			_ => 1_000_000_000_000_000_000,
		};
}
