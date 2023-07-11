namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 18, CodeType.Original)]
public class Day_18_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		return (
			ExecutePart(false, input.Lines).ToString(),
			ExecutePart(true, input.Lines).ToString());
	}

	private int ExecutePart(bool part2, string[] lines)
	{
		var lights = lines
			.Select(s => s.ToCharArray().Select(c => c == '#').ToArray())
			.ToArray();

		if (part2)
		{
			lights[0][0] = true;
			lights[0][^1] = true;
			lights[^1][0] = true;
			lights[^1][^1] = true;
		}

		int getLight(int x, int y)
		{
			if (x < 0) return 0;
			if (y < 0) return 0;
			if (x >= lights.Length) return 0;
			if (y >= lights[0].Length) return 0;

			return lights[x][y] ? 1 : 0;
		}

		foreach (var _ in Enumerable.Range(0, 100))
		{
			lights =
				lights
					.Select((row, x) =>
						row
							.Select((col, y) =>
							{
								if (part2 && (
									(x == 0 && y == 0) ||
									(x == lights.Length - 1 && y == 0) ||
									(x == 0 && y == lights[0].Length - 1) ||
									(x == lights.Length - 1 && y == lights[0].Length - 1)))
								{
									return true;
								}

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

		return lights
			.SelectMany(SuperEnumerable.Identity)
			.Count(SuperEnumerable.Identity);
	}
}
