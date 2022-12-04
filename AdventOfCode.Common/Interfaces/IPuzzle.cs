namespace AdventOfCode.Common.Interfaces;

using Models;

public interface IPuzzle
{
	(string part1, string part2) Solve(PuzzleInput input);
}
