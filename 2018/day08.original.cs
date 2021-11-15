namespace AdventOfCode;

public class Day_2018_08_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 8;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var data = input.GetString()
			.Trim()
			.Split()
			.Select(l => Convert.ToInt32(l))
			.ToList();

		int index = 0;
		int GetMetadataValue()
		{
			var childNodes = data[index++];
			var metadataNodes = data[index++];

			return
				Enumerable.Range(0, childNodes)
					.Sum(_ => GetMetadataValue()) +
				Enumerable.Range(0, metadataNodes)
					.Sum(_ => data[index++]);
		}

		Dump('A', GetMetadataValue());

		index = 0;
		int GetNodeValue()
		{
			var childNodes = data[index++];
			var metadataNodes = data[index++];

			if (childNodes == 0)
				return Enumerable.Range(0, metadataNodes)
					.Sum(_ => data[index++]);

			var nodes = Enumerable.Range(0, childNodes)
				.Select(_ => GetNodeValue())
				.ToList();

			return Enumerable.Range(0, metadataNodes)
				.Select(_ => data[index++])
				.Sum(i => i <= nodes.Count
					? nodes[i - 1]
					: 0);
		}

		Dump('B', GetNodeValue());
	}
}
