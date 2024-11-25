namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 08, CodeType.Original)]
public partial class Day_08_Original : IPuzzle
{
	[GeneratedRegex("^(?<reg_a>\\w+) (?<adj_dir>inc|dec) (?<adj_amt>-?\\d+) if (?<reg_b>\\w+) (?<comp>.{1,2}) (?<val>-?\\d+)$", RegexOptions.ExplicitCapture)]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();

		var instructions = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => new
			{
				dest_reg = m.Groups["reg_a"].Value,
				adj_val = (m.Groups["adj_dir"].Value == "inc" ? +1 : -1) *
					Convert.ToInt32(m.Groups["adj_amt"].Value),
				comp_reg = m.Groups["reg_b"].Value,
				comp_type = m.Groups["comp"].Value,
				comp_value = Convert.ToInt32(m.Groups["val"].Value),
			})
			.ToList();

		var comparisons = new Dictionary<string, Func<int, int, bool>>()
		{
			["=="] = (a, b) => a == b,
			["!="] = (a, b) => a != b,
			["<="] = (a, b) => a <= b,
			[">="] = (a, b) => a >= b,
			["<"] = (a, b) => a < b,
			[">"] = (a, b) => a > b,
		};

		var registers = new Dictionary<string, int>();

		var maxValue = 0;
		foreach (var i in instructions)
		{
			var regValue = registers.GetValueOrDefault(i.comp_reg);
			if (comparisons[i.comp_type](regValue, i.comp_value))
			{
				var destRegValue = registers.GetValueOrDefault(i.dest_reg);
				destRegValue += i.adj_val;
				maxValue = Math.Max(maxValue, destRegValue);
				registers[i.dest_reg] = destRegValue;
			}
		}

		return (
			registers
				.OrderByDescending(kvp => kvp.Value)
				.First()
				.Value
				.ToString(),
			maxValue.ToString());
	}
}
