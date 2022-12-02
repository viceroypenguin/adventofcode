namespace AdventOfCode.Runner;

public record PuzzleResult(
	PuzzleModel Puzzle,
	string Part1,
	string Part2,
	TimeSpan ElapsedMsParse,
	TimeSpan ElapsedMsPart1,
	TimeSpan ElapsedMsPart2);
