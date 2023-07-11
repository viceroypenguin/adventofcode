namespace AdventOfCode;

public class Day_2015_16_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 16;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var giftInput =
@"children: 3
cats: 7
samoyeds: 2
pomeranians: 3
akitas: 0
vizslas: 0
goldfish: 5
trees: 3
cars: 2
perfumes: 1";

		var regex = new Regex(@"Sue (\d+):( \w+: \d+,?)+");
		var detailRegex = new Regex(@" ?(\w+): (\d+)");

		var sues = input.GetLines()
			.Select(x => regex.Match(x))
			.Select(x => new
			{
				number = Convert.ToInt32(x.Groups[1].Value),
				details = x.Groups[2].Captures.OfType<Capture>()
					.Select(c => detailRegex.Match(c.Value))
					.Select(c => new
					{
						detailType = c.Groups[1].Value,
						detailValue = Convert.ToInt32(c.Groups[2].Value),
					})
					.ToList(),
			})
			.ToList();

		var giftDetails = giftInput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
			.Select(c => detailRegex.Match(c))
			.Select(c => new
			{
				detailType = c.Groups[1].Value,
				detailValue = Convert.ToInt32(c.Groups[2].Value),
			})
			.ToDictionary(x => x.detailType);

		foreach (var s in sues)
		{
			var flag = true;
			foreach (var d in s.details)
				if (!giftDetails.ContainsKey(d.detailType) ||
					giftDetails[d.detailType].detailValue != d.detailValue)
				{
					flag = false;
					break;
				}

			if (flag)
			{
				Dump('A', s.number);
			}
		}

		foreach (var s in sues)
		{
			var flag = true;
			foreach (var d in s.details)
				if (giftDetails.ContainsKey(d.detailType))
				{
					switch (d.detailType)
					{
						case "cats":
						case "trees":
							// gift detail is minimum value of aunt
							if (giftDetails[d.detailType].detailValue >= d.detailValue)
							{
								flag = false;
								goto skipLoop;
							}
							break;

						case "pomeranians":
						case "goldfish":
							// gift detail is maximum value of aunt
							if (giftDetails[d.detailType].detailValue <= d.detailValue)
							{
								flag = false;
								goto skipLoop;
							}
							break;

						default:
							if (giftDetails[d.detailType].detailValue != d.detailValue)
							{
								flag = false;
								goto skipLoop;
							}
							break;
					}
				}

			if (flag)
			{
				Dump('B', s.number);
			}

skipLoop:
			;
		}
	}
}
