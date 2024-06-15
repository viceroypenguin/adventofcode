namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 2, CodeType.Original)]
public class Day_02_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = input.Lines
			.Select(l => _part1Map[l])
			.Sum()
			.ToString();

		var part2 = input.Lines
			.Select(l => _part2Map[l])
			.Sum()
			.ToString();

		return (part1, part2);
	}

	private readonly Dictionary<string, int> _part1Map = new()
	{
		["A X"] = 1 + 3,
		["A Y"] = 2 + 6,
		["A Z"] = 3 + 0,
		["B X"] = 1 + 0,
		["B Y"] = 2 + 3,
		["B Z"] = 3 + 6,
		["C X"] = 1 + 6,
		["C Y"] = 2 + 0,
		["C Z"] = 3 + 3,
	};

	private readonly Dictionary<string, int> _part2Map = new()
	{
		["A X"] = 3 + 0,
		["A Y"] = 1 + 3,
		["A Z"] = 2 + 6,
		["B X"] = 1 + 0,
		["B Y"] = 2 + 3,
		["B Z"] = 3 + 6,
		["C X"] = 2 + 0,
		["C Y"] = 3 + 3,
		["C Z"] = 1 + 6,
	};
}
