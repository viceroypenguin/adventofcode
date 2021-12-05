using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_23_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 23;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		DoPartA(instructions);
		DoPartB(instructions);
	}

	private void DoPartA(long[] instructions)
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
						PartA = y.ToString();
						return;
					}

					var i = computers[(int)dest].Inputs;
					i.Enqueue(x);
					i.Enqueue(y);
				}
			}
		}
	}

	private void DoPartB(long[] instructions)
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
					PartB = y.ToString();
					return;
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
