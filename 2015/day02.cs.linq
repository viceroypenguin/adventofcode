<Query Kind="Statements" />

var regex = new Regex(@"(?<l>\d+)x(?<w>\d+)x(?<h>\d+)", RegexOptions.Compiled);
var boxes = File.ReadLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day02.input.txt"))
	.Select(l => regex.Match(l))
	.Select(m => new[]
	{
		Convert.ToInt32(m.Groups["l"].Value),
		Convert.ToInt32(m.Groups["w"].Value),
		Convert.ToInt32(m.Groups["h"].Value),
	}.OrderBy(l=>l).ToArray())
	.ToList();

var totalWrappingPaper =
	boxes
		.Select(b => new[] { b[0] * b[1], b[0] * b[2], b[1] * b[2], }.OrderBy(a=>a).ToArray())
		.Select(a => 3 * a[0] + 2 * a[1] + 2 * a[2])
		.Sum();
		
var totalRibbonLength =
	boxes
		.Select(b => b[0] * b[1] * b[2] + 2 * b[0] + 2 * b[1])
		.Sum();
		
totalWrappingPaper.Dump("Part A");
totalRibbonLength.Dump("Part B");
