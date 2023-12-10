namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 10, CodeType.Original)]
public partial class Day_10_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();
		var startPosition = map.GetMapPoints()
			.First(p => p.item == 'S');

		IEnumerable<((int x, int y), int cost)> GetNeighbors((int x, int y) p, int cost)
		{
			switch ((char)map[p.y][p.x])
			{
				case '|':
				{
					if (p.y > 0)
						yield return ((p.x, p.y - 1), cost + 1);
					if (p.y < map.Length - 1)
						yield return ((p.x, p.y + 1), cost + 1);
					yield break;
				}

				case '-':
				{
					if (p.x > 0)
						yield return ((p.x - 1, p.y), cost + 1);
					if (p.x < map[0].Length - 1)
						yield return ((p.x + 1, p.y), cost + 1);
					yield break;
				}

				case 'F':
					if (p.y < map.Length - 1)
						yield return ((p.x, p.y + 1), cost + 1);
					if (p.x < map[0].Length - 1)
						yield return ((p.x + 1, p.y), cost + 1);
					yield break;

				case 'L':
					if (p.y > 0)
						yield return ((p.x, p.y - 1), cost + 1);
					if (p.x < map[0].Length - 1)
						yield return ((p.x + 1, p.y), cost + 1);
					yield break;

				case 'J':
					if (p.x > 0)
						yield return ((p.x - 1, p.y), cost + 1);
					if (p.y > 0)
						yield return ((p.x, p.y - 1), cost + 1);
					yield break;

				case '7':
					if (p.x > 0)
						yield return ((p.x - 1, p.y), cost + 1);
					if (p.y < map.Length - 1)
						yield return ((p.x, p.y + 1), cost + 1);
					yield break;

				default:
				{
					var isNorth = (char)map[p.y - 1][p.x] is '|' or 'F' or '7';
					var isEast = (char)map[p.y][p.x + 1] is '-' or '7' or 'J';
					var isSouth = (char)map[p.y + 1][p.x] is '|' or 'J' or 'L';
					var isWest = (char)map[p.y][p.x - 1] is '-' or 'L' or 'F';

					if (isNorth)
					{
						if (isEast)
							goto case 'L';
						else if (isSouth)
							goto case '|';
						else
							goto case 'J';
					}
					else if (isEast)
					{
						if (isSouth)
							goto case 'F';
						else
							goto case '-';
					}
					else
					{
						goto case '7';
					}
				}

			}
		}

		var distances = SuperEnumerable
			.GetShortestPaths<(int x, int y), int>(
				startPosition.p,
				GetNeighbors);

		var part1 = distances.Values.Max(x => x.cost);

		var part2 = 0;
		for (var y = 0; y < map.Length; y++)
		{
			var isInLoop = false;
			for (var x = 0; x < map[0].Length; x++)
			{
				if (!distances.ContainsKey((x, y)))
				{
					if (isInLoop)
						part2++;
				}
				else if (map[y][x] is (byte)'|' or (byte)'F' or (byte)'7')
				{
					isInLoop = !isInLoop;
				}
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
