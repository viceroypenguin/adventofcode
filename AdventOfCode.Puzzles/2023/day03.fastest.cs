namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 03, CodeType.Fastest)]
public partial class Day_03_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;
		var y = 0;

		Span<(int number, int y, int start, int end)> openNumbers =
			stackalloc (int number, int y, int start, int end)[24];
		Span<(int x, int y, byte symbol)> openSymbol =
			stackalloc (int x, int y, byte symbol)[24];

		Span<(int x, int y, int num1)> openGears =
			stackalloc (int x, int y, int num1)[24];

		foreach (var l in input.Span.EnumerateLines())
		{
			var x = 0;
			while (x < l.Length)
			{
				if (l[x] == '.')
				{
					var advance = l[x..].IndexOfAnyExcept((byte)'.');
					if (advance == -1)
						break;
					x += advance;
				}

				if (l[x] is >= (byte)'0' and <= (byte)'9')
				{
					var (num, n) = l[x..(x + 3)].AtoI();

					var flag = false;
					foreach (var (sx, sy, b) in openSymbol)
					{
						if (b == 0) continue;
						if (sy < y - 1) continue;

						if (sx >= x - 1 && sx <= x + n)
						{
							flag = true;
							break;
						}
					}

					if (flag)
					{
						part1 += num;
					}
					else
					{
						for (var i = 0; ; i++)
						{
							if (openNumbers[i].number == 0
								|| openNumbers[i].y < y - 1)
							{
								openNumbers[i] = (num, y, x - 1, x + n);
								break;
							}
						}
					}

					x += n;
				}

				else
				{
					for (var i = 0; i < openNumbers.Length; i++)
					{
						if (openNumbers[i].number == 0)
							continue;

						if (openNumbers[i].y < y - 1)
							continue;

						if (x < openNumbers[i].start
							|| x > openNumbers[i].end)
						{
							continue;
						}

						part1 += openNumbers[i].number;
						openNumbers[i] = default;
					}

					for (var i = 0; ; i++)
					{
						if (openSymbol[i].symbol == 0
							|| openSymbol[i].y < y - 1)
						{
							openSymbol[i] = (x, y, l[x]);
							break;
						}
					}

					x++;
				}
			}

			y++;
		}

		return (part1.ToString(), part2.ToString());
	}
}
