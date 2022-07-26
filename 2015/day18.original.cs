namespace AdventOfCode;

public class Day_2015_18_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 18;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		ExecutePart(false, input);
		ExecutePart(true, input);
	}

	private void ExecutePart(bool part2, byte[] input)
	{
		var lights = input.GetLines()
			.Select(s => s.ToCharArray().Select(c => c == '#').ToArray())
			.ToArray();

		if (part2)
		{
			lights[0][0] = true;
			lights[0][lights[0].Length - 1] = true;
			lights[lights.Length - 1][0] = true;
			lights[lights.Length - 1][lights[0].Length - 1] = true;
		}

		Func<int, int, int> getLight = (x, y) =>
		{
			if (x < 0) return 0;
			if (y < 0) return 0;
			if (x >= lights.Length) return 0;
			if (y >= lights[0].Length) return 0;

			return lights[x][y] ? 1 : 0;
		};
		foreach (var _ in Enumerable.Range(0, 100))
		{
			lights =
				lights
					.Select((row, x) =>
						row
							.Select((col, y) =>
							{
								if (part2 && (
									x == 0 && y == 0 ||
									x == lights.Length - 1 && y == 0 ||
									x == 0 && y == lights[0].Length - 1 ||
									x == lights.Length - 1 && y == lights[0].Length - 1))
									return true;

								var numNeighbors =
									getLight(x - 1, y - 1) +
									getLight(x - 1, y) +
									getLight(x - 1, y + 1) +
									getLight(x, y - 1) +
									getLight(x, y + 1) +
									getLight(x + 1, y - 1) +
									getLight(x + 1, y) +
									getLight(x + 1, y + 1);
								return numNeighbors == 3 || (col && numNeighbors == 2);
							})
							.ToArray())
					.ToArray();
		}

		Dump(part2 ? 'B' : 'A',
			lights.SelectMany(x => x)
				.Where(b => b)
				.Count());
	}
}
