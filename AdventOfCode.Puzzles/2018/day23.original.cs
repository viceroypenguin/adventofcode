namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 23, CodeType.Original)]
public partial class Day_23_Original : IPuzzle
{
	public record struct Bot(int X, int Y, int Z, int P);

	[GeneratedRegex("pos=\\<(?<x>\\-?\\d+),(?<y>\\-?\\d+),(?<z>\\-?\\d+)\\>, r=(?<p>\\d+)")]
	private static partial Regex PositionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = PositionRegex();

		var bots = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => new Bot(
				X: Convert.ToInt32(m.Groups["x"].Value),
				Y: Convert.ToInt32(m.Groups["y"].Value),
				Z: Convert.ToInt32(m.Groups["z"].Value),
				P: Convert.ToInt32(m.Groups["p"].Value)))
			.ToList();

		var powerful = bots
			.OrderByDescending(x => x.P)
			.First();

		var part1 = bots
			.Count(x =>
				Math.Abs(x.X - powerful.X) +
				Math.Abs(x.Y - powerful.Y) +
				Math.Abs(x.Z - powerful.Z) <= powerful.P)
			.ToString();

		var xmin = bots.Min(b => b.X);
		var xdiff = bots.Max(b => b.X) - xmin;
		var ymin = bots.Min(b => b.Y);
		var ydiff = bots.Max(b => b.Y) - ymin;
		var zmin = bots.Min(b => b.Z);
		var zdiff = bots.Max(b => b.Z) - zmin;
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
						b.X - length + (length * x / 5),
						b.Y - length + (length * y / 5),
						b.Z - length + (length * z / 5),
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
					x: b.X - 5 + x,
					y: b.Y - 5 + y,
					z: b.Z - 5 + z))
			.Distinct()
			.Select(l => new BoundingBox(
				l.x,
				l.y,
				l.z,
				0,
				bots))
			.OrderByDescending(b => b.Count)
			.ThenByDescending(b => Math.Abs(b.X) + Math.Abs(b.Y) + Math.Abs(b.Z))
			.Take(5)
			.ToList();

		var part2 = boxes
			.Select(b => Math.Abs(b.X) + Math.Abs(b.Y) + Math.Abs(b.Z))
			.First()
			.ToString();

		return (part1, part2);
	}

	private sealed class BoundingBox : IComparable<BoundingBox>
	{
		public BoundingBox(int x, int y, int z, int length, IList<Bot> bots)
		{
			X = x;
			Y = y;
			Z = z;
			Length = length;

			Bots = bots
				.Where(b =>
					Math.Abs(b.X - X) +
					Math.Abs(b.Y - Y) +
					Math.Abs(b.Z - Z) <= b.P + Length)
				.ToList();
		}

		public int X { get; }
		public int Y { get; }
		public int Z { get; }
		public int Length { get; }

		public IList<Bot> Bots { get; }
		public int Count => Bots.Count;

		public int CompareTo(BoundingBox other) =>
			-Count.CompareTo(other.Count);
	}
}
