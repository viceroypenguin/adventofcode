<Query Kind="Statements" />

var input = 
@"cpy a b
dec b
cpy a d
cpy 0 a
cpy b c
inc a
dec c
jnz c -2
dec d
jnz d -5
dec b
cpy b c
cpy c d
dec d
inc c
jnz d -2
tgl c
cpy -16 c
jnz 1 c
cpy 81 c
jnz 92 d
inc a
inc d
jnz d -2
inc c
jnz c -5";

var regex = new Regex(@"(?<i>\w{3}) (?<x>-?\d+|a|b|c|d)(?: (?<y>-?\d+|a|b|c|d))?", RegexOptions.Compiled);

var instructions = 
	input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(s=>regex.Match(s))
        .Select(m=>new
        {
            i = m.Groups["i"].Value,
            x = m.Groups["x"].Value,
            y = m.Groups["y"].Value,
        })
		.ToArray();
        
var registers = new Dictionary<string, int>()
{
	{ "a", 12 },
	{ "b", 0 },
	{ "c", 0 },
	{ "d", 0 },
};

Func<string, int> argumentValue = (s)=>registers.ContainsKey(s) ? registers[s] : Convert.ToInt32(s);

var ip = 0;
while (ip < instructions.Length)
{
	var ins = instructions[ip];
	switch (ins.i)
	{
		case "cpy":
		{
			var value = argumentValue(ins.x);
			var dest = ins.y;
            if (registers.ContainsKey(dest))
                registers[dest] = value;
			break;
		}
			
		case "inc":
		{
			var dest = ins.x;
			registers[dest]++;
			break;
		}
			
		case "dec":
		{
			var dest = ins.x;
			registers[dest]--;
			break;
		}
			
		case "jnz":
		{
			var value = argumentValue(ins.x);
			if (value != 0)
			{
				var distance = argumentValue(ins.y);
				ip += distance;
				continue;
			}
			else
				break;
		}
        
        case "tgl":
        {
            var value = argumentValue(ins.x);
            var mip = value + ip;
            if (mip >= instructions.Length) break;
            
            var oldCmd = instructions[mip].i;
            var newCmd = 
                oldCmd == "jnz" ? "cpy" :
                oldCmd == "cpy" ? "jnz" :
                oldCmd == "inc" ? "dec" :
                oldCmd == "dec" ? "inc" :
                oldCmd == "tgl" ? "inc" :
                "";
            instructions[mip] = new
            {
                i = newCmd,
                x = instructions[mip].x,
                y = instructions[mip].y,
            };
            
            break;
        }
        
        default:
            throw new InvalidOperationException("How did we get here?");
	}
	ip++;
}

registers.Dump();