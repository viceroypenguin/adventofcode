namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 8, CodeType.Fastest)]
public partial class Day_08_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var height = input.Lines.Length;
		var width = input.Lines[0].Length;

		var part1 = (height * 2) + (width * 2) - 4;
		var part2 = 0;

		for (int y = 1, ty = 0; y < height - 1; y++, ty += width - 2)
		{
			for (var x = 1; x < width - 1; x++)
			{
				var score = 1;
				var visible = false;
				var h = input.Lines[y][x];

				// w
				var (v, j) = (true, 0);
				for (j = x - 1; v && j >= 0; j--)
				{
					if (input.Lines[y][j] >= h)
						v = false;
				}

				score *= x - j - 1;
				visible |= v;

				// e
				v = true;
				for (j = x + 1; v && j < width; j++)
				{
					if (input.Lines[y][j] >= h)
						v = false;
				}

				score *= j - x - 1;
				visible |= v;

				// n
				v = true;
				for (j = y - 1; v && j >= 0; j--)
				{
					if (input.Lines[j][x] >= h)
						v = false;
				}

				score *= y - j - 1;
				visible |= v;

				// s
				v = true;
				for (j = y + 1; v && j < height; j++)
				{
					if (input.Lines[j][x] >= h)
						v = false;
				}

				score *= j - y - 1;
				visible |= v;

				if (visible)
					part1++;
				if (score > part2)
					part2 = score;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
