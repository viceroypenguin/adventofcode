namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 09, CodeType.Original)]
public class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var str = input.Text;

		var index = 0;
		char GetNextChar() =>
			index == str.Length
				? default
				: str[index++];

		int ParseGarbage()
		{
			var cnt = 0;
			while (true)
			{
				switch (GetNextChar())
				{
					case '!':
						// ignore next char
						_ = GetNextChar();
						break;

					case '>':
						return cnt;

					default:
						cnt++;
						break;
				}
			}
		}

		(int score, int garbage) GetScore(int level = 1)
		{
			var x = (score: level, garbage: 0);

			while (true)
			{
				switch (GetNextChar())
				{
					case '}':
						return x;

					case '<':
						x.garbage += ParseGarbage();
						break;

					case '{':
						var y = GetScore(level + 1);
						x.score += y.score;
						x.garbage += y.garbage;
						break;

					default:
						break;
				}
			}
		}

		// drop first {
		_ = GetNextChar();

		var (score, garbage) = GetScore();
		return (
			score.ToString(),
			garbage.ToString());
	}
}
