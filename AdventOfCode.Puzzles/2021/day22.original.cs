using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 22, CodeType.Original)]
public partial class Day_22_Original : IPuzzle
{
	[GeneratedRegex("-?\\d+")]
	private static partial Regex NumberRegex();

	public (string part1, string part2) Solve(PuzzleInput input)
	{
		// parse out the instructions
		var instructions = input.Lines
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(l => (b: l[..2] == "on", m: NumberRegex().Matches(l).ToList()))
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

		var part1 = DoPartA(instructions);
		var part2 = DoPartB(instructions);

		return (part1, part2);
	}

	private string DoPartA(List<(bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z)> instructions)
	{
		// shortcut for getting every int between lo and hi
		static IEnumerable<int> GetDimension((int lo, int hi) dim) =>
			Enumerable.Range(dim.lo, dim.hi - dim.lo + 1);

		// keep track of which cubes are lit or not
		var map = new Dictionary<(int x, int y, int z), bool>();
		foreach (var (v, x, y, z) in instructions
				// only process inside defined small region
				.Where(a => a.x.lo >= -50 && a.x.hi <= 50
					&& a.y.lo >= -50 && a.y.hi <= 50
					&& a.z.lo >= -50 && a.z.hi <= 50))
			(
				// get every cell from all three dimensions
				from a in GetDimension(x)
				from b in GetDimension(y)
				from c in GetDimension(z)
				// 3d location
				select (a, b, c))
				// set the cell to the instruction
				.ForEach(p => map[p] = v);

		// how many do we have turned on now?
		return map
			.Where(kvp => kvp.Value)
			.Count()
			.ToString();
	}

	private string DoPartB(List<(bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z)> instructions)
	{
		// keep track of all of the boxes
		var boxes = new List<(bool b, (int lo, int hi) x, (int lo, int hi) y, (int lo, int hi) z)>();
		// for each instruction
		foreach (var b1 in instructions)
		{
			// if any existing boxes overlap
			// then we need to add negating overlaps
			// this is true regardless of if the existing
			// box is lit or unlit.
			// unlit boxes in this list will only be from
			// previous overlaps, and so are valid for
			// negating.
			boxes.AddRange(
				boxes
					// get the overlap
					.Select(b2 => Overlap(b1, b2))
					// is it a valid overlap?
					.Where(o => o.x.lo <= o.x.hi
						&& o.y.lo <= o.y.hi
						&& o.z.lo <= o.z.hi)
					.ToList());

			// iff this box is lit, we add it to the list
			if (b1.b)
				boxes.Add(b1);
		}

		// add pos and neg sum of all boxes in the list
		return boxes.Sum(b => BoxSize(b.x, b.y, b.z) * (b.b ? 1 : -1)).ToString();
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
