namespace AdventOfCode;

public class Day_2017_20_Original : Day
{
	public override int Year => 2017;
	public override int DayNumber => 20;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var regex = new Regex(@"^p=<(-?\d+),(-?\d+),(-?\d+)>,\s*v=<(-?\d+),(-?\d+),(-?\d+)>,\s*a=<(-?\d+),(-?\d+),(-?\d+)>$", RegexOptions.Compiled);
		var particles = input.GetLines()
			.Select(s => regex.Match(s))
			.Select((m, i) =>
			(
				i,
				p: (
					x: Convert.ToInt32(m.Groups[1].Value),
					y: Convert.ToInt32(m.Groups[2].Value),
					z: Convert.ToInt32(m.Groups[3].Value)),
				v: (
					x: Convert.ToInt32(m.Groups[4].Value),
					y: Convert.ToInt32(m.Groups[5].Value),
					z: Convert.ToInt32(m.Groups[6].Value)),
				a: (
					x: Convert.ToInt32(m.Groups[7].Value),
					y: Convert.ToInt32(m.Groups[8].Value),
					z: Convert.ToInt32(m.Groups[9].Value)),
				hasCollided: false
			))
			.ToArray();

		Dump('A', particles.OrderBy(x => Math.Abs(x.a.x) + Math.Abs(x.a.y) + Math.Abs(x.a.z)).First());

		var intersectMatrix =
			particles
				.Select(i => particles.Select(j => ParticlesDoIntersect(i, j)).ToArray())
				.ToArray();

		while (true)
		{
			int? collisionTime = null;
			foreach (var i in particles.Where(i => !i.hasCollided))
			{
				var time = particles
					.Where(j => j.i != i.i)
					.Where(j => !j.hasCollided)
					.Select(j => intersectMatrix[i.i][j.i])
					.Min(x => x);

				if (time != null &&
					time <= (collisionTime ?? time))
				{
					collisionTime = time;
				}
			}

			if (collisionTime == null)
				break;

			foreach (var i in particles.Where(i => !i.hasCollided).ToList())
				foreach (var j in particles
						.Where(j => !j.hasCollided)
						.Where(j => intersectMatrix[i.i][j.i] == collisionTime)
						.ToList())
					particles[j.i].hasCollided = true;
		}

		Dump('B', particles.Where(i => !i.hasCollided).Count());

		int? DirectionsDoIntersect(
			(int p, int v, int a) first,
			(int p, int v, int a) second)
		{
			var a = first.a - second.a;
			var b = first.v - second.v;
			var c = first.p - second.p;

			if (c == 0) return 0;

			if (a != 0)
			{
				var t = 0;
				while (Math.Sign(a) != Math.Sign(c) ||
						Math.Sign(a) != Math.Sign(b))
				{
					b += a;
					c += b;
					t++;

					if (c == 0)
						return t;
				}

				return null;
			}
			else if (b != 0)
			{
				if (Math.Sign(b) == Math.Sign(c))
					return null;
				else if (c < 0)
					return (-c % b) == 0 ? -c / b : default(int?);
				else
					return (c % -b) == 0 ? c / -b : default(int?);
			}

			return null;
		}

		int? ParticlesDoIntersect(
			(int i, (int x, int y, int z) p, (int x, int y, int z) v, (int x, int y, int z) a, bool hasCollided) first,
			(int i, (int x, int y, int z) p, (int x, int y, int z) v, (int x, int y, int z) a, bool hasCollided) second)
		{
			var x = DirectionsDoIntersect(
				(first.p.x, first.v.x, first.a.x),
				(second.p.x, second.v.x, second.a.x));
			var y = DirectionsDoIntersect(
				(first.p.y, first.v.y, first.a.y),
				(second.p.y, second.v.y, second.a.y));
			var z = DirectionsDoIntersect(
				(first.p.z, first.v.z, first.a.z),
				(second.p.z, second.v.z, second.a.z));

			if (x == null || y == null || z == null)
				return null;

			var times =
				new[] { x.Value, y.Value, z.Value, }
					.Where(i => i != 0)
					.Distinct()
					.ToList();

			if (times.Count == 1)
				return times[0];
			return null;
		}
	}
}
