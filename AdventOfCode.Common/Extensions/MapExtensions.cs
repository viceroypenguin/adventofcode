namespace AdventOfCode.Common.Extensions;

public static class MapExtensions
{
	public static byte[][] GetMap(this byte[] input) =>
		input.Segment(b => b == '\n')
			.Select(l => l
				.SkipWhile(b => b == '\n')
				.ToArray())
			.Where(l => l.Length > 0)
			.ToArray();

	public static int[][] GetIntMap(this byte[] input) =>
		input.Segment(b => b == '\n')
			.Select(l => l
				.SkipWhile(b => b == '\n')
				.Select(b => b - '0')
				.ToArray())
			.Where(l => l.Length > 0)
			.ToArray();

	public static IEnumerable<((int x, int y) p, T item)> GetMapPoints<T>(
			this IReadOnlyList<IReadOnlyList<T>> map) =>
		 Enumerable.Range(0, map.Count)
			.SelectMany(y => Enumerable.Range(0, map[y].Count)
				.Select(x => ((x, y), map[y][x])));

	public static bool IsValid<T>(
			this (int x, int y) p,
			IReadOnlyList<IReadOnlyList<T>> map) =>
		p.x.Between(0, map[0].Count - 1)
		&& p.y.Between(0, map.Count - 1);

	public static readonly IReadOnlyList<(int x, int y)> Neighbors =
		[(0, 1), (0, -1), (1, 0), (-1, 0),];
	public static IEnumerable<(int x, int y)> GetCartesianNeighbors(this (int x, int y) p) =>
		Neighbors.Select(d => (p.x + d.x, p.y + d.y));

	public static IEnumerable<(int x, int y)> GetCartesianNeighbors<T>(
			this (int x, int y) p,
			IReadOnlyList<IReadOnlyList<T>> map) =>
		p.GetCartesianNeighbors()
			.Where(q => q.IsValid(map));

	public static readonly IReadOnlyList<(int x, int y)> Adjacent =
		[(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1),];
	public static IEnumerable<(int x, int y)> GetCartesianAdjacent(this (int x, int y) p) =>
		Adjacent.Select(d => (p.x + d.x, p.y + d.y));

	public static IEnumerable<(int x, int y)> GetCartesianAdjacent<T>(
			this (int x, int y) p,
			IReadOnlyList<IReadOnlyList<T>> map) =>
		p.GetCartesianAdjacent()
			.Where(q => q.IsValid(map));

	public static void FloodFill<T>(
			this IReadOnlyList<IReadOnlyList<T>> map,
			(int x, int y) startingPoint,
			Func<(int x, int y), T, bool> canVisitPoint,
			Action<(int x, int y), T> visitPoint)
	{
		// keep track of where we've been
		var seen = new HashSet<(int x, int y)>();

		// get the neighboring points...
		IEnumerable<(int x, int y)> Traverse((int x, int y) q)
		{
			var (x, y) = q;

			// we've been here before
			if (seen.Contains((x, y)))
				// don't go anywhere
				return [];

			// on another type of border
			if (!canVisitPoint(q, map[y][x]))
				// don't go anywhere
				return [];

			// now we've been here for the first time
			// remember this and increase size of the basin
			_ = seen.Add(q);
			visitPoint(q, map[y][x]);

			// go in all four directions
			return q.GetCartesianNeighbors(map);
		}

		// execute a BFS based on traverse method
		SuperEnumerable
			.TraverseBreadthFirst(
				startingPoint,
				Traverse)
			.Consume();
	}
}
