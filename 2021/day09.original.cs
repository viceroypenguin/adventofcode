using System.Collections;

namespace AdventOfCode;

public class Day_2021_09_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 9;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input.GetLines();

		var sum = 0;
		var lowPoints = new List<(int x, int y)>();

		for (int y = 0; y < map.Length; y++)
			for (int x = 0; x < map[y].Length; x++)
			{
				var c = map[y][x];
				if (y > 0 && map[y - 1][x] <= c)
					continue;
				if (y < map.Length - 1 && map[y + 1][x] <= c)
					continue;
				if (x > 0 && map[y][x - 1] <= c)
					continue;
				if (x < map[y].Length - 1 && map[y][x + 1] <= c)
					continue;

				lowPoints.Add((x, y));
				sum += (c - (byte)'0') + 1;
			}

		PartA = sum.ToString();

		var sizes = new List<long>();
		foreach (var (x, y) in lowPoints)
		{
			var s = 0;
			var seen = new HashSet<(int x, int y)>();

			IEnumerable<(int x, int y)> traverse((int x, int y) p)
			{
				var (x, y) = p;

				if (map[y][x] == '9') yield break;

				if (seen.Contains((x, y))) yield break;
				seen.Add((x, y));

				s++;

				if (y > 0)
					yield return (x, y - 1);
				if (y < map.Length - 1)
					yield return (x, y + 1);
				if (x > 0)
					yield return (x - 1, y);
				if (x < map[y].Length - 1)
					yield return (x + 1, y);
			}

			MoreEnumerable
				.TraverseBreadthFirst(
					(x, y),
					traverse)
				.Consume();

			sizes.Add(s);
		}

		PartB = sizes
			.OrderByDescending(x => x)
			.Take(3)
			.Aggregate(1L, (a, b) => a * b)
			.ToString();
	}
}
