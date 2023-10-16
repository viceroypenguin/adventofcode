namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 18, CodeType.Original)]
public class Day_18_Original : IPuzzle
{
	private sealed class SnailfishNode
	{
		public int? Number { get; set; }
		public SnailfishNode LeftChild { get; set; }
		public SnailfishNode RightChild { get; set; }

		public SnailfishNode LeftNode { get; set; }
		public SnailfishNode RightNode { get; set; }

		public long GetMagnitude() =>
			Number ?? ((LeftChild.GetMagnitude() * 3) + (RightChild.GetMagnitude() * 2));

		public override string ToString() =>
			Number != null ? Number.ToString() :
			$"[{LeftChild},{RightChild}]";

		public SnailfishNode Clone() =>
			new Cloner().Clone(this);

		private sealed class Cloner
		{
			private SnailfishNode clonerCurrent;
			public SnailfishNode Clone(SnailfishNode node)
			{
				if (node == null)
					return null;

				if (node.Number != null)
				{
					var clone = new SnailfishNode
					{
						Number = node.Number,
						LeftNode = clonerCurrent,
					};

					if (clonerCurrent != null)
						clonerCurrent.RightNode = clone;
					clonerCurrent = clone;

					return clone;
				}
				else
				{
					return new SnailfishNode
					{
						LeftChild = Clone(node.LeftChild),
						RightChild = Clone(node.RightChild),
					};
				}
			}
		}
	}

	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var numbers = input.Lines
			.Select(ParseLine)
			.ToList();

		var part1 = numbers
			.Aggregate(Add)
			.GetMagnitude()
			.ToString();

		var part2 = (
			from a in numbers
			from b in numbers
			where a != b
			select Add(a, b).GetMagnitude())
			.Max().ToString();

		return (part1, part2);
	}

	private static SnailfishNode ParseLine(string l) =>
		new SnailfishParser().ParseNode(l).node;

	private sealed class SnailfishParser
	{
		private SnailfishNode parserCurrent;
		public (SnailfishNode node, int idx) ParseNode(ReadOnlySpan<char> txt)
		{
			var node = new SnailfishNode();

			if (char.IsNumber(txt[0]))
			{
				node.LeftNode = parserCurrent;
				if (parserCurrent != null)
					parserCurrent.RightNode = node;
				parserCurrent = node;

				node.Number = txt[0] - '0';
				return (node, 1);
			}
			else
			{
				(node.LeftChild, var ll) = ParseNode(txt[1..]);
				(node.RightChild, var rl) = ParseNode(txt[(1 + ll + 1)..]);

				return (
					node,
					1 + ll + 1 + rl + 1);
			}
		}
	}

	private static SnailfishNode FindNode(SnailfishNode root, int level, Func<SnailfishNode, int, bool> predicate)
	{
#pragma warning disable IDE0046 // Convert to conditional expression
		if (predicate(root, level))
			return root;
		if (root.Number != null)
			return null;

		return FindNode(root.LeftChild, level + 1, predicate)
			?? FindNode(root.RightChild, level + 1, predicate);
#pragma warning restore IDE0046 // Convert to conditional expression
	}

	private static bool NeedExplode(SnailfishNode root, out SnailfishNode node) =>
		(node = FindNode(root, 0, static (n, l) =>
			l >= 4
			&& n.Number == null
			&& n.LeftChild.Number != null
			&& n.RightChild.Number != null)) != null;

	private static void Explode(SnailfishNode node)
	{
		var left = node.LeftChild.Number.Value;
		var right = node.RightChild.Number.Value;

		node.Number = 0;
		node.LeftNode = node.LeftChild.LeftNode;
		if (node.LeftNode != null)
		{
			node.LeftNode.RightNode = node;
			node.LeftNode.Number += left;
		}
		node.LeftChild = null;

		node.RightNode = node.RightChild.RightNode;
		if (node.RightNode != null)
		{
			node.RightNode.LeftNode = node;
			node.RightNode.Number += right;
		}
		node.RightChild = null;
	}

	private static bool NeedSplit(SnailfishNode root, out SnailfishNode node) =>
		(node = FindNode(root, 0, static (n, _) => n.Number >= 10)) != null;

	private static void Split(SnailfishNode node)
	{
		var leftNumber = node.Number / 2;
		var rightNumber = node.Number - leftNumber;
		node.Number = default;
		node.LeftChild = new SnailfishNode { LeftNode = node.LeftNode, Number = leftNumber, };
		node.RightChild = new SnailfishNode { RightNode = node.RightNode, Number = rightNumber, };

		node.LeftChild.RightNode = node.RightChild;
		node.RightChild.LeftNode = node.LeftChild;

		if (node.LeftNode != null)
			node.LeftNode.RightNode = node.LeftChild;
		if (node.RightNode != null)
			node.RightNode.LeftNode = node.RightChild;

		node.LeftNode = node.RightNode = null;
	}

	private static void Reduce(SnailfishNode root)
	{
		while (true)
		{
			if (NeedExplode(root, out var node))
				Explode(node);
			else if (NeedSplit(root, out node))
				Split(node);
			else
				break;
		}
	}

	private static SnailfishNode Add(SnailfishNode left, SnailfishNode right)
	{
		left = left.Clone();
		right = right.Clone();

		var leftLink = left;
		while (leftLink.RightChild != null)
			leftLink = leftLink.RightChild;
		var rightLink = right;
		while (rightLink.LeftChild != null)
			rightLink = rightLink.LeftChild;
		(leftLink.RightNode, rightLink.LeftNode) =
			(rightLink, leftLink);

		var node = new SnailfishNode { LeftChild = left, RightChild = right };
		Reduce(node);
		return node;
	}
}
