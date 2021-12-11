using System.Collections;

namespace AdventOfCode;

public class Day_2021_11_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 11;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input.GetIntMap();

		var flashes = 0;
		for (int i = 0; i < 100; i++)
		{
			var flashPoints = new Queue<(int x, int y)>();

			void increment(int x, int y)
			{
				var l = ++map[y][x];
				if (l == 10)
				{
					flashPoints.Enqueue((x, y));
					flashes++;
				}
			}
			foreach (var ((x, y), l) in map.GetMapPoints())
				increment(x, y);

			while (flashPoints.Count != 0)
			{
				foreach (var (x, y) in flashPoints.Dequeue()
						.GetCartesianAdjacent(map))
					increment(x, y);
			}

			foreach (var ((x, y), l) in map.GetMapPoints())
				if (l >= 10)
					map[y][x] = 0;
		}

		PartA = flashes.ToString();

		map = input.GetIntMap();
		var mapSize = map.Length * map[0].Length;
		for (int i = 1; ; i++)
		{
			flashes = 0;
			var flashPoints = new Queue<(int x, int y)>();

			void increment(int x, int y)
			{
				var l = ++map[y][x];
				if (l == 10)
				{
					flashPoints.Enqueue((x, y));
					flashes++;
				}
			}
			foreach (var ((x, y), l) in map.GetMapPoints())
				increment(x, y);

			while (flashPoints.Count != 0)
			{
				foreach (var (x, y) in flashPoints.Dequeue()
						.GetCartesianAdjacent(map))
					increment(x, y);
			}

			foreach (var ((x, y), l) in map.GetMapPoints())
				if (l >= 10)
					map[y][x] = 0;

			if (flashes == mapSize)
			{
				PartB = i.ToString();
				return;
			}
		}
	}
}
