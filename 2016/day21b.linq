<Query Kind="Statements" />

var password = "fbgdceah";
var input = 
@"swap position 5 with position 6
reverse positions 1 through 6
rotate right 7 steps
rotate based on position of letter c
rotate right 7 steps
reverse positions 0 through 4
swap letter f with letter h
reverse positions 1 through 2
move position 1 to position 0
rotate based on position of letter f
move position 6 to position 3
reverse positions 3 through 6
rotate based on position of letter c
rotate based on position of letter b
move position 2 to position 4
swap letter b with letter d
move position 1 to position 6
move position 7 to position 1
swap letter f with letter c
move position 2 to position 3
swap position 1 with position 7
reverse positions 3 through 5
swap position 1 with position 4
move position 4 to position 7
rotate right 4 steps
reverse positions 3 through 6
move position 0 to position 6
swap position 3 with position 5
swap letter e with letter h
rotate based on position of letter c
swap position 4 with position 7
reverse positions 0 through 5
rotate right 5 steps
rotate left 0 steps
rotate based on position of letter f
swap letter e with letter b
rotate right 2 steps
rotate based on position of letter c
swap letter a with letter e
rotate left 4 steps
rotate left 0 steps
move position 6 to position 7
rotate right 2 steps
rotate left 6 steps
rotate based on position of letter d
swap letter a with letter b
move position 5 to position 4
reverse positions 0 through 7
rotate left 3 steps
rotate based on position of letter e
rotate based on position of letter h
swap position 4 with position 6
reverse positions 4 through 5
reverse positions 5 through 7
rotate left 3 steps
move position 7 to position 2
move position 3 to position 4
swap letter b with letter d
reverse positions 3 through 4
swap letter e with letter a
rotate left 4 steps
swap position 3 with position 4
swap position 7 with position 5
rotate right 1 step
rotate based on position of letter g
reverse positions 0 through 3
swap letter g with letter b
rotate based on position of letter b
swap letter a with letter c
swap position 0 with position 2
reverse positions 1 through 3
rotate left 7 steps
swap letter f with letter a
move position 5 to position 0
reverse positions 1 through 5
rotate based on position of letter d
rotate based on position of letter c
rotate left 2 steps
swap letter b with letter a
swap letter f with letter c
swap letter h with letter f
rotate based on position of letter b
rotate left 3 steps
swap letter b with letter h
reverse positions 1 through 7
rotate based on position of letter h
swap position 1 with position 5
rotate left 1 step
rotate based on position of letter h
reverse positions 0 through 1
swap position 5 with position 7
reverse positions 0 through 2
reverse positions 1 through 3
move position 1 to position 4
reverse positions 1 through 3
rotate left 1 step
swap position 4 with position 1
move position 1 to position 3
rotate right 2 steps
move position 0 to position 5";

var regex = new Regex(@"
swap\ (
    (?<swap_position>position\ (?<position_x>\d+)\ with\ position\ (?<position_y>\d+)) |
    (?<swap_letter>letter\ (?<letter_x>\w)\ with\ letter\ (?<letter_y>\w))  ) |
rotate\ (
    (?<rotate_steps>(?<rotate_direction>left|right)\ (?<rotate_count>\d+)\ steps?) |
    (?<rotate_position>based\ on\ position\ of\ letter\ (?<rotate_letter>\w))  ) |
(?<reverse>reverse\ positions\ (?<reverse_x>\d+)\ through\ (?<reverse_y>\d+)) |
(?<move>move\ position\ (?<move_x>\d+)\ to\ position\ (?<move_y>\d+))", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);

var instructions = 
	input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(s=>regex.Match(s))
        .Reverse()
        .ToList();

var working = password.ToList();
foreach (var i in instructions)
{
    if (i.Groups["swap_position"].Success)
    {
        var x = Convert.ToInt32(i.Groups["position_x"].Value);
        var y = Convert.ToInt32(i.Groups["position_y"].Value);
        
        var chr = working[x];
        working[x] = working[y];
        working[y] = chr;
    }
    else if (i.Groups["swap_letter"].Success)
    {
        var x = i.Groups["letter_x"].Value[0];
        var y = i.Groups["letter_y"].Value[0];
        
        var x_index = working.FindIndex(c=>c==x);
        var y_index = working.FindIndex(c=>c==y);
        
        working[x_index] = y;
        working[y_index] = x;
    }
    else if (i.Groups["rotate_steps"].Success)
    {
        var dir = i.Groups["rotate_direction"].Value;
        var steps = Convert.ToInt32(i.Groups["rotate_count"].Value) % working.Count;
        
        if (dir == "left")
        {
            steps = working.Count - steps;
            working = working.Skip(steps).Concat(working.Take(steps)).ToList();
        }
        else
        {
            working = working.Skip(steps).Concat(working.Take(steps)).ToList();
        }
    }
    else if (i.Groups["rotate_position"].Success)
    {
        var letter = i.Groups["rotate_letter"].Value[0];
        var endsUpAt = working.FindIndex(c=>c==letter);
        
        var originallyAt = 0;
        for (; originallyAt < working.Count; originallyAt++)
        {
            var newPosition = 
                originallyAt + // original position
                1 + // plus 1
                originallyAt + // plus shift by index
                (originallyAt>=4?1:0); // plus one more if four or more
                
            new { originallyAt, newPosition, endsUpAt }.Dump();
            if (newPosition%working.Count == endsUpAt)
                break;
        }
        
        if (endsUpAt > originallyAt)
        {
            var steps = endsUpAt - originallyAt;
            working = working.Skip(steps).Concat(working.Take(steps)).ToList();
        }
        else
        {
            var steps = working.Count - (originallyAt - endsUpAt);
            working = working.Skip(steps).Concat(working.Take(steps)).ToList();
        }
    }
    else if (i.Groups["reverse"].Success)
    {
        var x = Convert.ToInt32(i.Groups["reverse_x"].Value);
        var y = Convert.ToInt32(i.Groups["reverse_y"].Value);
        
        working.Reverse(x, y - x + 1);
    }
    else if (i.Groups["move"].Success)
    {
        var x = Convert.ToInt32(i.Groups["move_x"].Value);
        var y = Convert.ToInt32(i.Groups["move_y"].Value);
        
        var chr = working[y];
        working.RemoveAt(y);
        working.Insert(x, chr);
    }
    
    string.Join("", working).Dump();
}