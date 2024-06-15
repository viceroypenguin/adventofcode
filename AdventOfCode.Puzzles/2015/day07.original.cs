using System.Diagnostics;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 07, CodeType.Original)]
public partial class Day_07_Original : IPuzzle
{
	[GeneratedRegex(
		@"^\s*(
				(?<assign>\w+) |
				(?<not>NOT\s+(?<not_arg>\w+)) |
				((?<arg1>\w+)\s+(?<command>AND|OR|LSHIFT|RSHIFT)\s+(?<arg2>\w+))
			)
			\s*->\s*
			(?<dest>\w+)\s*$",
		RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace)]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();

		var wires = new Dictionary<string, Lazy<ushort>>();

		Func<ushort> GetArgument(string arg) =>
			ushort.TryParse(arg, out var num)
				? () => num
				: () => wires[arg].Value;

		Dictionary<string, Lazy<ushort>> ResetWires() =>
			input.Lines
				.Select(l => regex.Match(l))
				.Select(m =>
				{
					var destination = m.Groups["dest"].Value;
					if (m.Groups["assign"].Success)
						return (destination, value: new Lazy<ushort>(GetArgument(m.Groups["assign"].Value)));
					if (m.Groups["not"].Success)
					{
						var arg = GetArgument(m.Groups["not_arg"].Value);
						return (destination, value: new Lazy<ushort>(() => (ushort)~arg()));
					}

					if (m.Groups["command"].Success)
					{
						var arg1 = GetArgument(m.Groups["arg1"].Value);
						var arg2 = GetArgument(m.Groups["arg2"].Value);
						return m.Groups["command"].Value switch
						{
							"AND" => (destination, value: new Lazy<ushort>(() => (ushort)(arg1() & arg2()))),
							"OR" => (destination, value: new Lazy<ushort>(() => (ushort)(arg1() | arg2()))),
							"LSHIFT" => (destination, value: new Lazy<ushort>(() => (ushort)(arg1() << arg2()))),
							"RSHIFT" => (destination, value: new Lazy<ushort>(() => (ushort)(arg1() >> arg2()))),
							_ => throw new UnreachableException(),
						};
					}

					throw new UnreachableException();
				})
				.ToDictionary(x => x.destination, x => x.value);

		wires = ResetWires();
		var partA = wires["a"].Value;

		wires = ResetWires();
		wires["b"] = new Lazy<ushort>(() => partA);
		var partB = wires["a"].Value;

		return (partA.ToString(), partB.ToString());
	}
}
