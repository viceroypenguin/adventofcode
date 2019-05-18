using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2017_07_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 7;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var regex = new Regex(@"^(?<id>\w+) \((?<weight>\d+)\)( -> ((?<childid>\w+)(,\s*)?)*)?$", RegexOptions.Compiled);

			var nodes = input.GetLines()
				.Select(s => regex.Match(s))
				.Select(m => new
				{
					id = m.Groups["id"].Value,
					weight = Convert.ToInt32(m.Groups["weight"].Value),
					childids = m.Groups["childid"].Success
						? m.Groups["childid"].Captures.OfType<Capture>().Select(c => c.Value).ToList()
						: new List<string>(),
				})
				.ToList();

			var root = nodes.Select(n => n.id)
				.Except(nodes.SelectMany(n => n.childids))
				.Single();
			Dump('A', root);

			var dict = nodes.ToDictionary(n => n.id);
			int getSum(string id)
			{
				var node = dict[id];
				return node.weight + node.childids.Select(getSum).Sum();
			};

			int getVariance(string id)
			{
				var node = dict[id];
				var sums = node.childids.Select(s => new { childid = s, sum = getSum(s), }).ToList();
				var test = sums[0].sum;
				var variances = sums.Skip(1).Where(s => s.sum != test).ToList();
				if (variances.Count == 0)
					return 0;
				var variantId = variances.Count == 1 ? variances[0].childid : sums[0].childid;
				var variance = getVariance(variantId);
				if (variance != 0)
					return variance;
				var adjustment = variances.Count == 1
					? sums[0].sum - variances[0].sum
					: variances[0].sum - sums[0].sum;
				return dict[variantId].weight + adjustment;
			};

			Dump('B', getVariance(root));
		}
	}
}
