using System.Runtime.CompilerServices;

namespace AdventOfCode.Common;

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0051 // Remove unused private members

[InlineArray(4)]
public struct ValueArray4<T> : IEquatable<ValueArray4<T>> where T : IEquatable<T>
{
	private T _0;

	public override readonly bool Equals(object? obj) =>
		obj is ValueArray4<T> va && Equals(va);

	public readonly bool Equals(ValueArray4<T> other) =>
		EqualityComparer<T>.Default.Equals(this[0], other[0])
		&& EqualityComparer<T>.Default.Equals(this[1], other[1])
		&& EqualityComparer<T>.Default.Equals(this[2], other[2])
		&& EqualityComparer<T>.Default.Equals(this[3], other[3]);

	public override readonly int GetHashCode() =>
		HashCode.Combine(
			EqualityComparer<T>.Default.GetHashCode(this[0]),
			EqualityComparer<T>.Default.GetHashCode(this[1]),
			EqualityComparer<T>.Default.GetHashCode(this[2]),
			EqualityComparer<T>.Default.GetHashCode(this[3])
		);

	public static bool operator ==(ValueArray4<T> left, ValueArray4<T> right) =>
		left.Equals(right);

	public static bool operator !=(ValueArray4<T> left, ValueArray4<T> right) =>
		!(left == right);
}

[InlineArray(8)]
public struct ValueArray8<T> : IEquatable<ValueArray8<T>> where T : IEquatable<T>
{
	private T _0;

	public override readonly bool Equals(object? obj) =>
		obj is ValueArray8<T> va && Equals(va);

	public readonly bool Equals(ValueArray8<T> other) =>
		EqualityComparer<T>.Default.Equals(this[0], other[0])
		&& EqualityComparer<T>.Default.Equals(this[1], other[1])
		&& EqualityComparer<T>.Default.Equals(this[2], other[2])
		&& EqualityComparer<T>.Default.Equals(this[3], other[3])
		&& EqualityComparer<T>.Default.Equals(this[4], other[4])
		&& EqualityComparer<T>.Default.Equals(this[5], other[5])
		&& EqualityComparer<T>.Default.Equals(this[6], other[6])
		&& EqualityComparer<T>.Default.Equals(this[7], other[7]);

	public override readonly int GetHashCode() =>
		HashCode.Combine(
			EqualityComparer<T>.Default.GetHashCode(this[0]),
			EqualityComparer<T>.Default.GetHashCode(this[1]),
			EqualityComparer<T>.Default.GetHashCode(this[2]),
			EqualityComparer<T>.Default.GetHashCode(this[3]),
			EqualityComparer<T>.Default.GetHashCode(this[4]),
			EqualityComparer<T>.Default.GetHashCode(this[5]),
			EqualityComparer<T>.Default.GetHashCode(this[6]),
			EqualityComparer<T>.Default.GetHashCode(this[7])
		);

	public static bool operator ==(ValueArray8<T> left, ValueArray8<T> right) =>
		left.Equals(right);

	public static bool operator !=(ValueArray8<T> left, ValueArray8<T> right) =>
		!(left == right);
}
