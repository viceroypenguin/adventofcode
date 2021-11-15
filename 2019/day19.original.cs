using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_19_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 19;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		// 64k should be enough for anyone
		Array.Resize(ref instructions, 64 * 1024);

		var inputs = new BufferBlock<long>();
		var outputs = new BufferBlock<long>();

		var map = Enumerable.Range(0, 50)
			.Select(y => new long[50])
			.ToArray();

		int x = 0, y = 0;
		for (y = 0; y < 50; y++)
			for (x = 0; x < 50; x++)
			{
				inputs.Post(x);
				inputs.Post(y);

				new IntCodeComputer(instructions.ToArray(), inputs, outputs)
					.RunProgram()
					.GetAwaiter()
					.GetResult();

				map[y][x] = outputs.Receive();
			}

		PartA = map
			.SelectMany(r => r)
			.Count(x => x == 1)
			.ToString();

		y = Enumerable.Range(0, 50)
			.First(y => map[y][49] == 1);
		for (x = 101, y = 0; ; x++)
		{
			while (true)
			{
				inputs.Post(x);
				inputs.Post(y);

				new IntCodeComputer(instructions.ToArray(), inputs, outputs)
					.RunProgram()
					.GetAwaiter()
					.GetResult();

				if (outputs.Receive() == 1)
					break;
				y++;
			}

			inputs.Post(x - 99);
			inputs.Post(y + 99);

			new IntCodeComputer(instructions.ToArray(), inputs, outputs)
				.RunProgram()
				.GetAwaiter()
				.GetResult();

			if (outputs.Receive() == 1)
			{
				PartB = ((x - 99) * 10000 + y).ToString();
				return;
			}
		}
	}
}
