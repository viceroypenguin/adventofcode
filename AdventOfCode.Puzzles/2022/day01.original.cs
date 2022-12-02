namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle<List<int>>
{
	public List<int> Parse(PuzzleInput input) =>
		input.Lines
			.Split(string.Empty)
			.Select(g => g.Select(int.Parse).Sum())
			.ToList();

	public string Part1(List<int> input) =>
		input.Max().ToString();
	public string Part2(List<int> input) =>
		input.PartialSort(3, OrderByDirection.Descending)
			.Sum()
			.ToString();
}
