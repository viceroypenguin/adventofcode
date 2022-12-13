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

	private class ProjectionEqualityComparerImpl<T> : EqualityComparer<T>
	{
		/// <summary>
		/// Type specific equality function
		/// </summary>
		private readonly Func<T?, T?, bool> equalityFunction;

		/// <summary>
		/// Type specific hash function
		/// </summary>
		private readonly Func<T, int> hashFunction;

		/// <summary>
		/// Creates a new instance of an equality comparer using the provided functions
		/// </summary>
		/// <param name="equalityFunction">The equality function</param>
		/// <param name="hashFunction">The hash function</param>
		public ProjectionEqualityComparerImpl(Func<T?, T?, bool> equalityFunction, Func<T, int> hashFunction)
		{
			this.equalityFunction = equalityFunction ?? throw new ArgumentNullException(nameof(equalityFunction));
			this.hashFunction = hashFunction ?? throw new ArgumentNullException(nameof(hashFunction));
		}

		/// <inheritdoc/>
		public override bool Equals(T? x, T? y) => this.equalityFunction(x, y);

		/// <inheritdoc/>
		public override int GetHashCode(T obj) => this.hashFunction(obj);
	}
}
