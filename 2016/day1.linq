<Query Kind="Statements" />

var input = "L2, L5, L5, R5, L2, L4, R1, R1, L4, R2, R1, L1, L4, R1, L4, L4, R5, R3, R1, L1, R1, L5, L1, R5, L4, R2, L5, L3, L3, R3, L3, R4, R4, L2, L5, R1, R2, L2, L1, R3, R4, L193, R3, L5, R45, L1, R4, R79, L5, L5, R5, R1, L4, R3, R3, L4, R185, L5, L3, L1, R5, L2, R1, R3, R2, L3, L4, L2, R2, L3, L2, L2, L3, L5, R3, R4, L5, R1, R2, L2, R4, R3, L4, L3, L1, R3, R2, R1, R1, L3, R4, L5, R2, R1, R3, L3, L2, L2, R2, R1, R2, R3, L3, L3, R4, L4, R4, R4, R4, L3, L1, L2, R5, R2, R2, R2, L4, L3, L4, R4, L5, L4, R2, L4, L4, R4, R1, R5, L2, L4, L5, L3, L2, L4, L4, R3, L3, L4, R1, L2, R3, L2, R1, R2, R5, L4, L2, L1, L3, R2, R3, L2, L1, L5, L2, L1, R4";

var instructions = input.Split(',').Select(x=>x.Trim()).ToList();
var position = new { X = 0, Y = 0, Direction = 0, };

var set = new HashSet<Tuple<int, int>>(new [] { Tuple.Create(position.X, position.Y), });
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
			path.AddRange(Enumerable.Range(position.Y + 1, distance).Select(y=>Tuple.Create(position.X, y)));
			position = new { X = position.X, Y = position.Y + distance, Direction = newDirection };
			break;
		
		case 1: // E
			path.AddRange(Enumerable.Range(position.X + 1, distance).Select(x=>Tuple.Create(x, position.Y)));
			position = new { X = position.X + distance, Y = position.Y, Direction = newDirection };
			break;
		
		case 2: // S
			path.AddRange(Enumerable.Range(position.Y - distance, distance).Select(y=>Tuple.Create(position.X, y)).Reverse());
			position = new { X = position.X, Y = position.Y - distance, Direction = newDirection };
			break;
		
		case 3: // W
			path.AddRange(Enumerable.Range(position.X - distance, distance).Select(x=>Tuple.Create(x, position.Y)).Reverse());
			position = new { X = position.X - distance, Y = position.Y, Direction = newDirection };
			break;
	}
	
	// part A: ignore this foreach
	foreach (var p in path)
	{
		if (set.Contains(p))
		{
			(Math.Abs(p.Item1) + Math.Abs(p.Item2)).Dump();
			return;
		}
		else set.Add(p);
	}
}

(Math.Abs(position.X) + Math.Abs(position.Y)).Dump();
