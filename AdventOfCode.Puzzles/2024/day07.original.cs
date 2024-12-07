
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
				(x, d) => (
					x.Item1 + (IsValid(d.TestValue, d.Numbers) ? d.TestValue : 0),
					x.Item2 + (IsValid2(d.TestValue, d.Numbers) ? d.TestValue : 0)
				)
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

	private static bool IsValid2(long testValue, List<int> numbers)
	{
		var operators = new int[numbers.Count - 1];

		while (true)
		{
			long value = numbers[0];

			for (var j = 0; j < numbers.Count - 1; j++)
			{
				value = operators[j] switch
				{
					0 => value + numbers[j + 1],
					1 => value * numbers[j + 1],
					_ => (value * GetMultiplier(numbers[j + 1])) + numbers[j + 1],
				};
			}

			if (value == testValue)
				return true;

			if (operators.All(o => o == 2))
				return false;

			for (var j = operators.Length - 1; j >= 0; j--)
			{
				if (operators[j] == 2)
				{
					operators[j] = 0;
				}
				else
				{
					operators[j]++;
					break;
				}
			}
		}
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
