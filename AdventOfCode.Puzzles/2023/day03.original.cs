namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 03, CodeType.Original)]
public partial class Day_03_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		var part1 = 0;
		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] is >= (byte)'0' and <= (byte)'9')
				{
					var start = x;
					while (x < map[y].Length
						&& map[y][x] is >= (byte)'0' and <= (byte)'9')
					{
						x++;
					}

					var end = x;

					var flag = false;
					var num = 0;
					for (x = start; x < end; x++)
					{
						num = (num * 10) + map[y][x] - '0';
						foreach (var (py, px) in (y, x).GetCartesianAdjacent(map))
							flag |= map[py][px] is (< (byte)'0' or > (byte)'9') and not (byte)'.';
					}

					if (flag)
						part1 += num;
				}
			}
		}

		var part2 = 0L;
		for (var y = 0; y < map.Length; y++)
		{
			for (var x = 0; x < map[y].Length; x++)
			{
				if (map[y][x] is (byte)'*')
				{
					var num1 = 0L;
					var num2 = 0L;
					foreach (var (py, px) in (y, x).GetCartesianAdjacent(map))
					{
						if (map[py][px] is >= (byte)'0' and <= (byte)'9')
						{
							var qx = px;
							while (qx >= 0 && map[py][qx] is >= (byte)'0' and <= (byte)'9')
								qx--;
							qx++;

							var num = 0;
							while (qx < map[py].Length && map[py][qx] is >= (byte)'0' and <= (byte)'9')
							{
								num = (num * 10) + map[py][qx] - '0';
								qx++;
							}

							if (num != num1 && num != num2)
							{
								if (num1 == 0) num1 = num;
								else num2 = num;
							}
						}
					}

					part2 += num1 * num2;
				}
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
