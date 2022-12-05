namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 25, CodeType.Original)]
public class Day_25_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

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

		return (cnt.ToString(), string.Empty);
	}
}
