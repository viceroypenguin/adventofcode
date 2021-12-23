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

}
