namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 24, CodeType.Original)]
public partial class Day_24_Original : IPuzzle
{
	[GeneratedRegex(@"(?<i1>\w{3}) (?<op>AND|XOR|OR) (?<i2>\w{3}) -> (?<o>\w{3})", RegexOptions.Compiled)]
	private static partial Regex LineRegex { get; }

	public (string, string) Solve(PuzzleInput input)
	{
		var split = input.Lines.Split(string.Empty).ToList();
		var wires = split[0]
			.Select(x => x.Split(": "))
			.ToDictionary(x => x[0], x => new Lazy<bool>(() => x[1] == "1"));

		foreach (var m in split[1].Select(m => LineRegex.Match(m)))
		{
			var i1 = m.Groups["i1"].Value;
			var i2 = m.Groups["i2"].Value;
			var op = m.Groups["op"].Value;

			wires[m.Groups["o"].Value] = new Lazy<bool>(
				() => op switch
				{
					"AND" => wires[i1].Value & wires[i2].Value,
					"OR" => wires[i1].Value | wires[i2].Value,
					_ => wires[i1].Value ^ wires[i2].Value,
				}
			);
		}

		var part1 = 0L;
		foreach (var (key, value) in wires.Where(kvp => kvp.Key.StartsWith('z')))
		{
			var bit = int.Parse(key.AsSpan()[1..]);
			if (value.Value)
				part1 |= 1L << bit;
		}

		var part2 = 0;

		return (part1.ToString(), part2.ToString());
	}
}
