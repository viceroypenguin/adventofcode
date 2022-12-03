using System.Runtime.CompilerServices;

namespace AdventOfCode.Common;

public static class Helpers
{
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
}
