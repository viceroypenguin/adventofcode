namespace AdventOfCode;

[Puzzle(2022, 3, CodeType.Fastest)]
public class Day_03_Fastest : IPuzzle<string[]>
{
	public string[] Parse(PuzzleInput input) => input.Lines;

	private static int GetValue(char c) =>
		c > 'Z' ? c - 'a' + 1 : c - 'A' + 27;

	public string Part1(string[] input)
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

	public string Part2(string[] input)
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
