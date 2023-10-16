namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
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
		var map = input.Bytes.GetIntMap();
		
		// keep track of flashes over 100 steps
		var flashes = 0;
		for (var i = 0; i < 100; i++)
			flashes += step(map);

		var part1 = flashes.ToString();

		// reset to initial state
		map = input.Bytes.GetIntMap();
		// how many cells are on map
		// i.e. how many flashes == entire map flashed
		var mapSize = map.Length * map[0].Length;
		for (var i = 1; ; i++)
		{
			// run step
			flashes = step(map);
			// if entire map flashed...
			if (flashes == mapSize)
			{
				// print and we're done.
				var part2 = i.ToString();
				return (part1, part2);
			}
		}
	}
}
