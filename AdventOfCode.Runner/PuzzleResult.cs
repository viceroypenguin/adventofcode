namespace AdventOfCode.Runner;

public record PuzzleResult(
	PuzzleModel Puzzle,
	string Part1,
	string Part2,
	TimeSpan ElapsedParse,
	TimeSpan ElapsedPart1,
	TimeSpan ElapsedPart2);
