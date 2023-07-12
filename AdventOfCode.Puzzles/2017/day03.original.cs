namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 03, CodeType.Original)]
public class Day_03_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		(int x, int y) GetPosition(int i)
		{
			var maxRoot = (int)Math.Sqrt(i - 1);
			var rank = (maxRoot + 1) / 2;
			return GetPositionWithRank(i, rank);
		}

		(int x, int y) GetPositionWithRank(int i, int rank)
		{
			if (i <= 1) return (0, 0);

			var rankRoot = (rank * 2) + 1;
			var botRight = rankRoot * rankRoot;
			var sideLength = rank * 2;

			var sideNum = (botRight - i) / sideLength;
			var lastInSide = botRight - (sideLength * sideNum);
			return sideNum switch
			{
				0 => (x: rank - (lastInSide - i), y: -rank),
				1 => (x: -rank, y: lastInSide - i - rank),
				2 => (x: lastInSide - i - rank, y: +rank),
				3 => (x: +rank, y: rank - (lastInSide - i)),
				_ => throw new InvalidOperationException("??"),
			};
		}

		var number = Convert.ToInt32(input.Text);

		var position = GetPosition(number);
		var partA = Math.Abs(position.x) + Math.Abs(position.y);

		var gridSize = (int)Math.Ceiling(Math.Log10(number));
		var gridSideLength = Math.Max(gridSize * 2, 6);
		gridSize = gridSideLength / 2;
		var grid = new int?[gridSideLength + 1, gridSideLength + 1];
		int getPositionSum(int x, int y) =>
			(grid[x - 1, y] ?? 0) +
				(grid[x + 1, y] ?? 0) +
				(grid[x, y - 1] ?? 0) +
				(grid[x, y + 1] ?? 0) +
				(grid[x - 1, y - 1] ?? 0) +
				(grid[x - 1, y + 1] ?? 0) +
				(grid[x + 1, y - 1] ?? 0) +
				(grid[x + 1, y + 1] ?? 0);

		var idx = 1;
		var gridRank = 0;
		var gridBotRight = 1;

		var pos = (x: gridSize, y: gridSize);
		grid[pos.x, pos.y] = 1;

		while (grid[pos.x, pos.y] < number)
		{
			if (gridBotRight == idx)
			{
				gridRank++;
				var tmp = (gridRank * 2) + 1;
				gridBotRight = tmp * tmp;
			}

			idx++;
			{
				var (x, y) = GetPositionWithRank(idx, gridRank);
				// translate to printed output as expected
				pos = (x: -y + gridSize, y: x + gridSize);
			}

			grid[pos.x, pos.y] = getPositionSum(pos.x, pos.y);
		}

		var partB = grid[pos.x, pos.y];

		return (partA.ToString(), partB.ToString());
	}
}
