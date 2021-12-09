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

	public static byte[][] GetMap(this byte[] input)
	{
		var width = input.Index().First(b => b.Value == '\n').Key;
		return input.Batch(width + 1).Select(x => x.Take(..^1).ToArray()).ToArray();
	}

	private static readonly string[] _segmentSplitChars = new[] { "\r\n\r\n", "\n\n", };
	public static string[][] GetSegments(this byte[] input) =>
		GetString(input)
			.Split(_segmentSplitChars, StringSplitOptions.None)
			.Select(s => s.Split(_splitChars, StringSplitOptions.None))
			.ToArray();

	public static bool Between<T>(this T value, T min, T max) where T : IComparable<T> =>
		min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;

	// copied from: https://stackoverflow.com/questions/35127060/how-to-implement-atoi-using-simd
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (int value, int numChars) AtoI(this ReadOnlySpan<byte> bytes)
	{
		// initial sign? also, track negative
		var isNegSign = bytes[0] == '-' ? -1 : 0;
		var isPosSign = bytes[0] == '+' ? -1 : 0;
		var index = -(isNegSign + isPosSign);

		// skip leading zeros
		while (index < bytes.Length && bytes[index] == '0')
			index++;

		// get string length
		var start = index;
		while (index < bytes.Length && bytes[index] is >= (byte)'0' and <= (byte)'9')
			index++;

		if (index == start)
			return (0, index);

		// load into xmm register
		var value = Unsafe.ReadUnaligned<Vector128<byte>>(
			ref MemoryMarshal.GetReference(bytes[start..]));

		var zero = Vector128<byte>.Zero;

		// broadcast (length - 16) to all bytes
		var len = Avx2.Shuffle(
			Vector128.Create((byte)(index - start - 16)),
			zero);
		// create shuffle array
		var shuffle = Avx2.Add(
			Vector128.Create((byte)15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0),
			len);

		// shuffle array reverses items, and only includes valid ones
		value = Avx2.Shuffle(value, shuffle);

		// subtract the '0'
		value = Avx2.SubtractSaturate(
			value,
			Avx2.Shuffle(
				Vector128.Create((byte)'0'),
				zero));

		// unpack to words
		var lowUnpack = Avx2.UnpackLow(value, zero).AsInt16();
		var highUnpack = Avx2.UnpackHigh(value, zero).AsInt16();

		// scale each word
		var mulValue = Vector128.Create(1, 10, 100, 1000, 1, 10, 100, 1000);
		var lowScaled = Avx2.MultiplyAddAdjacent(lowUnpack, mulValue);
		var highScaled = Avx2.MultiplyAddAdjacent(highUnpack, mulValue);

		// horizontally add
		var scaled = Avx2.HorizontalAdd(lowScaled, highScaled);

		// scale again
		scaled = Avx2.MultiplyLow(
			scaled,
			Vector128.Create(1, 10_000, 100_000_000, 0));

		// two more horizontal adds
		scaled = Avx2.Add(
			scaled,
			Avx2.Shuffle(
				scaled,
				0b11101110));
		scaled = Avx2.Add(
			scaled,
			Avx2.Shuffle(
				scaled,
				0b01010101));

		var result = scaled.ToScalar();
		return (result + isNegSign, index);
	}

	// copied from: https://stackoverflow.com/questions/35127060/how-to-implement-atoi-using-simd
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (long value, int numChars) AtoL(this ReadOnlySpan<byte> bytes)
	{
		// initial sign? also, track negative
		var isNegSign = bytes[0] == '-' ? -1 : 0;
		var isPosSign = bytes[0] == '+' ? -1 : 0;
		var index = -(isNegSign + isPosSign);

		// skip leading zeros
		while (index < bytes.Length && bytes[index] == '0')
			index++;

		// get string length
		var start = index;
		while (index < bytes.Length && bytes[index] is >= (byte)'0' and <= (byte)'9')
			index++;

		if (index == start)
			return (0, index);

		// load into xmm register
		var value = Unsafe.ReadUnaligned<Vector256<byte>>(
			ref MemoryMarshal.GetReference(bytes[start..]));

		var zero = Vector256<byte>.Zero;

		// broadcast (length - 32) to all bytes
		var len = Avx2.Shuffle(
			Vector256.Create((byte)(index - start - 32)),
			zero);
		// create shuffle array
		var shuffle = Avx2.Add(
			Vector256.Create((byte)
				31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16,
				15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0),
			len);

		// shuffle array reverses items, and only includes valid ones
		value = Avx2.Shuffle(value, shuffle);

		// subtract the '0'
		value = Avx2.SubtractSaturate(
			value,
			Avx2.Shuffle(
				Vector256.Create((byte)'0'),
				zero));

		// unpack to words
		var lowUnpack = Avx2.UnpackLow(value, zero).AsInt16();
		var highUnpack = Avx2.UnpackHigh(value, zero).AsInt16();

		// scale each word
		var mulValue = Vector256.Create(
			1, 10, 100, 1000, 1, 10, 100, 1000,
			1, 10, 100, 1000, 1, 10, 100, 1000);
		var lowScaled = Avx2.MultiplyAddAdjacent(lowUnpack, mulValue);
		var highScaled = Avx2.MultiplyAddAdjacent(highUnpack, mulValue);

		// horizontally add
		var scaled = Avx2.HorizontalAdd(lowScaled, highScaled);

		var ints = MemoryMarshal.Cast<Vector256<int>, int>(
			MemoryMarshal.CreateReadOnlySpan(ref scaled, 1));

		long sum = 0L, factor = 1L;
		foreach (var i in ints)
		{
			sum += i * factor;
			factor *= 10_000;
		}

		return (sum + isNegSign, index);
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

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static (Dictionary<TState, int>, TState, int) Dijkstra<TState>(
		TState start,
		Func<TState, IEnumerable<(TState state, int cost)>> getNextStates,
		Func<Dictionary<TState, int>, TState, bool> endingCondition)
	{
		var totalCost = new Dictionary<TState, int>();
		var pq = new UpdateablePriorityQueue<TState>();
		pq.Enqueue(start, 0);

		(TState p, int cost) = (default, default);
		while (pq.Count != 0)
		{
			pq.TryDequeue(out p, out cost);

			while (pq.Count != 0 && totalCost.ContainsKey(p))
				pq.TryDequeue(out p, out cost);

			totalCost[p] = cost;
			if (endingCondition(totalCost, p))
				break;

			pq.EnqueueRange(getNextStates(p)
				.Select(s => (s.state, cost + s.cost)));
		}

		return (totalCost, p, cost);
	}

	public static IEnumerable<((int x, int y) p, T item)> GetMapPoints<T>(
			this IReadOnlyList<IReadOnlyList<T>> map) =>
		 Enumerable.Range(0, map.Count)
			.SelectMany(y => Enumerable.Range(0, map[y].Count)
				.Select(x => ((x, y), map[y][x])));

	private static readonly (int x, int y)[] Directions = 
		new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0), };
	public static IEnumerable<(int x, int y)> GetCartesianNeighbors(this (int x, int y) p) =>
		Directions.Select(d => (p.x + d.x, p.y + d.y));

	public static IEnumerable<(int x, int y)> GetCartesianNeighbors<T>(
			this (int x, int y) p,
			IReadOnlyList<IReadOnlyList<T>> map) =>
		p.GetCartesianNeighbors()
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
		MoreEnumerable
			.TraverseBreadthFirst(
				startingPoint,
				Traverse)
			.Consume();
	}
}
