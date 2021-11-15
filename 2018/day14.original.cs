namespace AdventOfCode;

public class Day_2018_14_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 14;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
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
			foreach (var n in MoreEnumerable
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

		var numRecipes = Convert.ToInt32(input.GetString());
		while (marbles.Count < numRecipes + 10)
			DoTick();

		Dump('A',
			string.Join(
				"",
				MoreEnumerable
					.Generate(
						marbles.First,
						n => nextNode(n))
					.Skip(numRecipes)
					.Take(10)
					.Select(n => n.Value)));

		marbles.Clear();
		AddNumber(numRecipes);
		var matchList = marbles.Reverse().ToList();

		marbles.Clear();
		AddNumber(37);
		elf1 = marbles.First;
		elf2 = nextNode(elf1);

		while (true)
		{
			DoTick();

			if (MoreEnumerable
				.Generate(
					marbles.Last,
					n => prevNode(n))
				.Take(matchList.Count)
				.Select(n => n.Value)
				.SequenceEqual(matchList))
			{
				Dump('B', marbles.Count - matchList.Count);
				return;
			}

			if (MoreEnumerable
				.Generate(
					marbles.Last,
					n => prevNode(n))
				.Skip(1)
				.Take(matchList.Count)
				.Select(n => n.Value)
				.SequenceEqual(matchList))
			{
				Dump('B', marbles.Count - matchList.Count - 1);
				return;
			}
		}
	}
}
