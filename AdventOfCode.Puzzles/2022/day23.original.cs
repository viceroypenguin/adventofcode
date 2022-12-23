namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 23, CodeType.Original)]
public partial class Day_23_Original : IPuzzle
{
	private enum Direction { North, East, South, West, };
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var grid = input.Bytes.GetMap()
			.GetMapPoints()
			.Where(p => p.item == '#')
			.Select(p => p.p)
			.ToHashSet();
		var startDir = Direction.North;

		var i = 0;
		for (; i < 10; i++)
		{
			(grid, startDir, _) = RunLifeStep(grid, startDir);
		}

		var (minX, maxX, minY, maxY) = grid
			.Aggregate(
				(minX: int.MaxValue, maxX: int.MinValue, minY: int.MaxValue, maxY: int.MinValue),
				(dim, p) => (
					minX: Math.Min(dim.minX, p.x),
					maxX: Math.Max(dim.maxX, p.x),
					minY: Math.Min(dim.minY, p.y),
					maxY: Math.Max(dim.maxY, p.y)));

		var part1 = (maxX - minX + 1) * (maxY - minY + 1) - grid.Count;

		while (true)
		{
			(grid, startDir, var noMoves) = RunLifeStep(grid, startDir);
			if (noMoves)
				break;
			i++;
		}

		return (part1.ToString(), (i + 1).ToString());
	}

	static Direction NextDirection(Direction dir) =>
		dir switch
		{
			Direction.North => Direction.South,
			Direction.South => Direction.West,
			Direction.West => Direction.East,
			Direction.East => Direction.North,
		};

	static (HashSet<(int x, int y)> grid, Direction startDir, bool noMoves) RunLifeStep(
		HashSet<(int x, int y)> grid, Direction startDir)
	{
		var next = new List<((int x, int y) old, (int x, int y) @new)>();
		foreach (var (x, y) in grid)
		{
			var @new = (x, y);

			if (grid.Contains((x - 1, y - 1))
				|| grid.Contains((x, y - 1))
				|| grid.Contains((x + 1, y - 1))
				|| grid.Contains((x + 1, y))
				|| grid.Contains((x + 1, y + 1))
				|| grid.Contains((x, y + 1))
				|| grid.Contains((x - 1, y))
				|| grid.Contains((x - 1, y + 1)))
			{
				var dir = startDir;
				do
				{
					var (blocked, tmp) = dir switch
					{
						Direction.North => (
							grid.Contains((x - 1, y - 1))
							|| grid.Contains((x, y - 1))
							|| grid.Contains((x + 1, y - 1)),
							(x, y - 1)),
						Direction.East => (
							grid.Contains((x + 1, y - 1))
							|| grid.Contains((x + 1, y))
							|| grid.Contains((x + 1, y + 1)),
							(x + 1, y)),
						Direction.South => (
							grid.Contains((x - 1, y + 1))
							|| grid.Contains((x, y + 1))
							|| grid.Contains((x + 1, y + 1)),
							(x, y + 1)),
						Direction.West => (
							grid.Contains((x - 1, y - 1))
							|| grid.Contains((x - 1, y))
							|| grid.Contains((x - 1, y + 1)),
							(x - 1, y)),
					};

					if (!blocked)
					{
						@new = tmp;
						break;
					}
				} while ((dir = NextDirection(dir)) != startDir);
			}

			next.Add(((x, y), @new));
		}

		var noMoves = next.All(x => x.old == x.@new);

		grid = next
			.GroupBy(x => x.@new)
			.SelectMany(g => g.Count() == 1
				? g.Select(x => x.@new)
				: g.Select(x => x.old))
			.ToHashSet();

		return (grid, NextDirection(startDir), noMoves);
	}
}
