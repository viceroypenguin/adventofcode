namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 02, CodeType.Fastest)]
public class Day_02_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.GetSpan();
		int part1 = 0, part2 = 0;
		Span<int> arr = stackalloc int[16];

		var j = 0;
		for (var i = 0; i < span.Length;)
		{
			var (x, y) = span[i..].AtoI();
			arr[j++] = x;
			i += y;

			if (span[i] == '\n')
			{
				int min = arr[0], max = arr[0];
				for (j = 1; j < 16; j++)
				{
					var n = arr[j];
					if (n < min) min = n;
					if (n > max) max = n;
				}

				part1 += max - min;

				var flag = false;
				for (j = 0; !flag && j < 16; j++)
				{
					for (var k = 0; !flag && k < 16; k++)
					{
						if (j == k) continue;
						if (arr[j] % arr[k] == 0)
						{
							part2 += arr[j] / arr[k];
							flag = true;
						}
						else if (arr[k] % arr[j] == 0)
						{
							part2 += arr[k] / arr[j];
							flag = true;
						}
					}
				}

				j = 0;
			}

			i++;
		}

		return (
			part1.ToString(),
			part2.ToString());
	}
}
