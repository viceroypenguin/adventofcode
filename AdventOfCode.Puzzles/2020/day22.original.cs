namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 22, CodeType.Original)]
public class Day_22_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var decks = input.Lines
			.Split(string.Empty)
			.Select(p => p.Skip(1).Select(int.Parse).ToList())
			.ToList();

		var part1 = PlayGame(decks[0], decks[1], static (c1, c2, _, _) => c1 > c2)
			.winningDeck
			.Reverse()
			.Select((x, i) => x * (i + 1))
			.Sum()
			.ToString();

		var part2 = PlayGame(decks[0], decks[1], RecursiveGame)
			.winningDeck
			.Reverse()
			.Select((x, i) => x * (i + 1))
			.Sum()
			.ToString();

		return (part1, part2);
	}

	private static (bool captainWon, Queue<int> winningDeck) PlayGame(
		IEnumerable<int> captainInput, IEnumerable<int> crabInput,
		Func<int, int, Queue<int>, Queue<int>, bool> getRoundWinner
	)
	{
		var captain = new Queue<int>(captainInput);
		var crab = new Queue<int>(crabInput);

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

	private static bool RecursiveGame(int c1, int c2, Queue<int> d1, Queue<int> d2) =>
		d1.Count >= c1 && d2.Count >= c2
			? PlayGame(d1.Take(c1), d2.Take(c2), RecursiveGame).captainWon
			: c1 > c2;
}
