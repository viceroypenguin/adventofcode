<Query Kind="Program" />

void Main()
{
	var input =
@"Tristram to AlphaCentauri = 34
Tristram to Snowdin = 100
Tristram to Tambi = 63
Tristram to Faerun = 108
Tristram to Norrath = 111
Tristram to Straylight = 89
Tristram to Arbre = 132
AlphaCentauri to Snowdin = 4
AlphaCentauri to Tambi = 79
AlphaCentauri to Faerun = 44
AlphaCentauri to Norrath = 147
AlphaCentauri to Straylight = 133
AlphaCentauri to Arbre = 74
Snowdin to Tambi = 105
Snowdin to Faerun = 95
Snowdin to Norrath = 48
Snowdin to Straylight = 88
Snowdin to Arbre = 7
Tambi to Faerun = 68
Tambi to Norrath = 134
Tambi to Straylight = 107
Tambi to Arbre = 40
Faerun to Norrath = 11
Faerun to Straylight = 66
Faerun to Arbre = 144
Norrath to Straylight = 115
Norrath to Arbre = 135
Straylight to Arbre = 127";

	var edges = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
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
		.OrderByDescending(p => p.TotalDistance)
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
