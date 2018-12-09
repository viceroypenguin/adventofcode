<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day09.input.txt"))
	.Split();
var numPlayers = Convert.ToInt32(input[0]);
var maxPoints = Convert.ToInt32(input[6]);

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

players.Max().Dump("Part A");

maxPoints *= 100;
for (; i <= maxPoints; i++)
	DoLoop();

players.Max().Dump("Part B");
