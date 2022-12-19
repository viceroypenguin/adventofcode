namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 14, CodeType.Original)]
public class Day_14_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var marbles = new LinkedList<int>();
		LinkedListNode<int> prevNode(LinkedListNode<int> node) =>
			node.Previous ?? marbles.Last;
		LinkedListNode<int> nextNode(LinkedListNode<int> node) =>
			node.Next ?? marbles.First;
		LinkedListNode<int> nextNodeCount(LinkedListNode<int> node, int count)
		{
			for (int i = 0; i < count; i++)
				node = nextNode(node);
			return node;
		}

		void AddNumber(int number)
		{
			foreach (var n in SuperEnumerable
				.Generate(
					number,
					n => n / 10)
				.TakeWhile(n => n != 0)
				.Reverse()
				.Select(n => n % 10)
				.DefaultIfEmpty(0))
			{
				marbles.AddLast(n);
			}
		}

		AddNumber(37);

		var elf1 = marbles.First;
		var elf2 = nextNode(elf1);

		void DoTick()
		{
			AddNumber(elf1.Value + elf2.Value);

			elf1 = nextNodeCount(elf1, elf1.Value + 1);
			elf2 = nextNodeCount(elf2, elf2.Value + 1);
		}

		var numRecipes = Convert.ToInt32(input.Text);
		while (marbles.Count < numRecipes + 10)
			DoTick();

		var part1 = string.Join(
				"",
				SuperEnumerable
					.Generate(
						marbles.First,
						n => nextNode(n))
					.Skip(numRecipes)
					.Take(10)
					.Select(n => n.Value));

		marbles.Clear();
		AddNumber(numRecipes);
		var matchList = marbles.Reverse().ToList();

		marbles.Clear();
		AddNumber(37);
		elf1 = marbles.First;
		elf2 = nextNode(elf1);

		var part2 = 0;
		while (true)
		{
			DoTick();

			if (SuperEnumerable
				.Generate(
					marbles.Last,
					n => prevNode(n))
				.Take(matchList.Count)
				.Select(n => n.Value)
				.SequenceEqual(matchList))
			{
				part2 = marbles.Count - matchList.Count;
				break;
			}

			if (SuperEnumerable
				.Generate(
					marbles.Last,
					n => prevNode(n))
				.Skip(1)
				.Take(matchList.Count)
				.Select(n => n.Value)
				.SequenceEqual(matchList))
			{
				part2 = marbles.Count - matchList.Count - 1;
				break;
			}
		}

		return (part1, part2.ToString());
	}
}
