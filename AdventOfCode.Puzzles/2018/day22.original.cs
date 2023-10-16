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
		const int margin = 100;
		const int modulo = 20183;

		var ground = Enumerable.Range(0, destination.y + margin)
			.Select(x => Enumerable.Repeat(0, destination.x + margin).ToArray())
			.ToArray();

		for (var x = 0; x < destination.x + margin; x++)
			ground[0][x] = ((x % modulo) * 16807 + depth) % modulo;

		for (var y = 0; y < destination.y + margin; y++)
			ground[y][0] = ((y % modulo) * (48271 % modulo) + depth) % modulo;

		for (var y = 1; y < destination.y + margin; y++)
		{
			var curRow = ground[y];
			var prevRow = ground[y - 1];

			for (var x = 1; x < destination.x + margin; x++)
			{
				if (x == destination.x && y == destination.y)
					curRow[x] = depth % modulo;
				else
					curRow[x] = (prevRow[x] * curRow[x - 1] + depth) % modulo;
			}
		}

		var part1 = ground.Take(destination.y + 1)
			.SelectMany(y => y.Take(destination.x + 1))
			.GroupBy(y => y % 3)
			.Sum(g => g.Key * g.Count())
			.ToString();

		const int neither = 0;
		const int torch = 1;
		const int gear = 2;

		IEnumerable<((int, int, int), int)> getNeighbors((int x, int y, int equip) pos, int cost)
		{
			var states = new List<((int, int, int), int)>();

			addNeighbor(pos.x + 1, pos.y, pos.equip, cost + 1);
			addNeighbor(pos.x, pos.y + 1, pos.equip, cost + 1);
			addNeighbor(pos.x - 1, pos.y, pos.equip, cost + 1);
			addNeighbor(pos.x, pos.y - 1, pos.equip, cost + 1);
			if (pos.equip != neither) addNeighbor(pos.x, pos.y, neither, cost + 7);
			if (pos.equip != torch) addNeighbor(pos.x, pos.y, torch, cost + 7);
			if (pos.equip != gear) addNeighbor(pos.x, pos.y, gear, cost + 7);

			return states;

			void addNeighbor(int x, int y, int equip, int newCost)
			{
				if (x < 0)
					return;
				if (y < 0)
					return;
				if (x >= destination.x + margin)
					return;
				if (y >= destination.y + margin)
					return;

				var type = ground[y][x] % 3;
				switch (type)
				{
					case 0:
						if (equip == torch || equip == gear)
							states.Add(((x, y, equip), newCost));
						return;

					case 1:
						if (equip == neither || equip == gear)
							states.Add(((x, y, equip), newCost));
						return;

					case 2:
						if (equip == neither || equip == torch)
							states.Add(((x, y, equip), newCost));
						return;
				}
			}
		}

		var cost = SuperEnumerable.GetShortestPathCost<(int, int, int), int>(
			(0, 0, torch),
			getNeighbors,
			(destination.x, destination.y, torch));

		var part2 = cost.ToString();

		return (part1, part2);
	}
}
