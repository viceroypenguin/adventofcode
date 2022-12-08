using CommunityToolkit.HighPerformance;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 8, CodeType.Fastest)]
public partial class Day_08_Fastest : IPuzzle
{
	public unsafe (string, string) Solve(PuzzleInput input)
	{
		var width = input.Bytes.AsSpan().IndexOf((byte)'\n');
		var height = input.Bytes.Length / (width + 1);
		Span2D<byte> map = new Span2D<byte>(input.Bytes, 0, height, width, 1);

		Span<(bool, int)> _trees = stackalloc (bool, int)[width * height];
		Span2D<(bool visible, int score)> trees = _trees.AsSpan2D(height, width);

		Span<(int index, int height)> treeStack = stackalloc (int, int)[Math.Max(width, height)];
		var stackIndex = 0;
		var maxHeight = 0;

		for (int i = 0; i < _trees.Length; i++)
			_trees[i] = (false, 1);

		for (int y = 0; y < height; y++)
		{
			stackIndex = maxHeight = 0;
			for (int x = 0; x < width; x++)
			{
				var h = map[y, x];
				if (h > maxHeight)
				{
					trees[y, x] = (true, trees[y, x].score * x);
					stackIndex = 0;
					maxHeight = h;
				}
				else
				{
					while (h > treeStack[stackIndex - 1].height)
						stackIndex--;
					var distance = x - treeStack[stackIndex - 1].index;
					trees[y, x] = (trees[y, x].visible, trees[y, x].score * distance);
				}

				treeStack[stackIndex++] = (x, h);
			}

			stackIndex = maxHeight = 0;
			for (int x = width - 1, j = 0; x >= 0; x--, j++)
			{
				var h = map[y, x];
				if (h > maxHeight)
				{
					trees[y, x] = (true, trees[y, x].score * j);
					stackIndex = 0;
					maxHeight = h;
				}
				else
				{
					while (h > treeStack[stackIndex - 1].height)
						stackIndex--;
					var distance = j - treeStack[stackIndex - 1].index;
					trees[y, x] = (trees[y, x].visible, trees[y, x].score * distance);
				}

				treeStack[stackIndex++] = (j, h);
			}
		}

		for (int x = 0; x < width; x++)
		{
			stackIndex = maxHeight = 0;
			for (int y = 0; y < height; y++)
			{
				var h = map[y, x];
				if (h > maxHeight)
				{
					trees[y, x] = (true, trees[y, x].score * y);
					stackIndex = 0;
					maxHeight = h;
				}
				else
				{
					while (h > treeStack[stackIndex - 1].height)
						stackIndex--;
					var distance = y - treeStack[stackIndex - 1].index;
					trees[y, x] = (trees[y, x].visible, trees[y, x].score * distance);
				}

				treeStack[stackIndex++] = (y, h);
			}

			stackIndex = maxHeight = 0;
			for (int y = height - 1, j = 0; y >= 0; y--, j++)
			{
				var h = map[y, x];
				if (h > maxHeight)
				{
					trees[y, x] = (true, trees[y, x].score * j);
					stackIndex = 0;
					maxHeight = h;
				}
				else
				{
					while (h > treeStack[stackIndex - 1].height)
						stackIndex--;
					var distance = j - treeStack[stackIndex - 1].index;
					trees[y, x] = (trees[y, x].visible, trees[y, x].score * distance);
				}

				treeStack[stackIndex++] = (j, h);
			}
		}

		var part1 = 0;
		var part2 = 0;

		foreach (var (visible, score) in trees)
		{
			if (visible)
				part1++;
			if (score > part2)
				part2 = score;
		}

		return (part1.ToString(), part2.ToString());
	}
}
