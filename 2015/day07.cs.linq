<Query Kind="Statements" />

var regex = new Regex(
	@"^\s*(
		(?<assign>\w+) |
		(?<not>NOT\s+(?<not_arg>\w+)) |
		((?<arg1>\w+)\s+(?<command>AND|OR|LSHIFT|RSHIFT)\s+(?<arg2>\w+))
	)
	\s*->\s*
	(?<dest>\w+)\s*$",
	RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);

Dictionary<string, Lazy<ushort>> wires = new Dictionary<string, Lazy<ushort>>();

Func<ushort> GetArgument(string arg)
{
	if (ushort.TryParse(arg, out var num))
		return () => num;
	return () => wires[arg].Value;
}

void ResetWires()
{
	wires = 
		File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day07.input.txt"))
			.Select(l => regex.Match(l))
			.Select(m =>
			{
				var destination = m.Groups["dest"].Value;
				if (m.Groups["assign"].Success)
					return (destination: destination, value: new Lazy<ushort>(GetArgument(m.Groups["assign"].Value)));
				if (m.Groups["not"].Success)
				{
					var arg = GetArgument(m.Groups["not_arg"].Value);
					return (destination: destination, value: new Lazy<ushort>(() => (ushort)(~arg())));
				}
				if (m.Groups["command"].Success)
				{
					var arg1 = GetArgument(m.Groups["arg1"].Value);
					var arg2 = GetArgument(m.Groups["arg2"].Value);
					switch (m.Groups["command"].Value)
					{
						case "AND": return (destination: destination, value: new Lazy<ushort>(() => (ushort)(arg1() & arg2())));
						case "OR": return (destination: destination, value: new Lazy<ushort>(() => (ushort)(arg1() | arg2())));
						case "LSHIFT": return (destination: destination, value: new Lazy<ushort>(() => (ushort)(arg1() << arg2())));
						case "RSHIFT": return (destination: destination, value: new Lazy<ushort>(() => (ushort)(arg1() >> arg2())));
					}
				}
				throw new InvalidOperationException();
			})
			.ToDictionary(x => x.destination, x => x.value);
}

ResetWires();
var partA = wires["a"].Value.Dump("Part A");

ResetWires();
wires["b"] = new Lazy<ushort>(() => partA);
wires["a"].Value.Dump("Part B");
