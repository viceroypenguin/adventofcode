<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var regex = new Regex(
     @"position=<(?<posx>(\s*|-)\d+),(?<posy>(\s|-)*\d+)> velocity=<(?<velx>(\s*|-)\d+),(?<vely>(\s|-)*\d+)>",
     RegexOptions.Compiled);

var points = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day10.input.txt"))
     .Select(l => regex.Match(l))
     .Select(m => (
          // transposing for visibility
          posx: Convert.ToInt32(m.Groups["posy"].Value),
          posy: Convert.ToInt32(m.Groups["posx"].Value),
          velx: Convert.ToInt32(m.Groups["vely"].Value),
          vely: Convert.ToInt32(m.Groups["velx"].Value)))
     .ToArray();

var minyvel = points.MinBy(p => p.vely).First();
var maxyvel = points.MaxBy(p => p.vely).First();

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

var pixels = new char[xdiff, ydiff];
for (int x = 0; x < xdiff; x++)
	for (int y = 0; y < ydiff; y++)
		pixels[x, y] = '.';

for (int i = 0; i < points.Length; i++)
{
	pixels[atStep[i].x - minx, atStep[i].y - miny] = '#';
}

pixels.Dump("Part A");
steps.Dump("Part B");
