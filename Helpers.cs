using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode;

public static class Helpers
{
	public static string GetString(this byte[] input) =>
		Encoding.ASCII.GetString(input);

	private static readonly string[] _splitChars = new[] { "\r\n", "\n", };
	public static string[] GetLines(this byte[] input, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) =>
		GetString(input)
			.Split(_splitChars, options);

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

	private static readonly string[] _segmentSplitChars = new[] { "\r\n\r\n", "\n\n", };
	public static string[][] GetSegments(this byte[] input) =>
		GetString(input)
			.Split(_segmentSplitChars, StringSplitOptions.None)
			.Select(s => s.Split(_splitChars, StringSplitOptions.None))
			.ToArray();

	public static bool Between<T>(this T value, T min, T max) where T : IComparable<T>
	{
		if (min.CompareTo(max) > 0) (min, max) = (max, min);
		return min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (int value, int numChars) AtoI(this ReadOnlySpan<byte> bytes)
	{
		// initial sign? also, track negative
		var isNegSign = bytes[0] == '-';
		if (isNegSign || bytes[0] == '+')
			bytes = bytes[1..];

		var value = 0;
		var n = 0;
		foreach (var t in bytes)
		{
			if (t == '\n')
				break;

			value = value * 10 + (t - '0');
			n++;
		}

		return (isNegSign ? -value : value, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (long value, int numChars) AtoL(this ReadOnlySpan<byte> bytes)
	{
		// initial sign? also, track negative
		var isNegSign = bytes[0] == '-';
		if (isNegSign || bytes[0] == '+')
			bytes = bytes[1..];

		var value = 0L;
		var n = 0;
		foreach (var t in bytes)
		{
			if (t == '\n')
				break;

			value = value * 10 + (t - '0');
			n++;
		}

		return (isNegSign ? -value : value, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static long gcd(long a, long b)
	{
		while (b != 0) b = a % (a = b);
		return a;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static long lcm(long a, long b) =>
		a * b / gcd(a, b);

	public static IEnumerable<((int x, int y) p, T item)> GetMapPoints<T>(
			this IReadOnlyList<IReadOnlyList<T>> map) =>
		 Enumerable.Range(0, map.Count)
			.SelectMany(y => Enumerable.Range(0, map[y].Count)
				.Select(x => ((x, y), map[y][x])));

	public static readonly IReadOnlyList<(int x, int y)> Neighbors =
		new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0), };
	public static IEnumerable<(int x, int y)> GetCartesianNeighbors(this (int x, int y) p) =>
		Neighbors.Select(d => (p.x + d.x, p.y + d.y));

	public static IEnumerable<(int x, int y)> GetCartesianNeighbors<T>(
			this (int x, int y) p,
			IReadOnlyList<IReadOnlyList<T>> map) =>
		p.GetCartesianNeighbors()
			.Where(q =>
				q.y >= 0 && q.y < map.Count
				&& q.x >= 0 && q.x < map[q.y].Count);

	public static readonly IReadOnlyList<(int x, int y)> Adjacent =
		new (int x, int y)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1), };
	public static IEnumerable<(int x, int y)> GetCartesianAdjacent(this (int x, int y) p) =>
		Adjacent.Select(d => (p.x + d.x, p.y + d.y));

	public static IEnumerable<(int x, int y)> GetCartesianAdjacent<T>(
			this (int x, int y) p,
			IReadOnlyList<IReadOnlyList<T>> map) =>
		p.GetCartesianAdjacent()
			.Where(q =>
				q.y >= 0 && q.y < map.Count
				&& q.x >= 0 && q.x < map[q.y].Count);

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
				return Array.Empty<(int x, int y)>();

			// on another type of border
			if (!canVisitPoint(q, map[y][x]))
				// don't go anywhere
				return Array.Empty<(int x, int y)>();

			// now we've been here for the first time
			// remember this and increase size of the basin
			seen.Add(q);
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
