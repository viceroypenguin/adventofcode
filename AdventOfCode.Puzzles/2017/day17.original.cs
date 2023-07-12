namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 17, CodeType.Original)]
public class Day_17_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var key = Convert.ToInt32(input.Text);

		var list = new LinkedList<int>();
		var position = list.AddFirst(0);
		LinkedListNode<int> next(LinkedListNode<int> node) =>
			node.Next ?? list.First;

		for (var i = 1; i < 2018; i++)
		{
			for (var k = 0; k < key; k++)
				position = next(position);
			position = list.AddAfter(position, i);
		}

		return (
			next(position).Value.ToString(),
			string.Empty);

		// TotalMicroseconds = 839_018_162;

		// this is *such* a bad algorithm. leaving my shame for posterity
		// Year 2017, Day 17, Type  Original      :   839,018,162 µs

		//var loop_count = 50_000_001;
		//for (int i = 2018; i < loop_count; i++)
		//{
		//	for (int k = 0; k < key; k++)
		//		position = next(position);
		//	position = list.AddAfter(position, i);
		//}

		//Dump('B', list.Find(0).Next.Value);
	}
}
