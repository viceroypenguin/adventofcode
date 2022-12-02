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

		DoPartA(lines);
		DoPartB(lines);
	}

	private void DoPartA(string[] lines)
	{
		// one-char position at end of string
		// easy parsing of current position
		// subtract one so 1..10 is 0..9 mod 10 for easy math
		var pos1 = lines[0][^1] - '0' - 1;
		var pos2 = lines[1][^1] - '0' - 1;

		// start with score of 0
		var (score1, score2) = (0, 0);
		// how many turns have we done
		var cnt = 0;
		while (true)
		{
			// cnt = 0-100
			// base is 0, 3, 6, 9...
			// rolls are 1, 2, 3 up from base
			// sum of rolls is base * 3 + 1 + 2 + 3
			// = base * 9 + 6
			// update position mod 10
			// math isn't exactly perfect, but %10 washes out a lot
			pos1 = (pos1 + (cnt % 100) * 9 + 6) % 10;
			// increment turn count
			cnt++;
			// update score based on position
			// add one more since 0..9 vs 1..10
			score1 += (pos1 + 1);
			// did we win?
			if (score1 >= 1000)
			{
				// 3-dice rolls per turn
				// report loser score value
				PartA = (score2 * cnt * 3).ToString();
				break;
			}

			// vice-versa
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

	private void DoPartB(string[] lines)
	{
		// calculate all potential outcomes
		// of 3 dirac dice
		var diceCount = (
				from x in Enumerable.Range(1, 3)
				from y in Enumerable.Range(1, 3)
				from z in Enumerable.Range(1, 3)
				// key by the total sum of the three dice rolls
				group 1 by x + y + z into a
				// how many universes of each sum 
				select (pos: a.Key, cnt: a.Count()))
			.ToList();

		// start with initial position of positions and 0-score, there's one universe of this
		var scores = new List<((int pos1, int pos2, int score1, int score2) state, long count)>()
		{
			((lines[0][^1] - '0' - 1, lines[1][^1] - '0' - 1, 0, 0), 1),
		};
		// while we have any games that are not yet done
		while (scores.Any(s => s.state.score1 < 21 && s.state.score2 < 21))
		{
			scores = scores
				// for-each existing universion
				.SelectMany(s =>
				{
					var ((pos1, pos2, score1, score2), count) = s;
					// if either player one, simplify to common state
					// to reduce unnecessary processing
					if (score1 >= 21)
						return Enumerable.Repeat((state: (0, 0, 0, 21), count), 1);
					if (score2 >= 21)
						return Enumerable.Repeat((state: (0, 0, 21, 0), count), 1);

					// for each dice roll from this universe
					return diceCount
						.Select(p =>
						{
							// update our position based on the dice rolls
							var p1 = (pos1 + p.pos) % 10;
							// update our score based on our new position
							var s1 = score1 + p1 + 1;
							return (
								// switch p1/p2
								state: (pos2, p1, score2, s1), 
								// this happened p.cnt times in each of the
								// existing # of universes
								count: count * p.cnt);
						});
				})
				// reduce the number of duplicate states
				.GroupBy(x => x.state, (s, g) => (s, g.Sum(x => x.count)))
				.ToList();
		}

		// all games are over
		var wins = scores
			// how many wins for each player?
			.GroupBy(s => s.state.score1 >= 21, (b, g) => (b, cnt: g.Sum(x => x.count)))
			// only care about the max num of wins
			.Max(x => x.cnt);
		PartB = wins.ToString();
	}
}
