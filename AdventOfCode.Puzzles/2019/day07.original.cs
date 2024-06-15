namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 07, CodeType.Original)]
public class Day_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		return (
			DoPart(instructions, 0),
			DoPart(instructions, 5));
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
