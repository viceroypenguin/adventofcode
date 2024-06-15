namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 20, CodeType.Original)]
public partial class Day_20_Original : IPuzzle
{
	private sealed record Node
	{
		public long Value { get; set; }
		public Node NextInt { get; set; } = default!;
		public Node NextNode { get; set; } = default!;
		public Node PrevNode { get; set; } = default!;
	}

#pragma warning disable IDE0051 // Remove unused private members
	private static string OutputNodes(Node node, int count) =>
		string.Join(", ",
			SuperEnumerable.Generate(node, n => n.NextNode)
				.Take(count)
				.Select(n => n.Value));
#pragma warning restore IDE0051 // Remove unused private members

	public (string part1, string part2) Solve(PuzzleInput input) =>
		(
			DoPart(input.Lines, false).ToString(),
			DoPart(input.Lines, true).ToString());

	private static long DoPart(string[] lines, bool part2)
	{
		var key = part2 ? 811589153L : 1;
		var (node, zero) = BuildNodes(lines, key);
		var intNode = node;
		var count = lines.Length;
		if (part2) count *= 10;

		var len = lines.Length - 1;

		for (var i = 0; i < count; i++)
		{
			var nextNode = node = intNode;
			intNode = intNode.NextInt;

			var n = node.Value + (key * len * 2);
			n %= len;

			if (n == 0)
				continue;

			if (n <= len / 2)
			{
				for (var j = 0; j < n; j++)
					nextNode = nextNode.NextNode;
			}
			else
			{
				n = len - n;
				for (var j = 0; j <= n; j++)
					nextNode = nextNode.PrevNode;
			}

			node.NextNode.PrevNode = node.PrevNode;
			node.PrevNode.NextNode = node.NextNode;

			node.NextNode = nextNode.NextNode;
			nextNode.NextNode = node;
			node.PrevNode = nextNode;
			node.NextNode.PrevNode = node;
		}

		var sum = 0L;
		node = zero;

		for (var i = 0; i < 1000; i++)
			node = node.NextNode;
		sum += node.Value;
		for (var i = 1000; i < 2000; i++)
			node = node.NextNode;
		sum += node.Value;
		for (var i = 2000; i < 3000; i++)
			node = node.NextNode;
		sum += node.Value;

		return sum;
	}

	private static (Node start, Node zero) BuildNodes(string[] lines, long key)
	{
		Node node = default!;
		Node first = default!;
		Node zero = default!;
		foreach (var n in lines.Select(int.Parse))
		{
			var nn = new Node
			{
				Value = n * key,
				PrevNode = node,
			};

			if (node != null)
				node.NextNode = node.NextInt = nn;
			else
				first = nn;

			node = nn;

			if (n == 0)
				zero = nn;
		}

		node.NextNode = node.NextInt = first;
		first.PrevNode = node;

		return (first, zero);
	}
}
