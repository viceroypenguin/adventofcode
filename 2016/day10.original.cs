namespace AdventOfCode;

public class Day_2016_10_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 10;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"(?:(?<give_away>bot (?<bot>\d+) gives low to (?<low_type>bot|output) (?<low_num>\d+) and high to (?<high_type>bot|output) (?<high_num>\d+))|(?<receive_value>value (?<value>\d+) goes to bot (?<bot>\d+)))", RegexOptions.Compiled);
		var magic = new { low = 17, high = 61 };

		var instructions =
			input.GetLines()
				.Select(str => regex.Match(str))
				.ToList();

		var destinations =
			instructions.Where(i => i.Groups["give_away"].Success)
				.Select(i => new
				{
					from_bot = Convert.ToInt32(i.Groups["bot"].Value),
					low_bot_destination = i.Groups["low_type"].Value == "bot" ? Convert.ToInt32(i.Groups["low_num"].Value) : default(int?),
					high_bot_destination = i.Groups["high_type"].Value == "bot" ? Convert.ToInt32(i.Groups["high_num"].Value) : default(int?),
					low_output_destination = i.Groups["low_type"].Value == "output" ? Convert.ToInt32(i.Groups["low_num"].Value) : default(int?),
					high_output_destination = i.Groups["high_type"].Value == "output" ? Convert.ToInt32(i.Groups["high_num"].Value) : default(int?),
					values = new List<int>(),
				})
				.ToDictionary(b => b.from_bot);

		foreach (var b in
			instructions.Where(i => i.Groups["receive_value"].Success)
				.Select(i => new
				{
					from_bot = Convert.ToInt32(i.Groups["bot"].Value),
					value = Convert.ToInt32(i.Groups["value"].Value),
				}))
		{
			destinations[b.from_bot].values.Add(b.value);
		}

		var outputs = new int[3];

		while (destinations.Any())
		{
			foreach (var x in destinations.ToList())
			{
				var b = x.Value;
				if (b.values.Count == 2)
				{
					var values = new { low = b.values.Min(), high = b.values.Max(), };
					if (values.low == magic.low && values.high == magic.high)
						Dump('A', b.from_bot);

					destinations.Remove(x.Key);

					if (b.low_bot_destination.HasValue)
						destinations[b.low_bot_destination.Value].values.Add(values.low);
					if (b.high_bot_destination.HasValue)
						destinations[b.high_bot_destination.Value].values.Add(values.high);

					if (b.low_output_destination <= 2)
						outputs[b.low_output_destination.Value] = values.low;
					if (b.high_output_destination <= 2)
						outputs[b.high_output_destination.Value] = values.high;
				}
			}
		}

		Dump('B', outputs[0] * outputs[1] * outputs[2]);
	}
}
