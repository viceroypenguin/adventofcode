namespace AdventOfCode.Common.Models;

public readonly record struct PuzzleModel(
	string? Name,
	int Year,
	int Day,
	CodeType CodeType,
	Type PuzzleType
);
