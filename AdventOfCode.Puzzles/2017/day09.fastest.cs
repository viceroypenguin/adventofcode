namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 09, CodeType.Fastest)]
public class Day_09_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day09.c
		var span = input.Span;

		int score = 0, garbage = 0, depth = 0, g = 0;
		for (var i = 0; i < span.Length; i++)
		{
			var c = span[i];
			switch (g | c)
			{
				case '}': score += depth; goto case '{';
				case '{': depth += '|' - c; break;
				case '<':
				case '>' | 0x80: g ^= 0x80; break;
				case '!' | 0x80: i++; break;
				default: garbage += g != 0 ? 1 : 0; break;
			}
		}

		return (
			score.ToString(),
			garbage.ToString());
	}
}
