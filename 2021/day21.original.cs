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

		var diceCount = (
				from x in Enumerable.Range(1, 3)
				from y in Enumerable.Range(1, 3)
				from z in Enumerable.Range(1, 3)
				group 1 by x + y + z into a
				select (pos: a.Key, cnt: a.Count()))
			.ToList();

		var scores = new List<((int pos1, int pos2, int score1, int score2) state, long count)>()
		{
			((lines[0][^1] - '0' - 1, lines[1][^1] - '0' - 1, 0, 0), 1),
		};
		while (scores.Any(s => s.state.score1 < 21 && s.state.score2 < 21))
		{
			scores = scores
				.SelectMany(s =>
				{
					var ((pos1, pos2, score1, score2), count) = s;
					if (score1 >= 21)
						return Enumerable.Repeat((state: (0, 0, 0, 21), count), 1);
					if (score2 >= 21)
						return Enumerable.Repeat((state: (0, 0, 21, 0), count), 1);

					return diceCount
						.Select(p =>
						{
							var p1 = (pos1 + p.pos) % 10;
							var s1 = score1 + p1 + 1;
							return (state: (pos2, p1, score2, s1), count: count * p.cnt);
						});
				})
				.GroupBy(x => x.state, (s, g) => (s, g.Sum(x => x.count)))
				.ToList();
		}

		var wins = scores
			.GroupBy(s => s.state.score1 >= 21, (b, g) => (b, cnt: g.Sum(x => x.count)))
			.Max(x => x.cnt);
		PartB = wins.ToString();
	}
}
