namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 09, CodeType.Original)]
public class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Text.Split();
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

		var i = 0;
		void DoLoop()
		{
			if (i % 23 == 0)
			{
				for (var j = 0; j < 7; j++)
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

		var part1 = players.Max().ToString();

		maxPoints *= 100;
		for (; i <= maxPoints; i++)
			DoLoop();

		var part2 = players.Max().ToString();

		return (part1, part2);
	}
}
