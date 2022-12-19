namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 20, CodeType.Original)]
public class Day_20_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		return (string.Empty, string.Empty);
		// Apparently, I checked in incomplete code?
		//var data = input.GetString();

		//var map = new Dictionary<(int x, int y), (bool n, bool e, bool s, bool w)>();
		//void SetDoor((int x, int y) loc, bool n = false, bool e = false, bool s = false, bool w = false)
		//{
		//	map.TryGetValue(loc, out var walls);

		//	if (n)
		//		walls.n = true;
		//	if (e)
		//		walls.e = true;
		//	if (s)
		//		walls.s = true;
		//	if (w)
		//		walls.w = true;

		//	map[loc] = walls;
		//}

		//var location = (x: 0, y: 0);
		//var stack = new Stack<(int x, int y)>();
		//foreach (var ch in data)
		//{
		//	switch (ch)
		//	{
		//		case 'N':
		//		case 'E':
		//		case 'S':
		//		case 'W':
		//			SetDoor(location, ch);
		//			break;

		//		case '^':
		//			break;

		//		case '(':
		//			stack.Push(location);
		//			break;

		//		case ')':
		//			location = stack.Pop();
		//			break;

		//		case '|':

		//	}
		//}
	}
}
