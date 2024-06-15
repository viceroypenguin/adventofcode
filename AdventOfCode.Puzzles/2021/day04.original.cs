namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 4, CodeType.Original)]
public class Day_04_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		// split by double-newline, and break int lines within each segment
		var segments = input.Lines.Split(string.Empty).ToList();

		// numbers are in first line of first segment
		var numbers = segments[0][0].Split(',').Select(x => Convert.ToInt32(x)).ToList();

		// segments 1 to end are each one bingo board
		var boards = segments.Skip(1)
			.Select(b =>
				b
					// for each line (x)
					.SelectMany((l, x) => l.Split()
						// skip empty string at front " 7", for example
						.Where(s => s.Length > 0)
						// for each number (y)
						.Select((s, y) => (pos: (x, y), num: Convert.ToInt32(s))))
					// more often use by number rather than by position
					// so dictionary that way instead
					.ToDictionary(
						x => x.num,
						x => x.pos))
			.ToList();

		// local function to access `numbers` variable
		(bool[,] matched, int number, int count) RunBingo(
			Dictionary<int, (int x, int y)> board)
		{
			var matched = new bool[5, 5];

			// keep track of both number, and index of that number in the list
			foreach (var (i, n) in numbers.Index())
			{
				// no need to work if we can't find the number in the board
				if (!board.TryGetValue(n, out var pos))
					continue;

				// set flag, then check if we had a bingo
				matched[pos.x, pos.y] = true;
				if (IsBingo(matched, pos.x, pos.y))
					// keep track of board, the number, and how long it took
					return (matched, n, i);
			}

			return default;
		}

		// run bingo game for all boards
		var bingos = boards
			.Select(b => (b, bingo: RunBingo(b)))
			.Where(b => b.bingo != default)
			.ToList();

		// get first completed board
		var part1 = GetBingoValue(bingos
			.MinBy(b => b.bingo.count)).ToString();

		// get last completed board
		var part2 = GetBingoValue(bingos
			.MaxBy(b => b.bingo.count)).ToString();

		return (part1, part2);
	}

	private static bool IsBingo(bool[,] board, int x, int y) =>
		// check if all of row/column is set;
		// only need to check the row and column where we
		// just set a flag, since others won't have changed status
		Enumerable.Range(0, 5).All(y => board[x, y])
		|| Enumerable.Range(0, 5).All(x => board[x, y]);

	private static int GetBingoValue((Dictionary<int, (int x, int y)> b, (bool[,] matched, int number, int count) bingo) mostSuccessful) =>
		mostSuccessful.bingo.number * GetUnmatchedSum(mostSuccessful.b, mostSuccessful.bingo.matched);

	private static int GetUnmatchedSum(Dictionary<int, (int x, int y)> b, bool[,] matched) =>
		b.Where(kvp => !matched[kvp.Value.x, kvp.Value.y])
			.Sum(kvp => kvp.Key);
}
