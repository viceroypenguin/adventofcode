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

		_pc = new IntCodeComputer(instructions);

		for (var i = 1; i <= 4; i++)
			HandleDirection((0, 0), i, 1);

		var part1 = _map[_oxygenLocation].distance.ToString();

		var queue = new Queue<((int x, int y) position, int distance)>();
		queue.Enqueue((_oxygenLocation, 0));

		_map[_oxygenLocation] = (1, 0); // simplify type check below
		while (queue.Count != 0)
		{
			var (position, distance) = queue.Dequeue();
			if (_map[position].type != 1)
				continue;

			_map[position] = (3, distance);

			queue.Enqueue(((position.x, position.y - 1), distance + 1));
			queue.Enqueue(((position.x, position.y + 1), distance + 1));
			queue.Enqueue(((position.x - 1, position.y), distance + 1));
			queue.Enqueue(((position.x + 1, position.y), distance + 1));
		}

		var part2 = _map.Values.Where(x => x.type == 3).Max(x => x.distance).ToString();
		return (part1, part2);
	}

	private IntCodeComputer _pc;
	private readonly Dictionary<(int x, int y), (int type, int distance)> _map =
		new() { [(0, 0)] = (0, 0), };
	private (int x, int y) _oxygenLocation;

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

		if (_map.ContainsKey(newPosition))
			return;

		_pc.Inputs.Enqueue(direction);
		_ = _pc.RunProgram();
		var response = _pc.Outputs.Dequeue();
		_map[newPosition] = ((int)response, distance);
		if (response == 0)
			return;

		if (response == 2)
			_oxygenLocation = newPosition;

		for (var i = 1; i <= 4; i++)
			HandleDirection(newPosition, i, distance + 1);

		_pc.Inputs.Enqueue(direction switch { 1 => 2, 2 => 1, 3 => 4, 4 => 3, _ => throw new UnreachableException(), });
		_ = _pc.RunProgram();
		_ = _pc.Outputs.Dequeue();
	}
}
