namespace AdventOfCode.Common.Models;

using Attributes;

public readonly record struct PuzzleModel(
	string? Name,
	int Year,
	int Day,
	CodeType CodeType,
	Type PuzzleType,
	Type ParsedType);
