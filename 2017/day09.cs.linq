<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day09.input.txt"));

var index = 0;
char getNextChar()
{
	if (index == input.Length) return default(char);
	return input[index++];
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
	score.Dump("Part A");
	garbage.Dump("Part B");
}