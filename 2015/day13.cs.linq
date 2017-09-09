<Query Kind="Program" />

void Main()
{
	var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day13.input.txt")); ;

	var edges = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(x =>
		{
			var splits = x.Split();
			return new { Start = splits[0], End = splits[10].TrimEnd('.'), Distance = Convert.ToInt32(splits[3]) * (splits[2] == "gain" ? +1 : -1) };
		})
		.OrderBy(x => x.Distance)
		.ToList();

	var points = edges.Select(x => x.Start).Concat(edges.Select(x => x.End)).Distinct().ToList();

	var paths = Permutation.HamiltonianPermutations(points.Count - 1)
		.Select(p =>
			new[] { p.Count }.Concat(p).Concat(new[] { p.Count })
			.Select(i => points[i])
			.ToList())
		.Select(p => p.Zip(p.Skip(1), (x, y) => new { Start = x, End = y }))
		.Select(p => p.Select(e => new
		{
			e.Start,
			e.End,
			Distance = edges.Where(_ => (_.Start == e.Start && _.End == e.End) || (_.Start == e.End && _.End == e.Start)).Sum(_ => _.Distance),
		}).ToList())
		.Select(p => new { Path = p, TotalDistance = p.Sum(e => e.Distance) })
		.OrderByDescending(p => p.TotalDistance)
		.Take(1)
		.Dump("Part A");

	points = edges.Select(x => x.Start).Concat(edges.Select(x => x.End)).Concat(new[] { "myself" }).Distinct().ToList();

	paths = Permutation.HamiltonianPermutations(points.Count - 1)
		.Select(p =>
			new[] { p.Count }.Concat(p).Concat(new[] { p.Count })
			.Select(i => points[i])
			.ToList())
		.Select(p => p.Zip(p.Skip(1), (x, y) => new { Start = x, End = y }))
		.Select(p => p.Select(e => new
		{
			e.Start,
			e.End,
			Distance = edges.Where(_ => (_.Start == e.Start && _.End == e.End) || (_.Start == e.End && _.End == e.Start)).Sum(_ => _.Distance),
		}).ToList())
		.Select(p => new { Path = p, TotalDistance = p.Sum(e => e.Distance) })
		.OrderByDescending(p => p.TotalDistance)
		.Take(1)
		.Dump("Part B ");
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