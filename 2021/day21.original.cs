namespace AdventOfCode;

public class Day_2021_21_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 21;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines();
		var pos1 = lines[0][^1] - '0' - 1;
		var pos2 = lines[1][^1] - '0' - 1;

		var (score1, score2) = (0, 0);
		var cnt = 0;
		while (true)
		{
			pos1 = (pos1 + (cnt % 100) * 9 + 6) % 10;
			cnt++;
			score1 += (pos1 + 1);
			if (score1 >= 1000)
			{
				PartA = (score2 * cnt * 3).ToString();
				break;
			}

			pos2 = (pos2 + (cnt % 100) * 9 + 6) % 10;
			cnt++;
			score2 += (pos2 + 1);
			if (score2 >= 1000)
			{
				PartA = (score1 * cnt * 3).ToString();
				break;
			}
		}
	}
}
