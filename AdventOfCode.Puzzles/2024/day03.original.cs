namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 03, CodeType.Original)]
public partial class Day_03_Original : IPuzzle
{
	[GeneratedRegex(@"mul\((\d+),(\d+)\)")]
	private static partial Regex MulRegex();

	[GeneratedRegex(@"(?<do>do\(\))|(?<mul>mul\((?<mul1>\d+),(?<mul2>\d+)\))|(?<dont>don't\(\))")]
	private static partial Regex InstructionsRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = MulRegex().Matches(input.Text)
			.Select(m => long.Parse(m.Groups[1].Value) * long.Parse(m.Groups[2].Value))
			.Sum()
			.ToString();

		var sum = 0L;
		var state = true;
		foreach (Match m in InstructionsRegex().Matches(input.Text))
		{
			switch (state, m.Groups["do"].Success, m.Groups["mul"].Success, m.Groups["dont"].Success)
			{
				case (false, true, _, _):
					state = true;
					break;

				case (true, _, _, true):
					state = false;
					break;

				case (true, _, true, _):
				{
					sum += long.Parse(m.Groups["mul1"].Value) * long.Parse(m.Groups["mul2"].Value);
					break;
				}

				default:
					break;
			}
		}

		var part2 = sum.ToString();

		return (part1, part2);
	}
}
