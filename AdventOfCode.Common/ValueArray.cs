using System.Runtime.CompilerServices;

namespace AdventOfCode.Common;

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0051 // Remove unused private members

[InlineArray(4)]
public record struct ValueArray4<T>
{
	private T _0;
}

[InlineArray(8)]
public record struct ValueArray8<T>
{
	private T _0;
}
