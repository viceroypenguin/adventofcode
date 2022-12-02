using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode;

public class Day_2021_07_Fastest : Day
{
	public override int Year => 2021;
	public override int DayNumber => 7;
	public override CodeType CodeType => CodeType.Fastest;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		// keep track of how many crabs at each position 0..2048
		// 2048 appears max based on data
		Span<int> crabs = stackalloc int[2048];

		// read data from input
		var span = new ReadOnlySpan<byte>(input);
		int i = 0, cnt = 0, max = 0;
		while (i < span.Length)
		{
			var (value, numChars) = span[i..].AtoI();
			// increment number of crabs at position
			crabs[value]++;

			max = Math.Max(max, value);
			cnt++;
			i += numChars + 1;
		}

		// round up max to next 8
		max = (max + 7) & ~0b111;
		// cut span short
		crabs = crabs[..max];

		var half = cnt / 2;
		var sum = 0;
		for (i = 0; sum < half; i++)
			sum += crabs[i];
		var median = (i - 1);
		PartA = PartAFuelTotal(crabs, median).ToString();

		sum = IndexSum(crabs);
		var avg = sum / cnt;

		var n1 = PartBFuelTotal(crabs, avg - 1);
		var n2 = PartBFuelTotal(crabs, avg);
		var n3 = PartBFuelTotal(crabs, avg + 1);

		PartB = Math.Min(Math.Min(n1, n2), n3).ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static int IndexSum(Span<int> arr)
	{
		// cache loop constants
		var eight = Vector256.Create(8);

		// convert int array to simd array
		var simdCrabs = MemoryMarshal.Cast<int, Vector256<int>>(arr);
		// start w/ 0 in every lane
		var simdSum = Vector256<int>.Zero;
		// keep track of index aka position
		var index = Vector256.Create(0, 1, 2, 3, 4, 5, 6, 7);

		for (int i = 0; i < arr.Length; i += Vector256<int>.Count)
		{
			simdSum = Avx2.Add(simdSum,
				Avx2.MultiplyLow(index, simdCrabs[0]));

			simdCrabs = simdCrabs[1..];
			index = Avx2.Add(index, eight);
		}

		// coalesce to a single int value
		var tmp = Avx2.Add(Avx2.ExtractVector128(simdSum, 0), Avx2.ExtractVector128(simdSum, 1));
		tmp = Avx2.HorizontalAdd(tmp, tmp);
		tmp = Avx2.HorizontalAdd(tmp, tmp);
		return tmp.ToScalar();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static int PartAFuelTotal(Span<int> arr, int position)
	{
		// cache loop constants
		var simdPosition = Vector256.Create(position);
		var one = Vector256.Create(1);
		var eight = Vector256.Create(8);

		// convert int array to simd array
		var simdCrabs = MemoryMarshal.Cast<int, Vector256<int>>(arr);
		// start w/ 0 in every lane
		var simdSum = Vector256<int>.Zero;
		// keep track of index aka position
		var index = Vector256.Create(0, 1, 2, 3, 4, 5, 6, 7);
		// simd add
		for (int i = 0; i < arr.Length; i += Vector256<int>.Count)
		{
			var n = Avx2.Abs(Avx2.Subtract(index, simdPosition)).AsInt32();
			simdSum = Avx2.Add(simdSum,
				Avx2.MultiplyLow(n, simdCrabs[0]));

			simdCrabs = simdCrabs[1..];
			index = Avx2.Add(index, eight);
		}

		// coalesce to a single int value
		var tmp = Avx2.Add(Avx2.ExtractVector128(simdSum, 0), Avx2.ExtractVector128(simdSum, 1));
		tmp = Avx2.HorizontalAdd(tmp, tmp);
		tmp = Avx2.HorizontalAdd(tmp, tmp);
		return tmp.ToScalar();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static int PartBFuelTotal(Span<int> arr, int position)
	{
		// cache loop constants
		var simdPosition = Vector256.Create(position);
		var one = Vector256.Create(1);
		var eight = Vector256.Create(8);

		// convert int array to simd array
		var simdCrabs = MemoryMarshal.Cast<int, Vector256<int>>(arr);
		// start w/ 0 in every lane
		var simdSum = Vector256<int>.Zero;
		// keep track of index aka position
		var index = Vector256.Create(0, 1, 2, 3, 4, 5, 6, 7);
		// simd add
		for (int i = 0; i < arr.Length; i += Vector256<int>.Count)
		{
			var n = Avx2.Abs(Avx2.Subtract(index, simdPosition)).AsInt32();
			var fuel = Avx2.ShiftRightArithmetic(Avx2.MultiplyLow(n, Avx2.Add(n, one)), 1);
			simdSum = Avx2.Add(simdSum,
				Avx2.MultiplyLow(fuel, simdCrabs[0]));

			simdCrabs = simdCrabs[1..];
			index = Avx2.Add(index, eight);
		}

		// coalesce to a single int value
		var tmp = Avx2.Add(Avx2.ExtractVector128(simdSum, 0), Avx2.ExtractVector128(simdSum, 1));
		tmp = Avx2.HorizontalAdd(tmp, tmp);
		tmp = Avx2.HorizontalAdd(tmp, tmp);
		return tmp.ToScalar();
	}
}
