namespace AdventOfCode;

public class Day_2021_19_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 19;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var scanners = input.GetLines()
			// each line that says "scanner" is a new block
			.Segment(x => x.Contains("scanner"))
			// each for each block, parse scanner id and points
			.Select(x => (
				scanner: Convert.ToInt32(Regex.Match(x.First(), "\\d+").Value),
				points: x.Skip(1)
					.Select(l => l.Split(','))
					.Select(l => (
						x: Convert.ToInt32(l[0]),
						y: Convert.ToInt32(l[1]),
						z: Convert.ToInt32(l[2])))
					.ToList()))
			// keep track of euclidean distance between points
			.Select(s => (
				s.scanner,
				s.points,
				distances: s.points.Index()
					.Subsets(2)
					.Select(p => (
						i: p[0].Key,
						j: p[1].Key,
						x: p[0].Value.x - p[1].Value.x,
						y: p[0].Value.y - p[1].Value.y,
						z: p[0].Value.z - p[1].Value.z))
					.Select(p => (p.i, p.j, dst: Math.Sqrt(p.x * p.x + p.y * p.y + p.z * p.z)))
					.ToList()))
			// transform - we want the raw list of distances
			// and we want a list of distance by point id
			.Select(s => (
				s.scanner,
				s.points,
				distances: s.distances.Select(d => d.dst).ToList(),
				distByPointIdx: s.distances.Select(d => (i: d.i, d.dst))
					.Concat(s.distances.Select(d => (i: d.j, d.dst)))
					.GroupBy(
						d => d.i,
						d => d.dst,
						(i, g) => (i, dsts: g.OrderBy(d => d).ToList()))
					.ToList()))
			.ToList();

		// using the raw distances between points for each scanner
		// look at each pair of scanners
		// do they have at least 66 matching distances
		// (12 matching points would have 66 pairs of points)
		var overlaps = scanners.Subsets(2)
			.Where(s => s[0].distances.Intersect(s[1].distances).Count() >= 66)
			.Select(s => (i: s[0].scanner, j: s[1].scanner))
			.ToList();

		// all 24 orientations
		var orientations = new Func<(int x, int y, int z), (int x, int y, int z)>[]
		{
			v => (v.x, v.y, v.z),    v => (v.y, v.z, v.x),    v => (-v.y, v.x, v.z),
			v => (-v.x, -v.y, v.z),  v => (v.y, -v.x, v.z),   v => (v.z, v.y, -v.x),
			v => (v.z, v.x, v.y),    v => (v.z, -v.y, v.x),   v => (v.z, -v.x, -v.y),
			v => (-v.x, v.y, -v.z),  v => (v.y, v.x, -v.z),   v => (v.x, -v.y, -v.z),
			v => (-v.y, -v.x, -v.z), v => (-v.z, v.y, v.x),   v => (-v.z, v.x, -v.y),
			v => (-v.z, -v.y, -v.x), v => (-v.z, -v.x, v.y),  v => (v.x, -v.z, v.y),
			v => (-v.y, -v.z, v.x),  v => (-v.x, -v.z, -v.y), v => (v.y, -v.z, -v.x),
			v => (v.x, v.z, -v.y),   v => (-v.y, v.z, -v.x),  v => (-v.x, v.z, v.y),
		};

		// keep track of each scanner in it's final orientation and position
		var map = new (bool set, (int x, int y, int z) origin, List<(int x, int y, int z)> points)[scanners.Count];
		// start with the first scanner
		map[0] = (true, (0, 0, 0), scanners[0].points);
		while (overlaps.Any())
		{
			// grab the next scanner; any pair where one of them
			// has already been placed will work
			var (i, j) = overlaps.FirstOrDefault(o => map[o.i].set || map[o.j].set);
			// don't need it anymore
			overlaps.Remove((i, j));

			// for simplicity of code, i is the one that's already placed
			// if not, swap them so i is for sure
			if (map[j].set)
				(i, j) = (j, i);

			// use the distance by point to identify which points
			// on each scanner are the same
			// since that point will have the same distance to the other
			// 11 points in both scanners
			var pointMap = scanners[i].distByPointIdx
				.Select(p => (p.i, j: scanners[j].distByPointIdx
					.FirstOrDefault(l => p.dsts.Intersect(l.dsts).Count() >= 11, (-1, default)).i))
				.Where(x => x.j >= 0)
				.ToList();

			// we know which points are the same, figure out the orientation
			// try each one, and see what the distance is between the oriented
			// points; the right orientation will have exactly one unique (x,y,z) distance
			var o = orientations
				.First(o => pointMap
					.Select(a => (
						p: map[i].points[a.i],
						q: o(scanners[j].points[a.j])))
					.Select(a => (x: a.p.x - a.q.x, y: a.p.y - a.q.y, z: a.p.z - a.q.z))
					.Distinct()
					.Count() == 1);

			// figure out the origin; can use any point to do this, so use the first
			// scanner i points are already in final position in map
			// so scanner j must be based on difference between oriented point
			// and matching real point in scanner i
			var p = map[i].points[pointMap[0].i];
			var q = o(scanners[j].points[pointMap[0].j]);
			var origin = (x: p.x - q.x, y: p.y - q.y, z: p.z - q.z);
			
			// save the new adjusted points, along with the origin
			map[j] = (
				true, 
				origin, 
				scanners[j].points
					.Select(o)
					.Select(p => (p.x + origin.x, p.y + origin.y, p.z + origin.z))
					.ToList());
		}

		// how many *distinct* points are there?
		PartA = map.SelectMany(m => m.points).Distinct().Count().ToString();
		// calculate manhattan distance between all pairs
		// and take the max
		PartB = map.Subsets(2)
			.Max(x => Math.Abs(x[0].origin.x - x[1].origin.x)
				+ Math.Abs(x[0].origin.y - x[1].origin.y)
				+ Math.Abs(x[0].origin.z - x[1].origin.z))
			.ToString();
	}
}
