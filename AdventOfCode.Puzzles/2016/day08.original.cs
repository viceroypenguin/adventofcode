namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 08, CodeType.Original)]
public partial class Day_08_Original : IPuzzle
{
	[GeneratedRegex("(?:(?<rect>rect (?<rect_rows>\\d+)x(?<rect_cols>\\d+))|(?<rotate_column>rotate column x=(?<col_num>\\d+) by (?<col_amt>\\d+))|(?<rotate_row>rotate row y=(?<row_num>\\d+) by (?<row_amt>\\d+)))", RegexOptions.Compiled)]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();

		var instructions =
			input.Lines
				.Select(str => regex.Match(str))
				.ToList();

		var SCREEN_ROWS = 6;
		var SCREEN_COLS = 50;

		var screen = new bool[SCREEN_COLS, SCREEN_ROWS];

		foreach (var i in instructions)
		{
			if (i.Groups["rect"].Success)
			{
				var rows = Convert.ToInt32(i.Groups["rect_rows"].Value);
				var cols = Convert.ToInt32(i.Groups["rect_cols"].Value);

				for (var x = 0; x < rows; x++)
				{
					for (var y = 0; y < cols; y++)
						screen[x, y] = true;
				}
			}
			else if (i.Groups["rotate_column"].Success)
			{
				var x = Convert.ToInt32(i.Groups["col_num"].Value);
				var shift = Convert.ToInt32(i.Groups["col_amt"].Value);

				var col = new bool[SCREEN_ROWS];
				for (var y = 0; y < SCREEN_ROWS; y++)
					col[(y + shift) % SCREEN_ROWS] = screen[x, y];
				for (var y = 0; y < SCREEN_ROWS; y++)
					screen[x, y] = col[y];
			}
			else if (i.Groups["rotate_row"].Success)
			{
				var y = Convert.ToInt32(i.Groups["row_num"].Value);
				var shift = Convert.ToInt32(i.Groups["row_amt"].Value);

				var row = new bool[SCREEN_COLS];
				for (var x = 0; x < SCREEN_COLS; x++)
					row[(x + shift) % SCREEN_COLS] = screen[x, y];
				for (var x = 0; x < SCREEN_COLS; x++)
					screen[x, y] = row[x];
			}
			else
			{
				throw new InvalidOperationException("Unknown command.");
			}
		}

		var partA = screen.OfType<bool>().Count(SuperEnumerable.Identity).ToString();

		var partB = string.Join(
			Environment.NewLine,
			Enumerable.Range(0, SCREEN_ROWS).Select(y =>
				string.Join("", Enumerable.Range(0, SCREEN_COLS).Select(x =>
					screen[x, y] ? '#' : ' '))));

		return (partA, partB);
	}
}
