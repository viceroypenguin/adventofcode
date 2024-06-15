namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 10, CodeType.Original)]
public partial class Day_10_Original : IPuzzle
{
	[GeneratedRegex("position=<(?<posx>(\\s*|-)\\d+),(?<posy>(\\s|-)*\\d+)> velocity=<(?<velx>(\\s*|-)\\d+),(?<vely>(\\s|-)*\\d+)>", RegexOptions.Compiled)]
	private static partial Regex PositionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = PositionRegex();

		var points = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => (
				// transposing for visibility
				posx: Convert.ToInt32(m.Groups["posy"].Value),
				posy: Convert.ToInt32(m.Groups["posx"].Value),
				velx: Convert.ToInt32(m.Groups["vely"].Value),
				vely: Convert.ToInt32(m.Groups["velx"].Value)
			))
			.ToArray();

		var minyvel = points.MinBy(p => p.vely);
		var maxyvel = points.MaxBy(p => p.vely);

		var steps = Math.Abs(minyvel.posx - maxyvel.posx) / Math.Abs(maxyvel.velx - minyvel.velx);

		var atStep = points
			 .Select(p => (
				  x: p.posx + (steps * p.velx),
				  y: p.posy + (steps * p.vely)))
			 .ToList();

		var minx = atStep.Min(p => p.x);
		var miny = atStep.Min(p => p.y);
		var xdiff = atStep.Max(p => p.x) - minx + 1;
		var ydiff = atStep.Max(p => p.y) - miny + 1;

		var pixels = Enumerable.Range(0, xdiff)
			.Select(x => Enumerable.Range(0, ydiff)
				.Select(y => ' ')
				.ToArray())
			.ToArray();

		for (var i = 0; i < points.Length; i++)
		{
			pixels[atStep[i].x - minx][atStep[i].y - miny] = '█';
		}

		var part1 = string.Join(Environment.NewLine, pixels
			.Select(l => string.Join("", l)));
		var part2 = steps.ToString();
		return (part1, part2);
	}
}
