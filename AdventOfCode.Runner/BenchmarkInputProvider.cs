namespace AdventOfCode.Runner;

public static class BenchmarkInputProvider
{
	public static PuzzleInput GetRawInput(int year, int day)
	{
		var inputFile = $"../../../../../../../Inputs/{year}/day{day:00}.input.txt";

		return new(
			File.ReadAllBytes(inputFile),
			File.ReadAllText(inputFile),
			File.ReadAllLines(inputFile));
	}
}
