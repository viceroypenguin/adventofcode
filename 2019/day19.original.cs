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

		var map = Enumerable.Range(0, 50)
			.Select(y => new long[50])
			.ToArray();

		int x = 0, y = 0;
		for (y = 0; y < 50; y++)
			for (x = 0; x < 50; x++)
				map[y][x] = RunProgram(instructions, x, y);

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
				if (RunProgram(instructions, x, y) == 1)
					break;
				y++;
			}

			if (RunProgram(instructions, x - 99, y + 99) == 1)
			{
				PartB = ((x - 99) * 10000 + y).ToString();
				return;
			}
		}
	}

	private static long RunProgram(long[] instructions, int x, int y)
	{
		var pc = new IntCodeComputer(instructions);
		pc.Inputs.Enqueue(x);
		pc.Inputs.Enqueue(y);
		pc.RunProgram();
		return pc.Outputs.Dequeue();
	}
}
