using System.Threading.Tasks.Dataflow;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 19, CodeType.Original)]
public class Day_19_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
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

		var part1 = map
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
				var part2 = ((x - 99) * 10000 + y).ToString();
				return (part1, part2);
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
