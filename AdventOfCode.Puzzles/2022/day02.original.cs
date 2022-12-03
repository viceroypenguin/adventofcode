namespace AdventOfCode;

[Puzzle(2022, 2, CodeType.Original)]
public class Day_02_Original : IPuzzle<string[]>
{
	public string[] Parse(PuzzleInput input) => input.Lines;

	private readonly Dictionary<string, int> part1Map = new()
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
	public string Part1(string[] input) =>
		input.Select(l => part1Map[l])
			.Sum()
			.ToString();

	private readonly Dictionary<string, int> part2Map = new()
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
	public string Part2(string[] input) =>
		input.Select(l => part2Map[l])
			.Sum()
			.ToString();
}
