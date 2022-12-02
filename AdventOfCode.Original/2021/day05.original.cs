using System.Collections;

namespace AdventOfCode;

public class Day_2021_05_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 5;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines()
			// Parse numbers
			.Select(l => Regex.Match(l, @"(\d+),(\d+) -> (\d+),(\d+)"))
			// convert to integers, identify them
			.Select(m => (
				x1: Convert.ToInt32(m.Groups[1].Value),
				y1: Convert.ToInt32(m.Groups[2].Value),
				x2: Convert.ToInt32(m.Groups[3].Value),
				y2: Convert.ToInt32(m.Groups[4].Value)))
			.ToList();

		// All the hard work is done in DoPart()
		// only difference is counting the diagonals
		PartA = DoPart(lines, skipDiagonals: true).ToString();
		PartB = DoPart(lines, skipDiagonals: false).ToString();
	}

	private static int DoPart(
			List<(int x1, int y1, int x2, int y2)> lines,
			bool skipDiagonals) =>
		lines
			// if not skipping diagonals, then all of them
			// if skipping diagonals, then only when x or y are same
			.Where(x => !skipDiagonals || x.x1 == x.x2 || x.y1 == x.y2)
			// splat/expand each line into all of its constituent points
			.SelectMany(x => GetPointsForLine(x.x1, x.y1, x.x2, x.y2))
			// consolidate points by their coordinates
			.GroupBy(x => x)
			// which groups have more than one point?
			.Where(g => g.Count() > 1)
			// how many of these groups?
			.Count();

	private static IEnumerable<(int x, int y)> GetPointsForLine(int x1, int y1, int x2, int y2)
	{
		// NB: this only works for 45' lines 

		// which direction does the line go in the x-axis?
		var xDir = Math.Sign(x2 - x1);
		// which direction does the line go in the y-axis?
		var yDir = Math.Sign(y2 - y1);
		for (
			// start at beginning to the line
			int x = x1, y = y1;
			// are we at the end of the line?
			// + xDir is so we include the final point of the line
			// in return
			x != (x2 + xDir) || y != (y2 + yDir);
			// advance x/y by direction
			x += xDir, y += yDir)
		{
			// return each point to enumeration
			yield return (x, y);
		}
	}
}
