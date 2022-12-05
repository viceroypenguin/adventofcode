namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 24, CodeType.Original)]
public class Day_24_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var groups = input.Lines
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.Batch(18)
			.Select(g =>
			{
				var a = Convert.ToInt32(g[4].Split()[^1]);
				var b = Convert.ToInt32(g[5].Split()[^1]);
				var c = Convert.ToInt32(g[15].Split()[^1]);
				return (a: a == 26, b, c);
			})
			.Index();

		var stack = new Stack<(int i, int c)>();
		var highDigits = new int[14];
		var lowDigits = new int[14];
		foreach (var (i, (a, b, c)) in groups)
		{
			if (a)
			{
				var (j, d) = stack.Pop();
				var diff = b + d;
				if (diff > 0)
				{
					highDigits[i] = 9;
					highDigits[j] = 9 - diff;

					lowDigits[j] = 1;
					lowDigits[i] = 1 + diff;
				}
				else
				{
					highDigits[j] = 9;
					highDigits[i] = 9 + diff;

					lowDigits[i] = 1;
					lowDigits[j] = 1 - diff;
				}
			}
			else
				stack.Push((i, c));
		}

		var part1 = string.Join("", highDigits);
		var part2 = string.Join("", lowDigits);
		return (part1, part2);
	}
}
