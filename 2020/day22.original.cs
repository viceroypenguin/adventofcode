namespace AdventOfCode;

public class Day_2020_22_Original : Day
{
	public override int Year => 2020;
	public override int DayNumber => 22;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var decks = input.GetLines()
			.Segment(s => s.StartsWith("Player"))
			.Select(p => p.Skip(1).Select(int.Parse).ToList())
			.ToList();


		PartA = playGame(decks[0], decks[1], static (c1, c2, _, _) => c1 > c2)
			.winningDeck
			.Reverse()
			.Select((x, i) => x * (i + 1))
			.Sum()
			.ToString();

		PartB = playGame(decks[0], decks[1], recursiveGame)
			.winningDeck
			.Reverse()
			.Select((x, i) => x * (i + 1))
			.Sum()
			.ToString();
	}

	private static (bool captainWon, Queue<int> winningDeck) playGame(
		IEnumerable<int> _captain, IEnumerable<int> _crab, 
		Func<int, int, Queue<int>, Queue<int>, bool> getRoundWinner)
	{
		var captain = new Queue<int>(_captain);
		var crab = new Queue<int>(_crab);

		var seenStates = new HashSet<int>();

		while (captain.Count > 0 && crab.Count > 0)
		{
			var state = HashCode.Combine(
				captain.Aggregate((a, b) => HashCode.Combine(a, b)),
				crab.Aggregate((a, b) => HashCode.Combine(a, b)));
			if (seenStates.Contains(state))
				return (true, captain);
			seenStates.Add(state);

			var c1 = captain.Dequeue();
			var c2 = crab.Dequeue();
			if (getRoundWinner(c1, c2, captain, crab))
			{
				captain.Enqueue(c1);
				captain.Enqueue(c2);
			}
			else
			{
				crab.Enqueue(c2);
				crab.Enqueue(c1);
			}
		}

		var winner = captain.Count > 0;
		return (winner, winner ? captain : crab);
	}

	private static bool recursiveGame(int c1, int c2, Queue<int> d1, Queue<int> d2) =>
		d1.Count >= c1 && d2.Count >= c2 
			? playGame(d1.Take(c1), d2.Take(c2), recursiveGame).captainWon 
			: c1 > c2;
}
