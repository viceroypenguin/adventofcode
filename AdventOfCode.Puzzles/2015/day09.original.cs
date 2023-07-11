namespace AdventOfCode;

public class Day_2015_09_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 9;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var edges = input.GetLines()
			.Select(x => x.Split(new[] { " to ", " = " }, StringSplitOptions.None))
			.Select(x => new { Start = x[0], End = x[1], Distance = Convert.ToInt32(x[2]) })
			.OrderBy(x => x.Distance)
			.ToList();

		var points = edges.Select(x => x.Start).Concat(edges.Select(x => x.End)).Distinct().ToList();

		var paths = Permutation.HamiltonianPermutations(points.Count)
			.Select(p => p
				.Select(i => points[i])
				.ToList())
			.Select(p => p.Zip(p.Skip(1), (x, y) => new { Start = x, End = y }))
			.Select(p => p.Select(e => new
			{
				e.Start,
				e.End,
				Distance = edges.SingleOrDefault(_ => (_.Start == e.Start && _.End == e.End) || (_.Start == e.End && _.End == e.Start))?.Distance
			}).ToList())
			.Select(p => new { Path = p, TotalDistance = p.Sum(e => e.Distance) })
			.ToList();

		Dump('A',
			paths
				.OrderBy(p => p.TotalDistance)
				.First()
				.TotalDistance);

		Dump('B',
			paths
				.OrderByDescending(p => p.TotalDistance)
				.First()
				.TotalDistance);
	}

	struct Permutation : IEnumerable<int>
	{
		public static Permutation Empty { get { return empty; } }
		private static Permutation empty = new Permutation(new int[] { });
		private int[] permutation;
		private Permutation(int[] permutation)
		{
			this.permutation = permutation;
		}
		private Permutation(IEnumerable<int> permutation)
			: this(permutation.ToArray())
		{ }
		public static IEnumerable<Permutation> HamiltonianPermutations(int n)
		{
			if (n < 0)
				throw new ArgumentOutOfRangeException("n");
			return HamiltonianPermutationsIterator(n);
		}
		private static IEnumerable<Permutation> HamiltonianPermutationsIterator(int n)
		{
			if (n == 0)
			{
				yield return Empty;
				yield break;
			}
			bool forwards = false;
			foreach (Permutation permutation in HamiltonianPermutationsIterator(n - 1))
			{
				for (int index = 0; index < n; index += 1)
				{
					yield return new Permutation(
						InsertAt(permutation, forwards ? index : n - index - 1, n - 1));
				}
				forwards = !forwards;
			}
		}
		public static IEnumerable<T> InsertAt<T>(
		  IEnumerable<T> items, int position, T newItem)
		{
			if (items == null)
				throw new ArgumentNullException("items");
			if (position < 0)
				throw new ArgumentOutOfRangeException("position");
			return InsertAtIterator<T>(items, position, newItem);
		}

		private static IEnumerable<T> InsertAtIterator<T>(
			IEnumerable<T> items, int position, T newItem)
		{
			int index = 0;
			bool yieldedNew = false;
			foreach (T item in items)
			{
				if (index == position)
				{
					yield return newItem;
					yieldedNew = true;
				}
				yield return item;
				index += 1;
			}
			if (index == position)
			{
				yield return newItem;
				yieldedNew = true;
			}
			if (!yieldedNew)
				throw new ArgumentOutOfRangeException("position");
		}
		public int this[int index]
		{
			get { return permutation[index]; }
		}
		public IEnumerator<int> GetEnumerator()
		{
			foreach (int item in permutation)
				yield return item;
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		public int Count { get { return this.permutation.Length; } }
		public override string ToString()
		{
			return string.Join<int>(",", permutation);
		}
	}
}
