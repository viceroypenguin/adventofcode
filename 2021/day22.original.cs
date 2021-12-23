namespace AdventOfCode;

public class Day_2021_22_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 22;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetLines()
			.Select(l => (b: l[..2] == "on", m: Regex.Matches(l, @"-?\d+").ToList()))
			.Select(m => (
				m.b,
				x: (
					lo: Convert.ToInt32(m.m[0].Value),
					hi: Convert.ToInt32(m.m[1].Value)),
				y: (
					lo: Convert.ToInt32(m.m[2].Value),
					hi: Convert.ToInt32(m.m[3].Value)),
				z: (
					lo: Convert.ToInt32(m.m[4].Value),
					hi: Convert.ToInt32(m.m[5].Value))))
			.ToList();

		DoPartA(instructions);
		DoPartB(instructions);
	}

	private void DoPartA(List<(bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z)> instructions)
	{
		var map = new Dictionary<(int x, int y, int z), bool>();
		static IEnumerable<int> GetDimension((int lo, int hi) dim) =>
			Enumerable.Range(dim.lo, dim.hi - dim.lo + 1);
		foreach (var (v, x, y, z) in instructions
				.Where(a => a.x.lo >= -50 && a.x.hi <= 50
					&& a.y.lo >= -50 && a.y.hi <= 50
					&& a.z.lo >= -50 && a.z.hi <= 50))
			(
				from a in GetDimension(x)
				from b in GetDimension(y)
				from c in GetDimension(z)
				select (a, b, c))
				.ForEach(p => map[p] = v);

		PartA = map
			.Where(kvp => kvp.Value)
			.Count()
			.ToString();
	}

	private void DoPartB(List<(bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z)> instructions)
	{
		var boxes = new List<(bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z)>();
		foreach (var b1 in instructions)
		{
			boxes.AddRange(
				boxes
					.Select(b2 => Overlap(b1, b2))
					.Where(o => o.x.lo <= o.x.hi
						&& o.y.lo <= o.y.hi
						&& o.z.lo <= o.z.hi)
					.ToList());

			if (b1.b)
				boxes.Add(b1);
		}

		PartB = boxes.Sum(b => BoxSize(b.x, b.y, b.z) * (b.b ? 1 : -1)).ToString();
	}

	private static long BoxSize(
		(int lo, int hi) x,
		(int lo, int hi) y,
		(int lo, int hi) z) =>
			(x.hi - x.lo + 1L)
			* (y.hi - y.lo + 1)
			* (z.hi - z.lo + 1);

	private static (bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z) Overlap(
			(bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z) b1,
			(bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z) b2) =>
		(
			!b2.b,
			(lo: Math.Max(b1.x.lo, b2.x.lo), hi: Math.Min(b1.x.hi, b2.x.hi)),
			(lo: Math.Max(b1.y.lo, b2.y.lo), hi: Math.Min(b1.y.hi, b2.y.hi)),
			(lo: Math.Max(b1.z.lo, b2.z.lo), hi: Math.Min(b1.z.hi, b2.z.hi)));
}
