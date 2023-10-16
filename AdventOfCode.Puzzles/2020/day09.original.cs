namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 9, CodeType.Original)]
public class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var numbers = input.Lines
			.Select(long.Parse)
			.ToArray();

		var invalidNumber = 0L;
		var queue = new Queue<long>(numbers.Take(25));
		var part1 = string.Empty;
		foreach (var n in numbers.Skip(25))
		{
			if (queue.Subsets(2)
					.Where(l => l[0] != l[1])
					.Any(l => l[0] + l[1] == n))
			{
				queue.Dequeue();
				queue.Enqueue(n);
			}
			else
			{
				part1 = (invalidNumber = n).ToString();
				break;
			}
		}

		Array.Reverse(numbers);
		for (var i = 0; ; i++)
		{
			if (numbers[i] > invalidNumber)
				continue;

			var x = numbers[i];
			var (sum, min, max) = (x, x, x);
			for (var j = i + 1; j < numbers.Length; j++)
			{
				sum += numbers[j];
				min = Math.Min(min, numbers[j]);
				max = Math.Max(max, numbers[j]);
				if (sum == invalidNumber)
				{
					var part2 = (min + max).ToString();
					return (part1, part2);
				}

				if (sum > invalidNumber)
					break;
			}
		}
	}
}
