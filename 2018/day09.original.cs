namespace AdventOfCode;

public class Day_2018_09_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 9;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var data = input.GetString().Split();
		var numPlayers = Convert.ToInt32(data[0]);
		var maxPoints = Convert.ToInt32(data[6]);

		var players = Enumerable.Range(0, numPlayers).Select(_ => 0L).ToArray();
		var marbles = new LinkedList<int>();
		marbles.AddFirst(0);

		var current = marbles.First;

		LinkedListNode<int> nextNode(LinkedListNode<int> node) =>
			node.Next ?? marbles.First;
		LinkedListNode<int> prevNode(LinkedListNode<int> node) =>
			node.Previous ?? marbles.Last;

		int i = 0;
		void DoLoop()
		{
			if (i % 23 == 0)
			{
				for (int j = 0; j < 7; j++)
					current = prevNode(current);

				var player = i % numPlayers;
				players[player] += (i + current.Next.Value);
				marbles.Remove(current.Next);
			}
			else
			{
				current = nextNode(current);
				current = nextNode(current);
				marbles.AddAfter(current, i);
			}
		}
		for (i = 1; i <= maxPoints; i++)
			DoLoop();

		Dump('A', players.Max());

		maxPoints *= 100;
		for (; i <= maxPoints; i++)
			DoLoop();

		Dump('B', players.Max());
	}
}
