using System.Collections;

namespace AdventOfCode;

public class Day_2021_04_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 4;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines();

		var numbers = lines[0].Split(',').Select(x => Convert.ToInt32(x)).ToList();

		var boards = lines[1..]
			.Batch(5)
			.Select(b =>
			{
				return b
					.SelectMany((l, x) => l.Split()
						.Where(s => s.Length > 0)
						.Select((s, y) => (pos: (x, y), num: Convert.ToInt32(s))))
					.ToDictionary(
						x => x.num,
						x => x.pos);
			})
			.ToList();

		(bool[,] matched, int number, int count) RunBingo(Dictionary<int, (int x, int y)> board)
		{
			var matched = new bool[5, 5];

			foreach (var (i, n) in numbers.Index())
			{
				if (!board.TryGetValue(n, out var pos))
					continue;

				matched[pos.x, pos.y] = true;
				if (IsBingo(matched))
					return (matched, n, i);
			}
			return default;
		}

		var bingos = boards
			.Select(b => (b, bingo: RunBingo(b)))
			.Where(b => b.bingo != default)
			.ToList();

		var mostSuccessful = bingos
			.OrderBy(b => b.bingo.count)
			.First();

		PartA = GetBingoValue(mostSuccessful).ToString();

		var leastSuccessful = bingos
			.OrderByDescending(b => b.bingo.count)
			.First();

		PartB = GetBingoValue(leastSuccessful).ToString();
	}

	private static int GetBingoValue((Dictionary<int, (int x, int y)> b, (bool[,] matched, int number, int count) bingo) mostSuccessful) => 
		mostSuccessful.bingo.number * GetUnmatchedSum(mostSuccessful.b, mostSuccessful.bingo.matched);

	private static int GetUnmatchedSum(Dictionary<int, (int x, int y)> b, bool[,] matched) =>
		b.Where(kvp => !matched[kvp.Value.x, kvp.Value.y])
			.Sum(kvp => kvp.Key);

	static bool IsBingo(bool[,] board)
	{
		for (int x = 0; x < 5; x++)
		{
			if (Enumerable.Range(0, 5).All(y => board[x, y]))
				return true;
		}
		for (int y = 0; y < 5; y++)
		{
			if (Enumerable.Range(0, 5).All(x => board[x, y]))
				return true;
		}

		//if (Enumerable.Range(0, 5).All(i => board[i, i]))
		//	return true;
		//if (Enumerable.Range(0, 5).All(i => board[i, 4 - i]))
		//	return true;
		return false;
	}
}
