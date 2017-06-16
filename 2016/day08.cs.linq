<Query Kind="Statements" />

var input = 
@"rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 3
rect 2x1
rotate row y=0 by 2
rect 1x2
rotate row y=1 by 5
rotate row y=0 by 3
rect 1x2
rotate column x=30 by 1
rotate column x=25 by 1
rotate column x=10 by 1
rotate row y=1 by 5
rotate row y=0 by 2
rect 1x2
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=2 by 18
rotate row y=0 by 5
rotate column x=0 by 1
rect 3x1
rotate row y=2 by 12
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate column x=20 by 1
rotate row y=2 by 5
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=2 by 15
rotate row y=0 by 15
rotate column x=10 by 1
rotate column x=5 by 1
rotate column x=0 by 1
rect 14x1
rotate column x=37 by 1
rotate column x=23 by 1
rotate column x=7 by 2
rotate row y=3 by 20
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=3 by 5
rotate row y=2 by 2
rotate row y=1 by 4
rotate row y=0 by 4
rect 1x4
rotate column x=35 by 3
rotate column x=18 by 3
rotate column x=13 by 3
rotate row y=3 by 5
rotate row y=2 by 3
rotate row y=1 by 1
rotate row y=0 by 1
rect 1x5
rotate row y=4 by 20
rotate row y=3 by 10
rotate row y=2 by 13
rotate row y=0 by 10
rotate column x=5 by 1
rotate column x=3 by 3
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate row y=4 by 10
rotate row y=3 by 10
rotate row y=1 by 10
rotate row y=0 by 10
rotate column x=7 by 2
rotate column x=5 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate row y=4 by 20
rotate row y=3 by 12
rotate row y=1 by 15
rotate row y=0 by 10
rotate column x=8 by 2
rotate column x=7 by 1
rotate column x=6 by 2
rotate column x=5 by 1
rotate column x=3 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate column x=46 by 2
rotate column x=43 by 2
rotate column x=24 by 2
rotate column x=14 by 3
rotate row y=5 by 15
rotate row y=4 by 10
rotate row y=3 by 3
rotate row y=2 by 37
rotate row y=1 by 10
rotate row y=0 by 5
rotate column x=0 by 3
rect 3x3
rotate row y=5 by 15
rotate row y=3 by 10
rotate row y=2 by 10
rotate row y=0 by 10
rotate column x=7 by 3
rotate column x=6 by 3
rotate column x=5 by 1
rotate column x=3 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate column x=19 by 1
rotate column x=10 by 3
rotate column x=5 by 4
rotate row y=5 by 5
rotate row y=4 by 5
rotate row y=3 by 40
rotate row y=2 by 35
rotate row y=1 by 15
rotate row y=0 by 30
rotate column x=48 by 4
rotate column x=47 by 3
rotate column x=46 by 3
rotate column x=45 by 1
rotate column x=43 by 1
rotate column x=42 by 5
rotate column x=41 by 5
rotate column x=40 by 1
rotate column x=33 by 2
rotate column x=32 by 3
rotate column x=31 by 2
rotate column x=28 by 1
rotate column x=27 by 5
rotate column x=26 by 5
rotate column x=25 by 1
rotate column x=23 by 5
rotate column x=22 by 5
rotate column x=21 by 5
rotate column x=18 by 5
rotate column x=17 by 5
rotate column x=16 by 5
rotate column x=13 by 5
rotate column x=12 by 5
rotate column x=11 by 5
rotate column x=3 by 1
rotate column x=2 by 5
rotate column x=1 by 5
rotate column x=0 by 1";

var regex = new Regex(@"(?:(?<rect>rect (?<rect_rows>\d+)x(?<rect_cols>\d+))|(?<rotate_column>rotate column x=(?<col_num>\d+) by (?<col_amt>\d+))|(?<rotate_row>rotate row y=(?<row_num>\d+) by (?<row_amt>\d+)))", RegexOptions.Compiled);

var instructions = 
    input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(str=>regex.Match(str))
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

screen.OfType<bool>().Where(b=>b).Count().Dump("Part A");

for (int y = 0; y < SCREEN_ROWS; y++)
    string.Join("", Enumerable.Range(0, SCREEN_COLS)
        .Select(x=>screen[x, y] ? "1" : "_")).Dump();
        