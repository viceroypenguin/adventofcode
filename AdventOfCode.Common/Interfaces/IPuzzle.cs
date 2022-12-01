namespace AdventOfCode.Common.Interfaces;

using Models;

public interface IPuzzle<TParsed>
{
	TParsed Parse(PuzzleInput input);

	string Part1(TParsed input);
	string Part2(TParsed input);
}
