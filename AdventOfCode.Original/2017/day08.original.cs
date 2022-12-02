namespace AdventOfCode;

public class Day_2017_08_Original : Day
{
	public override int Year => 2017;
	public override int DayNumber => 8;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"^(?<reg_a>\w+) (?<adj_dir>inc|dec) (?<adj_amt>-?\d+) if (?<reg_b>\w+) (?<comp>.{1,2}) (?<val>-?\d+)$", RegexOptions.Compiled);

		var instructions = input.GetLines()
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
		Func<string, int> getRegister = reg => registers.ContainsKey(reg) ? registers[reg] : 0;
		var maxValue = 0;
		foreach (var i in instructions)
		{
			var regValue = getRegister(i.comp_reg);
			if (comparisons[i.comp_type](regValue, i.comp_value))
			{
				var destRegValue = getRegister(i.dest_reg);
				destRegValue += i.adj_val;
				maxValue = Math.Max(maxValue, destRegValue);
				registers[i.dest_reg] = destRegValue;
			}
		}

		// registers.Dump();
		Dump('A',
			registers
				.OrderByDescending(kvp => kvp.Value)
				.First()
				.Value);

		Dump('B', maxValue);
	}
}
