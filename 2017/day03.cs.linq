<Query Kind="Statements" />

var input = 325489;

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
	switch (sideNum)
	{
		case 0:
			// bottom; count backwards from (rank, -rank)
			return (x: rank - (lastInSide - i), y: -rank);
		case 1:
			// left; count backwards from (-rank, -rank)
			return (x: -rank, y: (lastInSide - i) - rank);
		case 2:
			// top; count backwards from (-rank, rank)
			return (x: (lastInSide - i) - rank, y: +rank);
		case 3:
			// right; count backwards from (rank, rank)
			return (x: +rank, y: rank - (lastInSide - i));
		default:
			sideNum.Dump();
			throw new InvalidOperationException("??");
	}
}

{
	var position = GetPosition(input);
	var distance = Math.Abs(position.x) + Math.Abs(position.y);
	distance.Dump("Part A");
}

var gridSize = (int)Math.Ceiling(Math.Log10(input));
var gridSideLength = Math.Max(gridSize * 2, 6);
gridSize = gridSideLength / 2;
var grid = new int?[gridSideLength + 1, gridSideLength + 1];
Func<int, int, int> getPositionSum = (int x, int y) =>
{
	return
		(grid[x - 1, y] ?? 0) +
		(grid[x + 1, y] ?? 0) +
		(grid[x, y - 1] ?? 0) +
		(grid[x, y + 1] ?? 0) +
		(grid[x - 1, y - 1] ?? 0) +
		(grid[x - 1, y + 1] ?? 0) +
		(grid[x + 1, y - 1] ?? 0) +
		(grid[x + 1, y + 1] ?? 0);
};

var idx = 1;
var gridRank = 0;
var gridBotRight = 1;

var pos = (x: gridSize, y: gridSize);
grid[pos.x, pos.y] = 1;

while (grid[pos.x, pos.y] < input)
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

grid[pos.x, pos.y].Dump("Part B");
// uncomment if you want to see the final grid!
// grid.Dump();

