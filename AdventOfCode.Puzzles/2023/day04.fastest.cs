using System.Numerics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 04, CodeType.Fastest)]
public sealed partial class Day_04_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;

		Span<ulong> winning = stackalloc ulong[2];
		Span<ulong> mine = stackalloc ulong[2];
		Span<int> multipliers = stackalloc int[10];

		foreach (var line in input.Span.EnumerateLines())
		{
			if (line.Length == 0)
				break;

			winning[0] = 0;
			winning[1] = 0;
			mine[0] = 0;
			mine[1] = 0;

			for (int x = 10, n = 0; n < 10; x += 3, n++)
			{
				var num = ((line[x] | 0x10) - '0') * 10;
				num += line[x + 1] - '0';

				winning[num / 64] |= 1UL << (num % 64);
			}

			for (int x = 42, n = 0; n < 25; x += 3, n++)
			{
				var num = ((line[x] | 0x10) - '0') * 10;
				num += line[x + 1] - '0';

				mine[num / 64] |= 1UL << (num % 64);
			}

			var count = BitOperations.PopCount(winning[0] & mine[0])
				+ BitOperations.PopCount(winning[1] & mine[1]);

			part1 += (1 << count) >> 1;

			var cards = multipliers[0] + 1;
			part2 += cards;

			multipliers[1..10].CopyTo(multipliers);
			multipliers[9] = 0;
			for (var i = 0; i < count; i++)
				multipliers[i] += cards;
		}

		return (part1.ToString(), part2.ToString());
	}
}
