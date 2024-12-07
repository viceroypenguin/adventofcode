
namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 07, CodeType.Original)]
public partial class Day_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var regex = new Regex(@"^(?<test>\d+):( (?<numbers>\d+))*$", RegexOptions.ExplicitCapture);
		var data = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => new
			{
				TestValue = long.Parse(m.Groups[1].Value),
				Numbers = m.Groups[2].Captures.Select(c => int.Parse(c.Value)).ToList(),
			})
			.ToList();

		var part1 = data
			.Where(d => IsValid(d.TestValue, d.Numbers))
			.Sum(d => d.TestValue)
			.ToString();

		var part2 = data
			.Where(d => IsValid2(d.TestValue, d.Numbers))
			.Sum(d => d.TestValue)
			.ToString();

		return (part1, part2);
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
					_ => long.Parse(value.ToString() + numbers[j + 1].ToString()),
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
}
