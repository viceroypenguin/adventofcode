namespace AdventOfCode;

public class Day_2018_10_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 10;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var regex = new Regex(
			@"position=<(?<posx>(\s*|-)\d+),(?<posy>(\s|-)*\d+)> velocity=<(?<velx>(\s*|-)\d+),(?<vely>(\s|-)*\d+)>",
			RegexOptions.Compiled);

		var points = input.GetLines()
			 .Select(l => regex.Match(l))
			 .Select(m => (
				  // transposing for visibility
				  posx: Convert.ToInt32(m.Groups["posy"].Value),
				  posy: Convert.ToInt32(m.Groups["posx"].Value),
				  velx: Convert.ToInt32(m.Groups["vely"].Value),
				  vely: Convert.ToInt32(m.Groups["velx"].Value)))
			 .ToArray();

		var minyvel = points.MinBy(p => p.vely);
		var maxyvel = points.MaxBy(p => p.vely);

		var steps = Math.Abs(minyvel.posx - maxyvel.posx) / Math.Abs(maxyvel.velx - minyvel.velx);

		var atStep = points
			 .Select(p => (
				  x: p.posx + steps * p.velx,
				  y: p.posy + steps * p.vely))
			 .ToList();

		var minx = atStep.Min(p => p.x);
		var miny = atStep.Min(p => p.y);
		var xdiff = atStep.Max(p => p.x) - minx + 1;
		var ydiff = atStep.Max(p => p.y) - miny + 1;

		var pixels = Enumerable.Range(0, xdiff)
			.Select(x => Enumerable.Range(0, ydiff)
				.Select(y => '.')
				.ToArray())
			.ToArray();

		for (int i = 0; i < points.Length; i++)
		{
			pixels[atStep[i].x - minx][atStep[i].y - miny] = '#';
		}

		DumpScreen('A', pixels);
		Dump('B', steps);
	}
}
