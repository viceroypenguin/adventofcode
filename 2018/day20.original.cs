namespace AdventOfCode;

public class Day_2018_20_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 20;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
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
