namespace AdventOfCode;

public class Day_2016_01_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var instructions = input.GetString().Split(',').Select(x => x.Trim()).ToList();
		var position = new { X = 0, Y = 0, Direction = 0, };

		var set = new HashSet<Tuple<int, int>>(new[] { Tuple.Create(position.X, position.Y), });
		Tuple<int, int> firstVisited = null;

		foreach (var i in instructions)
		{
			var newDirection = i[0] == 'L' ?
				position.Direction - 1 :
				position.Direction + 1;

			newDirection = (newDirection + 4) % 4;
			var distance = Convert.ToInt32(i.Substring(1));
			List<Tuple<int, int>> path = new List<Tuple<int, int>>();
			switch (newDirection)
			{
				case 0: // N
					path.AddRange(Enumerable.Range(position.Y + 1, distance).Select(y => Tuple.Create(position.X, y)));
					position = new { X = position.X, Y = position.Y + distance, Direction = newDirection };
					break;

				case 1: // E
					path.AddRange(Enumerable.Range(position.X + 1, distance).Select(x => Tuple.Create(x, position.Y)));
					position = new { X = position.X + distance, Y = position.Y, Direction = newDirection };
					break;

				case 2: // S
					path.AddRange(Enumerable.Range(position.Y - distance, distance).Select(y => Tuple.Create(position.X, y)).Reverse());
					position = new { X = position.X, Y = position.Y - distance, Direction = newDirection };
					break;

				case 3: // W
					path.AddRange(Enumerable.Range(position.X - distance, distance).Select(x => Tuple.Create(x, position.Y)).Reverse());
					position = new { X = position.X - distance, Y = position.Y, Direction = newDirection };
					break;
			}

			// part A: ignore this foreach
			if (firstVisited == null)
				foreach (var p in path)
				{
					if (set.Contains(p))
					{
						firstVisited = p;
						break;
					}
					else set.Add(p);
				}
		}

		Dump('A', Math.Abs(position.X) + Math.Abs(position.Y));
		Dump('B', Math.Abs(firstVisited.Item1) + Math.Abs(firstVisited.Item2));
	}
}
