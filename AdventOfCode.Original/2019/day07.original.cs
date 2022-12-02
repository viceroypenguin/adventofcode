using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_07_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 7;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		PartA = DoPart(instructions, 0);
		PartB = DoPart(instructions, 5);
	}

	private static string DoPart(long[] instructions, int start) =>
		SuperEnumerable.Permutations(Enumerable.Range(start, 5))
			.Select(arr =>
			{
				var computers = Enumerable.Range(0, 5)
					.Select(i =>
					{
						var pc = new IntCodeComputer(instructions);
						pc.Inputs.Enqueue(arr[i]);
						return pc;
					})
					.ToList();

				var signal = 0L;
				while (computers[^1].ProgramStatus != ProgramStatus.Completed)
				{
					foreach (var c in computers)
					{
						c.Inputs.Enqueue(signal);
						c.RunProgram();
						signal = c.Outputs.Dequeue();
					}
				}

				return signal;
			})
			.Max()
			.ToString();
}
