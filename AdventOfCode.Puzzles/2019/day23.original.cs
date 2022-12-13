namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 23, CodeType.Original)]
public class Day_23_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		return (
			DoPartA(instructions),
			DoPartB(instructions));
	}

	private string DoPartA(long[] instructions)
	{
		var computers = Enumerable.Range(0, 50)
			.Select(i =>
			{
				var pc = new IntCodeComputer(instructions);
				pc.Inputs.Enqueue(i);
				return pc;
			})
			.ToList();

		while (true)
		{
			foreach (var pc in computers)
			{
				if (pc.Inputs.Count == 0)
					pc.Inputs.Enqueue(-1);
				pc.RunProgram();
				while (pc.Outputs.Count != 0)
				{
					var dest = pc.Outputs.Dequeue();
					var x = pc.Outputs.Dequeue();
					var y = pc.Outputs.Dequeue();

					if (dest == 255)
					{
						return y.ToString();
					}

					var i = computers[(int)dest].Inputs;
					i.Enqueue(x);
					i.Enqueue(y);
				}
			}
		}
	}

	private string DoPartB(long[] instructions)
	{
		var computers = Enumerable.Range(0, 50)
			.Select(i =>
			{
				var pc = new IntCodeComputer(instructions);
				pc.Inputs.Enqueue(i);
				return pc;
			})
			.ToList();

		(long x, long y)? natPacket = default;
		(long x, long y)? prevNatPacket = default;

		while (true)
		{
			foreach (var pc in computers)
			{
				if (pc.Inputs.Count == 0)
					pc.Inputs.Enqueue(-1);
				pc.RunProgram();
				while (pc.Outputs.Count != 0)
				{
					var dest = pc.Outputs.Dequeue();
					var x = pc.Outputs.Dequeue();
					var y = pc.Outputs.Dequeue();

					if (dest == 255)
					{
						natPacket = (x, y);
					}
					else
					{
						var i = computers[(int)dest].Inputs;
						i.Enqueue(x);
						i.Enqueue(y);
					}
				}
			}

			if (natPacket != default
				&& computers.All(c => c.Inputs.Count == 0))
			{
				var (x, y) = natPacket.Value;
				if (prevNatPacket != default
					&& y == prevNatPacket.Value.y)
				{
					return y.ToString();
				}

				var i = computers[0].Inputs;
				i.Enqueue(x);
				i.Enqueue(y);

				prevNatPacket = natPacket;
				natPacket = default;
			}
		}
	}
}
