namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 04, CodeType.Fastest)]
public sealed partial class Day_04_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;

		Span<int> winning = stackalloc int[10];
		Span<int> mine = stackalloc int[25];
		Span<int> multipliers = stackalloc int[10];

		foreach (var line in input.Span.EnumerateLines())
		{
			if (line.Length == 0)
				break;

			for (int x = 10, n = 0; n < winning.Length; x += 3, n++)
			{
				var tens = line[x] == ' ' ? 0 : (line[x] - '0') * 10;
				winning[n] = tens + line[x + 1] - '0';
			}

			for (int x = 42, n = 0; n < mine.Length; x += 3, n++)
			{
				var tens = line[x] == ' ' ? 0 : (line[x] - '0') * 10;
				mine[n] = tens + line[x + 1] - '0';
			}

			var count = 0;
			foreach (var w in winning)
			{
				foreach (var m in mine)
				{
					if (w == m)
					{
						count++;
						break;
					}
				}
			}

			part1 += 1 << (count - 1);

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
