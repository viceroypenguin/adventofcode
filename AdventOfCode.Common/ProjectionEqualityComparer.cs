namespace AdventOfCode.Common;

public static class ProjectionEqualityComparer
{
	/// <summary>
	/// Creates an equality comparer using the provided function
	/// </summary>
	/// <param name="equalityFunction">The equality function</param>
	/// <returns>The new comparer</returns>
	/// <remarks>
	/// The returned comparer may or may not be valid in a hashset or dictionary,
	/// depending on how the <paramref name="equalityFunction"/> relates to the
	/// default hashcode for <typeparamref name="T"/>.
	/// </remarks>
	public static IEqualityComparer<T> Create<T>(Func<T?, T?, bool> equalityFunction) =>
		new ProjectionEqualityComparerImpl<T>(equalityFunction, EqualityComparer<T>.Default.GetHashCode!);

	/// <summary>
	/// Creates an equality comparer using the provided functions
	/// </summary>
	/// <param name="equalityFunction">The equality function</param>
	/// <param name="hashFunction">The hash function</param>
	/// <returns>The new comparer</returns>
	public static IEqualityComparer<T> Create<T>(Func<T?, T?, bool> equalityFunction, Func<T, int> hashFunction) =>
		new ProjectionEqualityComparerImpl<T>(equalityFunction, hashFunction);

	/// <summary>
	/// Creates a new instance of an equality comparer using the provided functions
	/// </summary>
	/// <param name="equalityFunction">The equality function</param>
	/// <param name="hashFunction">The hash function</param>
	private sealed class ProjectionEqualityComparerImpl<T>(
		Func<T?, T?, bool> equalityFunction,
		Func<T, int> hashFunction
	) : EqualityComparer<T>
	{
		/// <inheritdoc/>
		public override bool Equals(T? x, T? y) => equalityFunction(x, y);

		/// <inheritdoc/>
		public override int GetHashCode(T obj) => hashFunction(obj);
	}
}
