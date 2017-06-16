<Query Kind="Program" />

void Main()
{
	var input =
@"Alice would lose 57 happiness units by sitting next to Bob.
Alice would lose 62 happiness units by sitting next to Carol.
Alice would lose 75 happiness units by sitting next to David.
Alice would gain 71 happiness units by sitting next to Eric.
Alice would lose 22 happiness units by sitting next to Frank.
Alice would lose 23 happiness units by sitting next to George.
Alice would lose 76 happiness units by sitting next to Mallory.
Bob would lose 14 happiness units by sitting next to Alice.
Bob would gain 48 happiness units by sitting next to Carol.
Bob would gain 89 happiness units by sitting next to David.
Bob would gain 86 happiness units by sitting next to Eric.
Bob would lose 2 happiness units by sitting next to Frank.
Bob would gain 27 happiness units by sitting next to George.
Bob would gain 19 happiness units by sitting next to Mallory.
Carol would gain 37 happiness units by sitting next to Alice.
Carol would gain 45 happiness units by sitting next to Bob.
Carol would gain 24 happiness units by sitting next to David.
Carol would gain 5 happiness units by sitting next to Eric.
Carol would lose 68 happiness units by sitting next to Frank.
Carol would lose 25 happiness units by sitting next to George.
Carol would gain 30 happiness units by sitting next to Mallory.
David would lose 51 happiness units by sitting next to Alice.
David would gain 34 happiness units by sitting next to Bob.
David would gain 99 happiness units by sitting next to Carol.
David would gain 91 happiness units by sitting next to Eric.
David would lose 38 happiness units by sitting next to Frank.
David would gain 60 happiness units by sitting next to George.
David would lose 63 happiness units by sitting next to Mallory.
Eric would gain 23 happiness units by sitting next to Alice.
Eric would lose 69 happiness units by sitting next to Bob.
Eric would lose 33 happiness units by sitting next to Carol.
Eric would lose 47 happiness units by sitting next to David.
Eric would gain 75 happiness units by sitting next to Frank.
Eric would gain 82 happiness units by sitting next to George.
Eric would gain 13 happiness units by sitting next to Mallory.
Frank would gain 77 happiness units by sitting next to Alice.
Frank would gain 27 happiness units by sitting next to Bob.
Frank would lose 87 happiness units by sitting next to Carol.
Frank would gain 74 happiness units by sitting next to David.
Frank would lose 41 happiness units by sitting next to Eric.
Frank would lose 99 happiness units by sitting next to George.
Frank would gain 26 happiness units by sitting next to Mallory.
George would lose 63 happiness units by sitting next to Alice.
George would lose 51 happiness units by sitting next to Bob.
George would lose 60 happiness units by sitting next to Carol.
George would gain 30 happiness units by sitting next to David.
George would lose 100 happiness units by sitting next to Eric.
George would lose 63 happiness units by sitting next to Frank.
George would gain 57 happiness units by sitting next to Mallory.
Mallory would lose 71 happiness units by sitting next to Alice.
Mallory would lose 28 happiness units by sitting next to Bob.
Mallory would lose 10 happiness units by sitting next to Carol.
Mallory would gain 44 happiness units by sitting next to David.
Mallory would gain 22 happiness units by sitting next to Eric.
Mallory would gain 79 happiness units by sitting next to Frank.
Mallory would lose 16 happiness units by sitting next to George.";

	var edges = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(x =>
		{
			var splits = x.Split();
			return new { Start = splits[0], End = splits[10].TrimEnd('.'), Distance = Convert.ToInt32(splits[3]) * (splits[2] == "gain" ? +1 : -1) };
		})
		.OrderBy(x => x.Distance)
		.ToList();

	var points = edges.Select(x => x.Start).Concat(edges.Select(x => x.End)).Distinct().Concat(new[] { "myself" }).ToList();

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
		.Dump();

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