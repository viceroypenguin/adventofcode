namespace AdventOfCode;

public class Day_2021_14_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 14;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines();

		DoPartA(lines);
		DoPartB(lines);
	}

	private void DoPartA(string[] lines)
	{
		var polymer = lines[0].ToList();
		var instructions = lines
			.Skip(1)
			.Select(x => x.Split(" -> "))
			.Select(x => (old: x[0].ToList(), @new: new[] { x[0][0], x[1][0], }))
			.ToList();

		List<char> insert(List<char> polymer) =>
			polymer
				.Lead(1, (a, b) =>
				{
					if (b == default) return new[] { a, };
					var (_, @new) = instructions
						.FirstOrDefault(x => x.old[0] == a && x.old[1] == b);
					return @new ?? (new[] { a, });
				})
				.SelectMany(x => x)
				.ToList();

		for (int i = 0; i < 10; i++)
			polymer = insert(polymer);

		var elements = polymer
			.GroupBy(x => x, (c, g) => (c, cnt: g.Count()))
			.OrderBy(x => x.cnt)
			.ToList();

		PartA = (elements[^1].cnt - elements[0].cnt).ToString();
	}

	private void DoPartB(string[] lines)
	{
		var polymer = lines[0]
			.Window(2)
			.Select(x => (key: string.Join("", x), cnt: 1L))
			.ToList();
		var instructions = lines
			.Skip(1)
			.Select(x => x.Split(" -> "))
			.ToDictionary(
				x => x[0],
				x => new[]
				{
					string.Join("", x[0][0], x[1][0]),
					string.Join("", x[1][0], x[0][1]),
				});

		List<(string key, long cnt)> insert(List<(string key, long cnt)> polymer) =>
			polymer
				.SelectMany(x => instructions[x.key]
					.Select(y => (key: y, x.cnt)))
				.GroupBy(x => x.key, (key, g) => (key, g.Sum(x => x.cnt)))
				.ToList();

		for (int i = 0; i < 40; i++)
			polymer = insert(polymer);

		var elements = polymer
			.Append((lines[0][^1..], 1))
			.GroupBy(x => x.key[0], (c, g) => (c, cnt: g.Sum(x => x.cnt)))
			.OrderBy(x => x.cnt)
			.ToList();

		PartB = (elements[^1].cnt - elements[0].cnt).ToString();
	}
}
