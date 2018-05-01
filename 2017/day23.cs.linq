<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

class Instruction
{
	public string Operation { get; set; }
	public string Source { get; set; }
	public string Destination { get; set; }
}

void Main()
{
	var regex = new Regex(@"^(?<inst>set|sub|mul|jnz) (?<dst>\w|-?\d+)( (?<src>\w|-?\d+))?$", RegexOptions.Compiled);
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day23.input.txt"))
		.Select(inst => regex.Match(inst))
		.Select(m => new Instruction
		{
			Operation = m.Groups["inst"].Value,
			Source = m.Groups["src"].Value,
			Destination = m.Groups["dst"].Value,
		})
		.ToList();

	ProcessInstructions(input);

	CountComposites(input);
}

void ProcessInstructions(IList<Instruction> input)
{
	var registers = new Dictionary<string, long>();

	long getRegister(string r) => registers.ContainsKey(r) ? registers[r] : 0;
	long getValue(string src)
	{
		if (int.TryParse(src, out var x)) return x;
		return getRegister(src);
	}

	var ip = 0;
	var mulInstructions = 0;

	while (ip < input.Count)
	{
		var instruction = input[ip];
		switch (instruction.Operation)
		{
			case "set":
				{
					registers[instruction.Destination] =
						getValue(instruction.Source);
					break;
				}

			case "sub":
				{
					var register = getRegister(instruction.Destination);
					register -= getValue(instruction.Source);
					registers[instruction.Destination] = register;
					break;
				}

			case "mul":
				{
					mulInstructions++;
					var register = getRegister(instruction.Destination);
					register *= getValue(instruction.Source);
					registers[instruction.Destination] = register;
					break;
				}

			case "jnz":
				{
					var value = getValue(instruction.Destination);
					if (value != 0)
					{
						ip += (int)getValue(instruction.Source);
						continue;
					}
					break;
				}
		}

		ip++;
	}

	mulInstructions.Dump("Part A");
}

void CountComposites(IList<Instruction> input)
{
	var initial = Convert.ToInt32(input[0].Source) * 100 + 100000;
	var max = initial - Convert.ToInt32(input[7].Source);
	var maxFactor = (int)Math.Sqrt(max);
	var increment = -Convert.ToInt32(input[30].Source);

	var composites = 0;
	for (var x = initial; x <= max; x += increment)
	{
		if (x % 2 == 0)
		{
			composites++;
			continue;
		}

		for (var n = 3; n <= maxFactor; n += 2)
		{
			if (x % n == 0)
			{
				composites++;
				break;
			}
		}
	}

	composites.Dump("Part B");
}
