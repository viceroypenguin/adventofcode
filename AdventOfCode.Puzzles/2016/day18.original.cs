namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 18, CodeType.Original)]
public class Day_18_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		return (
			ExecutePart(input.Lines[0], 40).ToString(),
			ExecutePart(input.Lines[0], 400000).ToString());
	}

	private static int ExecutePart(string input, int rows)
	{
		var tiles = new List<IList<bool>>
		{
			input.Select(c => c == '^').ToArray(),
		};

		while (tiles.Count < rows)
		{
			var row = tiles[^1];
			var newRow = new bool[row.Count];

			for (var i = 0; i < row.Count; i++)
			{
				var left = i > 0 && row[i - 1];
				var right = i < row.Count - 1 && row[i + 1];
				newRow[i] = left ^ right;
			}

			tiles.Add(newRow);
		}

		return tiles.SelectMany(x => x).Count(b => !b);
	}
}
