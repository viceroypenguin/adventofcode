namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 3, CodeType.Fastest)]
public class Day_03_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = Part1(input.Lines);
		var part2 = Part2(input.Lines);
		return (part1, part2);
	}

	private static int GetValue(char c) =>
		c > 'Z' ? c - 'a' + 1 : c - 'A' + 27;

	private static string Part1(string[] input)
	{
		var sum = 0;
		foreach (var l in input)
		{
			var left = l.AsSpan()[..(l.Length / 2)];
			var right = l.AsSpan()[(l.Length / 2)..];

			foreach (var c in left)
			{
				if (right.Contains(c))
				{
					sum += GetValue(c);
					break;
				}
			}
		}

		return sum.ToString();
	}

	private static string Part2(string[] input)
	{
		var sum = 0;
		for (int i = 0; i < input.Length; i += 3)
		{
			foreach (var c in input[i])
			{
				if (input[i + 1].Contains(c)
					&& input[i + 2].Contains(c))
				{
					sum += GetValue(c);
					break;
				}
			}
		}

		return sum.ToString();
	}
}
