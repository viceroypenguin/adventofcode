namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 18, CodeType.Original)]
public class Day_18_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var cubes = input.Lines
			.Select(l => l.Split(',').Select(int.Parse).ToList())
			.Select(l => (x: l[0], y: l[1], z: l[2]))
			.ToHashSet();

		var water = new HashSet<(int x, int y, int z)>(22 * 22 * 22);
		IEnumerable<(int, int, int)> GetCartesianNeighbors((int x, int y, int z) p)
		{
			if (water.Contains(p))
				yield break;

			water.Add(p);
			var (x, y, z) = p;

			if (x >= 0 && !cubes.Contains((x - 1, y, z)))
				yield return (x - 1, y, z);
			if (x <= 21 && !cubes.Contains((x + 1, y, z)))
				yield return (x + 1, y, z);
			if (y >= 0 && !cubes.Contains((x, y - 1, z)))
				yield return (x, y - 1, z);
			if (y <= 21 && !cubes.Contains((x, y + 1, z)))
				yield return (x, y + 1, z);
			if (z >= 0 && !cubes.Contains((x, y, z - 1)))
				yield return (x, y, z - 1);
			if (z <= 21 && !cubes.Contains((x, y, z + 1)))
				yield return (x, y, z + 1);
		}

		SuperEnumerable
			.TraverseBreadthFirst(
				(-1, -1, -1),
				GetCartesianNeighbors)
			.Consume();

		var part1 = 0;
		var part2 = 0;
		foreach (var (x, y, z) in cubes)
		{
			part1 += 6 - GetNeighbors(cubes, x, y, z);
			part2 += GetNeighbors(water, x, y, z);
		}

		return (part1.ToString(), part2.ToString());
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
