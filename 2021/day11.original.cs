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

		// handle one step
		int step(int[][] map)
		{
			// count the flashes
			var flashes = 0;
			// keep track of where to flash neighbors
			var flashPoints = new Queue<(int x, int y)>();

			// increment a cell, and if necessary
			// remember to flash neighbors
			void increment(int x, int y)
			{
				var l = ++map[y][x];
				if (l == 10)
				{
					flashPoints.Enqueue((x, y));
					flashes++;
				}
			}

			// for every point, just increment
			// don't handle flashes yet
			foreach (var ((x, y), l) in map.GetMapPoints())
				increment(x, y);

			// now, for all flash points, trigger 
			// and increment neighbors; will recurse
			// via queue if necessary
			while (flashPoints.Count != 0)
			{
				foreach (var (x, y) in flashPoints.Dequeue()
						.GetCartesianAdjacent(map))
					increment(x, y);
			}

			// now everything is done, reset flashed values
			foreach (var ((x, y), l) in map.GetMapPoints())
				if (l >= 10)
					map[y][x] = 0;

			// how many flashes did we see?
			return flashes;
		}

		// get initial state
		var map = input.GetIntMap();
		
		// keep track of flashes over 100 steps
		var flashes = 0;
		for (int i = 0; i < 100; i++)
			flashes += step(map);

		PartA = flashes.ToString();

		// reset to initial state
		map = input.GetIntMap();
		// how many cells are on map
		// i.e. how many flashes == entire map flashed
		var mapSize = map.Length * map[0].Length;
		for (int i = 1; ; i++)
		{
			// run step
			flashes = step(map);
			// if entire map flashed...
			if (flashes == mapSize)
			{
				// print and we're done.
				PartB = i.ToString();
				return;
			}
		}
	}
}
