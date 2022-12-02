namespace AdventOfCode;

public class Day_2019_10_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 10;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var map = input
			.GetLines()
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
					yDist = Math.Sign(yDist);
				else if (yDist == 0)
					xDist = Math.Sign(xDist);
				else
				{
					var gcd = GCD(Math.Abs(xDist), Math.Abs(yDist));
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
						flag = false;
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

		PartA = maxSee.ToString();

		var queues = asteroids
			.Where(a => a != maxCoords)
			.Select(a =>
			{
				var xDist = a.x - maxCoords.x;
				var yDist = a.y - maxCoords.y;

				var angle = Math.Atan2(xDist, yDist);
				return (a.x, a.y, angle, dist: Math.Sqrt(xDist * xDist + yDist * yDist));
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

		PartB = queues.Repeat()
			.SelectMany(GetValue)
			.Skip(199)
			.Take(1)
			.Select(a => a.x * 100 + a.y)
			.Single()
			.ToString();
	}

	static int GCD(int a, int b)
	{
		while (b != 0)
		{
			var mod = a % b;
			a = b;
			b = mod;
		}

		return a;
	}
}
