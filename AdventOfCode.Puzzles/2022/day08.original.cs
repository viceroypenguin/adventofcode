namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 8, CodeType.Original)]
public partial class Day_08_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetIntMap();
		var part1 = 0;
		for (var y = 0; y < map.Length; y++)
			for (var x = 0; x < map[y].Length; x++)
			{
				var height = map[y][x];

				// w
				var visible = true;
				for (var j = 0; visible && j < x; j++)
					if (map[y][j] >= height)
						visible = false;

				if (visible)
				{
					part1++;
					continue;
				}

				// e
				visible = true;
				for (var j = map[y].Length - 1; visible && x < j; j--)
					if (map[y][j] >= height)
						visible = false;

				if (visible)
				{
					part1++;
					continue;
				}

				// n
				visible = true;
				for (var j = 0; visible && j < y; j++)
					if (map[j][x] >= height)
						visible = false;

				if (visible)
				{
					part1++;
					continue;
				}

				// s
				visible = true;
				for (var j = map.Length - 1; visible && y < j; j--)
					if (map[j][x] >= height)
						visible = false;

				if (visible)
				{
					part1++;
					continue;
				}
			}

		var part2 = 0;
		for (var y = 0; y < map.Length; y++)
			for (var x = 0; x < map[y].Length; x++)
			{
				var height = map[y][x];
				var product = 1;

				// w
				var cnt = 0;
				for (var j = x - 1; j >= 0; j--)
				{
					cnt++;
					if (map[y][j] >= height)
						break;
				}

				product *= cnt;

				// e
				cnt = 0;
				for (var j = x + 1; j < map[y].Length; j++)
				{
					cnt++;
					if (map[y][j] >= height)
						break;
				}

				product *= cnt;

				// n
				cnt = 0;
				for (var j = y - 1; j >= 0; j--)
				{
					cnt++;
					if (map[j][x] >= height)
						break;
				}

				product *= cnt;

				// s
				cnt = 0;
				for (var j = y + 1; j < map.Length; j++)
				{
					cnt++;
					if (map[j][x] >= height)
						break;
				}

				product *= cnt;
				if (product > part2)
					part2 = product;
			}

		return (part1.ToString(), part2.ToString());
	}
}
