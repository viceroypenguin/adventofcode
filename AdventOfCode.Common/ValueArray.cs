using System.Runtime.InteropServices;

namespace AdventOfCode.Common;

[StructLayout(LayoutKind.Sequential)]
public record struct ValueArray4<T>
{
	public T _0, _1, _2, _3;

	public readonly T this[int i] =>
		i switch
		{
			0 => _0,
			1 => _1,
			2 => _2,
			3 => _3,
			_ => throw new ArgumentOutOfRangeException(nameof(i)),
		};
}

[StructLayout(LayoutKind.Sequential)]
public record struct ValueArray8<T>
{
	public T _0, _1, _2, _3, _4, _5, _6, _7;

	public readonly T this[int i] =>
		i switch
		{
			0 => _0,
			1 => _1,
			2 => _2,
			3 => _3,
			4 => _4,
			5 => _5,
			6 => _6,
			7 => _7,
			_ => throw new ArgumentOutOfRangeException(nameof(i)),
		};
}

public static class ValueArrayExtensions
{
	public static Span<T> Span<T>(ref this ValueArray4<T> array) =>
		MemoryMarshal.CreateSpan(ref array._0, 4);

	public static Span<T> Span<T>(ref this ValueArray8<T> array) =>
		MemoryMarshal.CreateSpan(ref array._0, 8);
}
