namespace AdventOfCode;

public class Day_2016_08_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 8;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var regex = new Regex(@"(?:(?<rect>rect (?<rect_rows>\d+)x(?<rect_cols>\d+))|(?<rotate_column>rotate column x=(?<col_num>\d+) by (?<col_amt>\d+))|(?<rotate_row>rotate row y=(?<row_num>\d+) by (?<row_amt>\d+)))", RegexOptions.Compiled);

		var instructions =
			input.GetLines()
				.Select(str => regex.Match(str))
				.ToList();

		//instructions.Dump();

		var SCREEN_ROWS = 6;
		var SCREEN_COLS = 50;

		var screen = new bool[SCREEN_COLS, SCREEN_ROWS];

		foreach (var i in instructions)
		{
			if (i.Groups["rect"].Success)
			{
				var rows = Convert.ToInt32(i.Groups["rect_rows"].Value);
				var cols = Convert.ToInt32(i.Groups["rect_cols"].Value);

				for (int x = 0; x < rows; x++)
					for (int y = 0; y < cols; y++)
						screen[x, y] = true;
			}
			else if (i.Groups["rotate_column"].Success)
			{
				var x = Convert.ToInt32(i.Groups["col_num"].Value);
				var shift = Convert.ToInt32(i.Groups["col_amt"].Value);

				var col = new bool[SCREEN_ROWS];
				for (int y = 0; y < SCREEN_ROWS; y++)
					col[(y + shift) % SCREEN_ROWS] = screen[x, y];
				for (int y = 0; y < SCREEN_ROWS; y++)
					screen[x, y] = col[y];
			}
			else if (i.Groups["rotate_row"].Success)
			{
				var y = Convert.ToInt32(i.Groups["row_num"].Value);
				var shift = Convert.ToInt32(i.Groups["row_amt"].Value);

				var row = new bool[SCREEN_COLS];
				for (int x = 0; x < SCREEN_COLS; x++)
					row[(x + shift) % SCREEN_COLS] = screen[x, y];
				for (int x = 0; x < SCREEN_COLS; x++)
					screen[x, y] = row[x];
			}
			else
				throw new InvalidOperationException("Unknown command.");
		}

		Dump('A',
			screen.OfType<bool>().Where(b => b).Count());

		DumpScreen('B',
			Enumerable.Range(0, SCREEN_ROWS).Select(y =>
				Enumerable.Range(0, SCREEN_COLS).Select(x =>
					screen[x, y] ? '#' : '_')));
	}
}
