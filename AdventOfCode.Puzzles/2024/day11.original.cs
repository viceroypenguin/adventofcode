using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 11, CodeType.Original)]
public partial class Day_11_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var numbers = input.Lines[0].Split(' ').Select(long.Parse).ToList();
		var newNumbers = new List<long>();

		for (var i = 0; i < 25; i++)
		{
			newNumbers.Clear();
			foreach (var n in numbers)
			{
				if (n is 0)
				{
					newNumbers.Add(1);
				}
				else if ((long)Math.Log10(n) % 2 == 1)
				{
					var pow = (long)Math.Pow(10, (long)Math.Log10(n) / 2 + 1);

					newNumbers.Add(n / pow);
					newNumbers.Add(n % pow);
				}
				else
				{
					newNumbers.Add(n * 2024);
				}
			}

			(numbers, newNumbers) = (newNumbers, numbers);
		}

		var part1 = numbers.Count.ToString();

		var stones = input.Lines[0].Split(' ')
			.Select(long.Parse)
			.CountBy(x => x)
			.ToDictionary(x => x.Key, x => (long)x.Value);

		var newStones = new Dictionary<long, long>();

		for (var i = 0; i < 75; i++)
		{
			newStones.Clear();

			foreach (var (stone, numberOfStones) in stones)
			{
				if (stone is 0)
				{
					newStones.Increment(1, numberOfStones);
				}
				else if ((long)Math.Log10(stone) % 2 == 1)
				{
					var pow = (long)Math.Pow(10, (long)Math.Log10(stone) / 2 + 1);

					newStones.Increment(stone / pow, numberOfStones);
					newStones.Increment(stone % pow, numberOfStones);
				}
				else
				{
					newStones.Increment(stone * 2024, numberOfStones);
				}
			}

			(stones, newStones) = (newStones, stones);
		}

		var part2 = stones.Sum(x => x.Value).ToString();
		return (part1, part2);
	}
}

file static class DictionaryExtensions
{
	public static void Increment(this Dictionary<long, long> dictionary, long key, long increment)
	{
		ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out _);
		value += increment;
	}
}
