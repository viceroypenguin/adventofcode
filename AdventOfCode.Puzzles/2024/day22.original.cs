using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 22, CodeType.Original)]
public partial class Day_22_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0L;
		var prices = new Dictionary<(int, int, int, int), int>();
		var seen = new HashSet<(int, int, int, int)>();

		foreach (var l in input.Lines)
		{
			seen.Clear();

			var num = long.Parse(l);
			var d1 = 0;
			var d2 = 0;
			var d3 = 0;
			var d4 = 0;

			for (var i = 0; i < 2000; i++)
			{
				var x = GetNextNumber(num);
				(num, d1, d2, d3, d4) = (x, ((int)x % 10) - ((int)num % 10), d1, d2, d3);

				if (i >= 3 && seen.Add((d1, d2, d3, d4)))
				{
					ref var z = ref CollectionsMarshal.GetValueRefOrAddDefault(prices, (d1, d2, d3, d4), out _);
					z += (int)x % 10;
				}
			}

			part1 += (int)num;
		}

		var part2 = prices.Values.Max();

		return (part1.ToString(), part2.ToString());
	}

	private static long GetNextNumber(long secret)
	{
		var num = ((secret * 64) ^ secret) % 16777216;
		num = (num / 32) ^ num;
		num = ((num * 2048) ^ num) % 16777216;
		return num;
	}
}
