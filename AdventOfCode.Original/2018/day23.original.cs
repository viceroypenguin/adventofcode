namespace AdventOfCode;

public class Day_2018_23_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 23;
	public override CodeType CodeType => CodeType.Original;

	public class Bot
	{
		public int x;
		public int y;
		public int z;
		public int p;
	}
	IList<Bot> bots;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"pos=\<(?<x>\-?\d+),(?<y>\-?\d+),(?<z>\-?\d+)\>, r=(?<p>\d+)");

		bots = input.GetLines()
			.Select(l => regex.Match(l))
			.Select(m => new Bot
			{
				x = Convert.ToInt32(m.Groups["x"].Value),
				y = Convert.ToInt32(m.Groups["y"].Value),
				z = Convert.ToInt32(m.Groups["z"].Value),
				p = Convert.ToInt32(m.Groups["p"].Value),
			})
			.ToList();

		var powerful = bots
			.OrderByDescending(x => x.p)
			.First();

		Dump('A', bots
			.Count(x =>
				Math.Abs(x.x - powerful.x) +
				Math.Abs(x.y - powerful.y) +
				Math.Abs(x.z - powerful.z) <= powerful.p));

		var xmin = bots.Min(b => b.x);
		var xdiff = bots.Max(b => b.x) - xmin;
		var ymin = bots.Min(b => b.y);
		var ydiff = bots.Max(b => b.y) - ymin;
		var zmin = bots.Min(b => b.z);
		var zdiff = bots.Max(b => b.z) - zmin;
		var length = (xdiff + ydiff + zdiff) / 10;

		var boxes = (
				from x in Enumerable.Range(0, 11)
				from y in Enumerable.Range(0, 11)
				from z in Enumerable.Range(0, 11)
				select new BoundingBox(
					xmin + (xdiff * x / 10),
					ymin + (ydiff * y / 10),
					zmin + (zdiff * z / 10),
					length,
					bots)
			)
			.OrderByDescending(b => b.Count)
			.Take(200)
			.ToList();

		while (length >= 10)
		{
			length = Math.Max(5, length /= 10);

			boxes = boxes
				.SelectMany(b =>
					from x in Enumerable.Range(0, 11)
					from y in Enumerable.Range(0, 11)
					from z in Enumerable.Range(0, 11)
					select new BoundingBox(
						b.x - length + (length * x / 5),
						b.y - length + (length * y / 5),
						b.z - length + (length * z / 5),
						length,
						b.Bots))
				.OrderByDescending(b => b.Count)
				.Take(200)
				.ToList();
		}

		boxes = boxes
			.SelectMany(b =>
				from x in Enumerable.Range(0, 11)
				from y in Enumerable.Range(0, 11)
				from z in Enumerable.Range(0, 11)
				select (
					x: b.x - 5 + x,
					y: b.y - 5 + y,
					z: b.z - 5 + z))
			.Distinct()
			.Select(l => new BoundingBox(
				l.x,
				l.y,
				l.z,
				0,
				bots))
			.OrderByDescending(b => b.Count)
			.ThenByDescending(b => Math.Abs(b.x) + Math.Abs(b.y) + Math.Abs(b.z))
			.Take(5)
			.ToList();

		Dump('B',
			boxes
				.Select(b => Math.Abs(b.x) + Math.Abs(b.y) + Math.Abs(b.z))
				.First());
	}

	class BoundingBox : IComparable<BoundingBox>
	{
		public BoundingBox(int x, int y, int z, int length, IList<Bot> bots)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.length = length;

			Bots = bots
				.Where(b =>
					Math.Abs(b.x - this.x) +
					Math.Abs(b.y - this.y) +
					Math.Abs(b.z - this.z) <= (b.p + this.length))
				.ToList();
		}

		public int x { get; }
		public int y { get; }
		public int z { get; }
		public int length { get; }

		public IList<Bot> Bots { get; }
		public int Count => Bots.Count;

		public int CompareTo(BoundingBox other) =>
			-this.Count.CompareTo(other.Count);
	}
}
