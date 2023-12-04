using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Common.Extensions;

public static class NumberExtensions
{
	public static bool Between<T>(this T value, T min, T max) where T : IBinaryNumber<T>
	{
		if (min > max) (min, max) = (max, min);
		return min <= value && value <= max;
	}

	public static bool Between(this byte value, byte min, byte max)
	{
		return (uint)(value - min) <= (uint)(max - min);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static long Gcd(long a, long b)
	{
		while (b != 0) b = a % (a = b);
		return a;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static long Lcm(long a, long b) =>
		a * b / Gcd(a, b);

	public static int DivRoundUp(this int a, int b) =>
		(a + b - 1) / b;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (int value, int numChars) AtoI(this ReadOnlySpan<byte> bytes)
	{
		var n = 0;

		// initial sign? also, track negative
		var isNegSign = bytes[0] == '-';
		if (isNegSign || bytes[0] == '+')
			bytes = bytes[++n..];

		var value = 0;
		foreach (var t in bytes)
		{
			if (t is < (byte)'0' or > (byte)'9')
				break;

			value = (value * 10) + (t - '0');
			n++;
		}

		return (isNegSign ? -value : value, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (int value, int numChars) AtoI(this ReadOnlySpan<char> bytes)
	{
		var n = 0;

		// initial sign? also, track negative
		var isNegSign = bytes[0] == '-';
		if (isNegSign || bytes[0] == '+')
			bytes = bytes[++n..];

		var value = 0;
		foreach (var t in bytes)
		{
			if (t is < '0' or > '9')
				break;

			value = (value * 10) + (t - '0');
			n++;
		}

		return (isNegSign ? -value : value, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (long value, int numChars) AtoL(this ReadOnlySpan<byte> bytes)
	{
		var n = 0;

		// initial sign? also, track negative
		var isNegSign = bytes[0] == '-';
		if (isNegSign || bytes[0] == '+')
			bytes = bytes[++n..];

		var value = 0L;
		foreach (var t in bytes)
		{
			if (t is < (byte)'0' or > (byte)'9')
				break;

			value = (value * 10) + (t - '0');
			n++;
		}

		return (isNegSign ? -value : value, n);
	}
}
