<Query Kind="Statements" />

var input = 
@"cpy 1 a
cpy 1 b
cpy 26 d
jnz c 2
jnz 1 5
cpy 7 c
inc d
dec c
jnz c -2
cpy a c
inc a
dec b
jnz b -2
cpy c b
dec d
jnz d -6
cpy 13 c
cpy 14 d
inc a
dec d
jnz d -2
dec c
jnz c -5";

var regex = new Regex(@"(?<instruction>\w{3}) (?<x>-?\d+|a|b|c|d)(?: (?<y>-?\d+|a|b|c|d))?", RegexOptions.Compiled);

var instructions = 
	input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(s=>regex.Match(s))
		.ToArray();

var registers = new Dictionary<string, int>()
{
	{ "a", 0 },
	{ "b", 0 },
	{ "c", 1 },
	{ "d", 0 },
};

var ip = 0;
while (ip < instructions.Length)
{
	var instruction = instructions[ip];
	switch (instruction.Groups["instruction"].Value)
	{
		case "cpy":
		{
			var dest = instruction.Groups["y"].Value;
			var value = registers.ContainsKey(instruction.Groups["x"].Value) ? registers[instruction.Groups["x"].Value] : Convert.ToInt32(instruction.Groups["x"].Value);
			registers[dest] = value;
			break;
		}
			
		case "inc":
		{
			var dest = instruction.Groups["x"].Value;
			registers[dest]++;
			break;
		}
			
		case "dec":
		{
			var dest = instruction.Groups["x"].Value;
			registers[dest]--;
			break;
		}
			
		case "jnz":
		{
			var value = registers.ContainsKey(instruction.Groups["x"].Value) ? registers[instruction.Groups["x"].Value] : Convert.ToInt32(instruction.Groups["x"].Value);
			if (value != 0)
			{
				var distance = Convert.ToInt32(instruction.Groups["y"].Value);
				ip += distance;
				continue;
			}
			else
				break;
		}
	}
	ip++;
}

registers.Dump();
