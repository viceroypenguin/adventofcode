namespace AdventOfCode.Common.Interfaces;

using Models;

public interface IPuzzleInputProvider
{
	PuzzleInput GetRawInput(int year, int day);
}
