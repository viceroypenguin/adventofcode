namespace AdventOfCode;

public class Day_2016_15_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 15;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"Disc #(?<disc_num>\d+) has (?<num_positions>\d+) positions; at time=0, it is at position (?<initial_position>\d+).", RegexOptions.Compiled);

		var discs =
			input.GetLines()
				.Select(s => regex.Match(s))
				.Select(m => new
				{
					disc = Convert.ToInt32(m.Groups["disc_num"].Value),
					num_positions = Convert.ToInt32(m.Groups["num_positions"].Value),
					initial_position = Convert.ToInt32(m.Groups["initial_position"].Value),
				})
				.ToList();

		for (int initialTime = 0; ; initialTime++)
		{
			var flag = true;
			for (int capsulePosition = 1; flag && capsulePosition <= discs.Count; capsulePosition++)
			{
				var disc = discs[capsulePosition - 1];
				var discPosition = (initialTime + capsulePosition + disc.initial_position) % disc.num_positions;
				if (discPosition != 0) flag = false;
			}

			if (flag)
			{
				Dump('A', initialTime);
				break;
			}
		}

		discs.Add(new { disc = 7, num_positions = 11, initial_position = 0 });
		for (int initialTime = 0; ; initialTime++)
		{
			var flag = true;
			for (int capsulePosition = 1; flag && capsulePosition <= discs.Count; capsulePosition++)
			{
				var disc = discs[capsulePosition - 1];
				var discPosition = (initialTime + capsulePosition + disc.initial_position) % disc.num_positions;
				if (discPosition != 0) flag = false;
			}

			if (flag)
			{
				Dump('B', initialTime);
				break;
			}
		}
	}
}
