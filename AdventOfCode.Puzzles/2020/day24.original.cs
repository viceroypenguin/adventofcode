namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 24, CodeType.Original)]
public class Day_24_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var tiles = new Dictionary<(int e, int ne, int se), bool>();

		int dir = 0, e = 0, ne = 0, se = 0;
		foreach (var c in input.Bytes)
		{
			switch (c)
			{
				case (byte)'\n':
					tiles[(e, ne, se)] = !tiles.GetValueOrDefault((e, ne, se));
					dir = 0; e = 0; ne = 0; se = 0;
					continue;

				case (byte)'n':
				case (byte)'s':
					dir = c;
					continue;

				case (byte)'e':
				case (byte)'w':

					dir = (dir << 8) + c;
					break;
			}

			(e, ne, se) = Move(e, ne, se, dir);
			dir = 0;
		}

		var part1 = tiles.Values.Count(x => x).ToString();

		for (int i = 0; i < 100; i++)
			tiles = Step(tiles);
		var part2 = tiles.Values.Count(x => x).ToString();

		return (part1, part2);
	}

	private static (int e, int ne, int se) Move(int e, int ne, int se, int dir)
	{
		switch (dir)
		{
			case 0x65: // e
				if (e < 0) e++;
				else if (se < 0 || ne < 0) { ne++; se++; }
				else e++;
				break;

			case 0x77: // w
				if (e > 0) e--;
				else if (se > 0 || ne > 0) { se--; ne--; }
				else e--;
				break;

			case 0x6e65: //ne
				if (ne < 0) ne++;
				else if (e < 0 || se > 0) { se--; e++; }
				else ne++;
				break;

			case 0x6e77: //nw
				if (se > 0) se--;
				else if (e > 0 || ne < 0) { e--; ne++; }
				else se--;
				break;

			case 0x7377: //sw
				if (ne > 0) ne--;
				else if (e > 0 || se < 0) { se++; e--; }
				else ne--;
				break;

			case 0x7365: //se
				if (se < 0) se++;
				else if (e < 0 || ne > 0) { e++; ne--; }
				else se++;
				break;
		}

		return (e, ne, se);
	}

	private static readonly int[] directions =
		new[]
		{
			0x65,
			0x77,
			0x6e65,
			0x6e77,
			0x7377,
			0x7365,
		};
	private static Dictionary<(int e, int ne, int se), bool> Step(Dictionary<(int e, int ne, int se), bool> input)
	{
		var @new = new Dictionary<(int e, int ne, int se), bool>();
		var whites = new HashSet<(int e, int ne, int se)>();
		foreach (var (pos, val) in input)
		{
			if (!val) continue;

			var cnt = 0;
			foreach (var d in directions)
			{
				var neighbor = Move(pos.e, pos.ne, pos.se, d);
				if (input.GetValueOrDefault(neighbor))
					cnt++;
				else
					whites.Add(neighbor);
			}

			if (cnt == 1 || cnt == 2)
				@new[pos] = true;
		}

		foreach (var pos in whites)
		{
			var cnt = 0;
			foreach (var d in directions)
			{
				var neighbor = Move(pos.e, pos.ne, pos.se, d);
				if (input.GetValueOrDefault(neighbor))
					cnt++;
			}

			if (cnt == 2)
				@new[pos] = true;
		}

		return @new;
	}
}
