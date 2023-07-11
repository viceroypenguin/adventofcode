namespace AdventOfCode;

public class Day_2017_09_Original : Day
{
	public override int Year => 2017;
	public override int DayNumber => 9;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var str = input.GetString();

		var index = 0;
		char getNextChar()
		{
			if (index == input.Length) return default(char);
			return str[index++];
		}

		int parseGarbage()
		{
			var cnt = 0;
			while (true)
			{
				switch (getNextChar())
				{
					case '!':
						// ignore next char
						getNextChar();
						break;

					case '>':
						return cnt;

					default:
						cnt++;
						break;
				}
			}
		}

		(int score, int garbage) getScore(int level = 1)
		{
			var x = (score: level, garbage: 0);

			while (true)
			{
				switch (getNextChar())
				{
					case '}':
						return x;

					case '<':
						x.garbage += parseGarbage();
						break;

					case '{':
						var y = getScore(level + 1);
						x.score += y.score;
						x.garbage += y.garbage;
						break;
				}
			}
		}

		getNextChar(); // drop first {
		{
			var (score, garbage) = getScore();
			Dump('A', score);
			Dump('B', garbage);
		}
	}
}
