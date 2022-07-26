namespace AdventOfCode;

public class Day_2016_03_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 3;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines();
		Dump('A', lines
			.Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
			.Select(line => new { A = Convert.ToInt32(line[0]), B = Convert.ToInt32(line[1]), C = Convert.ToInt32(line[2]), })
			.Where(tri => tri.A + tri.B > tri.C && tri.A + tri.C > tri.B && tri.B + tri.C > tri.A)
			.Count());
		Dump('B', lines
			.Select((line, idx) => new { parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), idx })
			.GroupBy(x => x.idx / 3)
			.SelectMany(x =>
			{
				var arr = x.Select(z => z.parts).ToArray();
				return new[]
				{
						new { A = Convert.ToInt32(arr[0][0]), B = Convert.ToInt32(arr[1][0]), C = Convert.ToInt32(arr[2][0]), },
						new { A = Convert.ToInt32(arr[0][1]), B = Convert.ToInt32(arr[1][1]), C = Convert.ToInt32(arr[2][1]), },
						new { A = Convert.ToInt32(arr[0][2]), B = Convert.ToInt32(arr[1][2]), C = Convert.ToInt32(arr[2][2]), },
				};
			})
			.Where(tri => tri.A + tri.B > tri.C && tri.A + tri.C > tri.B && tri.B + tri.C > tri.A)
			.Count());
	}
}
