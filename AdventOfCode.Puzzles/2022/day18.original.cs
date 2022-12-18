namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 18, CodeType.Original)]
public partial class Day_18_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var cubes = input.Lines
			.Select(l => l.Split(',').Select(int.Parse).ToList())
			.Select(l => (x: l[0], y: l[1], z: l[2]))
			.ToHashSet();

		var surfaceArea = 0;
		foreach (var (x, y, z) in cubes)
			surfaceArea += 6 - GetNeighbors(cubes, x, y, z);

		var part1 = surfaceArea.ToString();

		var water = new HashSet<(int x, int y, int z)>();

		IEnumerable<(int, int, int)> GetCartesianNeighbors((int x, int y, int z) p)
		{
			if (water.Contains(p))
				yield break;

			water.Add(p);
			var (x, y, z) = p;

			if (x >= 0 && !cubes.Contains((x - 1, y, z)))
				yield return (x - 1, y, z);
			if (x <= 20 && !cubes.Contains((x + 1, y, z)))
				yield return (x + 1, y, z);
			if (y >= 0 && !cubes.Contains((x, y - 1, z)))
				yield return (x, y - 1, z);
			if (y <= 20 && !cubes.Contains((x, y + 1, z)))
				yield return (x, y + 1, z);
			if (z >= 0 && !cubes.Contains((x, y, z - 1)))
				yield return (x, y, z - 1);
			if (z <= 20 && !cubes.Contains((x, y, z + 1)))
				yield return (x, y, z + 1);
		}

		SuperEnumerable
			.TraverseBreadthFirst(
				(-1, -1, -1),
				GetCartesianNeighbors)
			.Consume();

		water.UnionWith(cubes);
		for (int x = -1; x <= 21; x++)
			for (int y = -1; y <= 21; y++)
				for (int z = -1; z <= 21; z++)
				{
					if (!water.Contains((x, y, z)))
						surfaceArea -= GetNeighbors(cubes, x, y, z);
				}

		var part2 = surfaceArea.ToString();
		return (part1, part2);
	}

	private static int GetNeighbors(HashSet<(int x, int y, int z)> cubes, int x, int y, int z)
	{
		var neighbors = 0;
		if (cubes.Contains((x - 1, y, z)))
			neighbors++;
		if (cubes.Contains((x + 1, y, z)))
			neighbors++;
		if (cubes.Contains((x, y - 1, z)))
			neighbors++;
		if (cubes.Contains((x, y + 1, z)))
			neighbors++;
		if (cubes.Contains((x, y, z - 1)))
			neighbors++;
		if (cubes.Contains((x, y, z + 1)))
			neighbors++;
		return neighbors;
	}
}
