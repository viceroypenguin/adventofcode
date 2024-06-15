namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 22, CodeType.Original)]
public class Day_22_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Lines;
		var depth = Convert.ToInt32(data[0].Split()[1]);
		var coordStr = data[1].Split()[1].Split(',');
		var destination = (x: Convert.ToInt32(coordStr[0]), y: Convert.ToInt32(coordStr[1]));
		const int Margin = 100;
		const int Modulo = 20183;

		var ground = Enumerable.Range(0, destination.y + Margin)
			.Select(x => Enumerable.Repeat(0, destination.x + Margin).ToArray())
			.ToArray();

		for (var x = 0; x < destination.x + Margin; x++)
			ground[0][x] = (((x % Modulo) * 16807) + depth) % Modulo;

		for (var y = 0; y < destination.y + Margin; y++)
			ground[y][0] = (((y % Modulo) * (48271 % Modulo)) + depth) % Modulo;

		for (var y = 1; y < destination.y + Margin; y++)
		{
			var curRow = ground[y];
			var prevRow = ground[y - 1];

			for (var x = 1; x < destination.x + Margin; x++)
			{
				curRow[x] = x == destination.x && y == destination.y
					? depth % Modulo
					: ((prevRow[x] * curRow[x - 1]) + depth) % Modulo;
			}
		}

		var part1 = ground.Take(destination.y + 1)
			.SelectMany(y => y.Take(destination.x + 1))
			.GroupBy(y => y % 3)
			.Sum(g => g.Key * g.Count())
			.ToString();

		const int Neither = 0;
		const int Torch = 1;
		const int Gear = 2;

		IEnumerable<((int, int, int), int)> GetNeighbors((int x, int y, int equip) pos, int cost)
		{
			var states = new List<((int, int, int), int)>();

			AddNeighbor(pos.x + 1, pos.y, pos.equip, cost + 1);
			AddNeighbor(pos.x, pos.y + 1, pos.equip, cost + 1);
			AddNeighbor(pos.x - 1, pos.y, pos.equip, cost + 1);
			AddNeighbor(pos.x, pos.y - 1, pos.equip, cost + 1);
			if (pos.equip != Neither) AddNeighbor(pos.x, pos.y, Neither, cost + 7);
			if (pos.equip != Torch) AddNeighbor(pos.x, pos.y, Torch, cost + 7);
			if (pos.equip != Gear) AddNeighbor(pos.x, pos.y, Gear, cost + 7);

			return states;

			void AddNeighbor(int x, int y, int equip, int newCost)
			{
				if (x < 0)
					return;
				if (y < 0)
					return;
				if (x >= destination.x + Margin)
					return;
				if (y >= destination.y + Margin)
					return;

				var type = ground[y][x] % 3;
				switch (type)
				{
					case 0 when equip is Torch or Gear:
						states.Add(((x, y, equip), newCost));
						return;

					case 1 when equip is Neither or Gear:
						states.Add(((x, y, equip), newCost));
						return;

					case 2 when equip is Neither or Torch:
						states.Add(((x, y, equip), newCost));
						return;

					default:
						return;
				}
			}
		}

		var cost = SuperEnumerable.GetShortestPathCost<(int, int, int), int>(
			(0, 0, Torch),
			GetNeighbors,
			(destination.x, destination.y, Torch));

		var part2 = cost.ToString();

		return (part1, part2);
	}
}
