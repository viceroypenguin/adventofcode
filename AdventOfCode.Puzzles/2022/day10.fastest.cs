namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 10, CodeType.Fastest)]
public partial class Day_10_Fastest : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var sum = 0;
		var reg = 1;
		var add = 0;
		var inst = 0;
		var delay = 1;

		Span<char> screen = stackalloc char[(41 * 6) - 1];
		for (var j = 40; j < screen.Length; j += 41)
			screen[j] = '\n';

		for (int y = 0, j = 0; j < screen.Length; y += 40, j += 41)
		{
			for (var x = 0; x < 40; x++)
			{
				if (--delay == 0)
				{
					reg += add;
					var l = input.Lines[inst++];
					if (!l.Equals("noop", StringComparison.OrdinalIgnoreCase))
					{
						delay = 2;
						(add, _) = l.AsSpan()[5..].AtoI();
					}
					else
					{
						(delay, add) = (1, 0);
					}
				}

				if (x == 19)
					sum += reg * (y + x + 1);

				var c = Math.Abs(x - reg) <= 1 ? '█' : ' ';
				screen[j + x] = c;
			}
		}

		return (sum.ToString(), new string(screen));
	}
}
