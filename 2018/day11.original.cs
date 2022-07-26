namespace AdventOfCode;

public class Day_2018_11_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 11;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var serialNumber = Convert.ToInt32(input.GetString());
		var cells = new int[301, 301];

		for (int x = 1; x <= 300; x++)
		{
			var rackId = x + 10;

			for (int y = 1; y <= 300; y++)
			{
				var powerLevel = (rackId * y + serialNumber) * rackId;
				cells[x, y] = (powerLevel % 1000) / 100 - 5;
			}
		}

		Dump('A',
		(
			from x in Enumerable.Range(1, 298)
			from y in Enumerable.Range(1, 298)
			select (x, y, sum: (
				from x2 in Enumerable.Range(x, 3)
				from y2 in Enumerable.Range(y, 3)
				select cells[x2, y2]).Sum())
		)
			.OrderByDescending(x => x.sum)
			.First());

		Dump('B',
		(
			from size in Enumerable.Range(1, 30)
			from x in Enumerable.Range(1, 301 - size)
			from y in Enumerable.Range(1, 301 - size)
			select (x, y, size, sum: (
				from x2 in Enumerable.Range(x, size)
				from y2 in Enumerable.Range(y, size)
				select cells[x2, y2]).Sum())
		)
			.OrderByDescending(x => x.sum)
			.First());
	}
}
