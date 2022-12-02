namespace AdventOfCode;

public class Day_2021_25_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 25;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input.GetMap();

		var yLength = map.Length;
		var xLength = map[0].Length;

		bool Step(byte[][] map)
		{
			var anyMove = false;

			var moveInstructions = Enumerable.Range(0, yLength)
				.SelectMany(y => Enumerable.Range(0, xLength)
					.Where(x => map[y][x] == '>' && map[y][(x + 1) % xLength] == '.')
					.Select(x => (x, y)))
				.ToList();

			if (moveInstructions.Any())
				anyMove = true;
			foreach (var (x, y) in moveInstructions)
				(map[y][x], map[y][(x + 1) % xLength]) =
					((byte)'.', (byte)'>');

			moveInstructions = Enumerable.Range(0, yLength)
				.SelectMany(y => Enumerable.Range(0, xLength)
					.Where(x => map[y][x] == 'v' && map[(y + 1) % yLength][x] == '.')
					.Select(x => (x, y)))
				.ToList();

			if (moveInstructions.Any())
				anyMove = true;
			foreach (var (x, y) in moveInstructions)
				(map[y][x], map[(y + 1) % yLength][x]) =
					((byte)'.', (byte)'v');

			return anyMove;
		}

		var cnt = 1;
		while (Step(map))
			cnt++;

		PartA = cnt.ToString();
	}
}
