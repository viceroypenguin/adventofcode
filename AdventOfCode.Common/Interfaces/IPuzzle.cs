namespace AdventOfCode.Common.Interfaces;

public interface IPuzzle
{
	(string part1, string part2) Solve(PuzzleInput input);
}
