using static AdventOfCode.Common.Extensions.NumberExtensions;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 10, CodeType.Original)]
public class Day_10_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Lines
			.Select((r, y) => r.Select((c, x) => (x, y, isAsteroid: c == '#')).ToArray())
			.ToArray();

		var mapX = map[0].Length;
		var mapY = map.Length;

		var asteroids = map
			.SelectMany(r => r)
			.Where(a => a.isAsteroid)
			.Select(a => (a.x, a.y))
			.ToHashSet();

		var maxSee = 0;
		var maxCoords = (x: -1, y: -1);

		foreach (var station in asteroids)
		{
			var (x, y) = station;
			var see = 0;
			foreach (var a in asteroids)
			{
				if (a == station)
					// ignore self
					continue;

				var xDist = a.x - x;
				var yDist = a.y - y;

				if (xDist == 0)
				{
					yDist = Math.Sign(yDist);
				}
				else if (yDist == 0)
				{
					xDist = Math.Sign(xDist);
				}
				else
				{
					var gcd = (int)Gcd(Math.Abs(xDist), Math.Abs(yDist));
					if (gcd == 1)
					{
						see++;
						continue;
					}

					xDist /= gcd;
					yDist /= gcd;
				}

				int _x = x + xDist, _y = y + yDist;
				var flag = true;
				while (flag
					&& _x >= 0 && _x < mapX
					&& _y >= 0 && _y < mapY
					&& (_x, _y) != a)
				{
					if (asteroids.Contains((_x, _y)))
					{
						flag = false;
					}
					else
					{
						_x += xDist;
						_y += yDist;
					}
				}

				if (flag)
					see++;
			}

			if (see > maxSee)
			{
				maxSee = see;
				maxCoords = (x, y);
			}
		}

		var part1 = maxSee.ToString();

		var queues = asteroids
			.Where(a => a != maxCoords)
			.Select(a =>
			{
				var xDist = a.x - maxCoords.x;
				var yDist = a.y - maxCoords.y;

				var angle = Math.Atan2(xDist, yDist);
				return (a.x, a.y, angle, dist: Math.Sqrt((xDist * xDist) + (yDist * yDist)));
			})
			.ToLookup(a => a.angle)
			.OrderByDescending(a => a.Key)
			.Select(a => new Queue<(int x, int y, double angle, double dist)>(a.OrderBy(b => b.dist)))
			.ToList();

		static IEnumerable<(int x, int y, double angle, double dist)> GetValue(
			Queue<(int x, int y, double angle, double dist)> q)
		{
			if (q.Count > 0)
				yield return q.Dequeue();
		}

		var part2 = queues.Repeat()
			.SelectMany(GetValue)
			.Skip(199)
			.Take(1)
			.Select(a => (a.x * 100) + a.y)
			.Single()
			.ToString();

		return (part1, part2);
	}
}
