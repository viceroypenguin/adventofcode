using System.Diagnostics;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 15, CodeType.Original)]
public class Day_15_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		pc = new IntCodeComputer(instructions);

		for (var i = 1; i <= 4; i++)
			HandleDirection((0, 0), i, 1);

		var part1 = map[oxygenLocation].distance.ToString();

		var queue = new Queue<((int x, int y) position, int distance)>();
		queue.Enqueue((oxygenLocation, 0));

		map[oxygenLocation] = (1, 0); // simplify type check below
		while (queue.Count != 0)
		{
			var (position, distance) = queue.Dequeue();
			if (map[position].type != 1)
				continue;

			map[position] = (3, distance);

			queue.Enqueue(((position.x, position.y - 1), distance + 1));
			queue.Enqueue(((position.x, position.y + 1), distance + 1));
			queue.Enqueue(((position.x - 1, position.y), distance + 1));
			queue.Enqueue(((position.x + 1, position.y), distance + 1));
		}

		var part2 = map.Values.Where(x => x.type == 3).Max(x => x.distance).ToString();
		return (part1, part2);
	}

	private IntCodeComputer pc;
	private readonly Dictionary<(int x, int y), (int type, int distance)> map =
		new() { [(0, 0)] = (0, 0), };
	private (int x, int y) oxygenLocation;

	private void HandleDirection((int x, int y) position, int direction, int distance)
	{
		var newPosition = direction switch
		{
			1 => (position.x, position.y - 1),
			2 => (position.x, position.y + 1),
			3 => (position.x - 1, position.y),
			4 => (position.x + 1, position.y),
			_ => throw new UnreachableException(),
		};

		if (map.ContainsKey(newPosition))
			return;

		pc.Inputs.Enqueue(direction);
		_ = pc.RunProgram();
		var response = pc.Outputs.Dequeue();
		map[newPosition] = ((int)response, distance);
		if (response == 0)
			return;

		if (response == 2)
			oxygenLocation = newPosition;

		for (var i = 1; i <= 4; i++)
			HandleDirection(newPosition, i, distance + 1);

		pc.Inputs.Enqueue(direction switch { 1 => 2, 2 => 1, 3 => 4, 4 => 3, _ => throw new UnreachableException(), });
		_ = pc.RunProgram();
		_ = pc.Outputs.Dequeue();
	}
}
