using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_07_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 7;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var regex = new Regex(
				@"^(?<container>[\w ]+?) bags contain ((?<none>no other bags)|((?<contained>[\w ]+) bags?,? ?)+).$",
				RegexOptions.ExplicitCapture);

			var bagRules = input.GetLines()
				.Select(l => regex.Match(l))
				.ToDictionary(
					m => m.Groups["container"].Value,
					m => m.Groups["contained"].Captures
						.Select(c => Regex.Match(c.Value, @"^(\d+) (.*)$"))
						.Select(m => (
							count: Convert.ToInt32(m.Groups[1].Value),
							color: m.Groups[2].Value))
						.ToArray());

			var reverse = bagRules
				.SelectMany(
					kvp => kvp.Value,
					(kvp, c) => (from: kvp.Key, c.color))
				.ToLookup(x => x.color, x => x.from);

			var visited = new HashSet<string>();
			void visitReverse(string color)
			{
				if (visited.Contains(color))
					return;
				visited.Add(color);
				foreach (var c in reverse[color])
					visitReverse(c);
			}

			visitReverse("shiny gold");
			PartA = (visited.Count - 1).ToString();

			int bagTotal(string color) =>
				1 + bagRules[color].Sum(x => x.count * bagTotal(x.color));
			PartB = (bagTotal("shiny gold") - 1).ToString();
		}
	}
}
