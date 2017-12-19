<Query Kind="Statements" />

var input = 370;
var loop_count = 50_000_001;

var list = new LinkedList<int>();
var position = list.AddFirst(0);
Func<LinkedListNode<int>, LinkedListNode<int>> next = node =>
{
	return node.Next ?? list.First;
};

for (int i = 1; i < 2018; i++)
{
	for (int k = 0; k < input; k++)
		position = next(position);
	position = list.AddAfter(position, i);
}

next(position).Value.Dump("Part A");

for (int i = 2018; i < loop_count; i++)
{
	for (int k = 0; k < input; k++)
		position = next(position);
	position = list.AddAfter(position, i);
}

list.Find(0).Next.Value.Dump("Part B");
