namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 07, CodeType.Original)]
public class Day_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var regex = new Regex(@"^(?<id>\w+) \((?<weight>\d+)\)( -> ((?<childid>\w+)(,\s*)?)*)?$", RegexOptions.Compiled);

		var nodes = input.Lines
			.Select(s => regex.Match(s))
			.Select(m => new
			{
				id = m.Groups["id"].Value,
				weight = Convert.ToInt32(m.Groups["weight"].Value),
				childids = m.Groups["childid"].Success
					? m.Groups["childid"].Captures.OfType<Capture>().Select(c => c.Value).ToList()
					: [],
			})
			.ToList();

		var root = nodes.Select(n => n.id)
			.Except(nodes.SelectMany(n => n.childids))
			.Single();

		var dict = nodes.ToDictionary(n => n.id);
		int GetSum(string id)
		{
			var node = dict[id];
			return node.weight + node.childids.Select(GetSum).Sum();
		};

		int GetVariance(string id)
		{
			var node = dict[id];
			var sums = node.childids.Select(s => new { childid = s, sum = GetSum(s), }).ToList();
			var test = sums[0].sum;
			var variances = sums.Skip(1).Where(s => s.sum != test).ToList();
			if (variances.Count == 0)
				return 0;
			var variantId = variances.Count == 1 ? variances[0].childid : sums[0].childid;
			var variance = GetVariance(variantId);
			if (variance != 0)
				return variance;
			var adjustment = variances.Count == 1
				? sums[0].sum - variances[0].sum
				: variances[0].sum - sums[0].sum;
			return dict[variantId].weight + adjustment;
		};

		var partB = GetVariance(root);
		return (root, partB.ToString());
	}
}
