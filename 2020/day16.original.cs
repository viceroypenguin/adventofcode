using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using MoreLinq;
using static AdventOfCode.Helpers;

namespace AdventOfCode
{
	public class Day_2020_16_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 16;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var segments = input.GetLines(StringSplitOptions.None)
				.Segment(s => string.IsNullOrWhiteSpace(s))
				.ToArray();

			var rulesDescriptions = segments[0];
			var myTicket = segments[1].Last().Split(',').Select(int.Parse).ToArray();
			var otherTickets = segments[2].Skip(2).Select(l => l.Split(',').Select(int.Parse).ToArray()).ToArray();

			var rules = rulesDescriptions
				.Select(l => (
					name: l[..l.IndexOf(':')],
					values: Regex.Matches(l, @"(\d+)-(\d+)").OfType<Match>()
						.Select(m => (lo: int.Parse(m.Groups[1].Value), hi: int.Parse(m.Groups[2].Value)))
						.ToArray()))
				.ToArray();

			var validNumbers = rules.SelectMany(x => x.values).ToArray();
			PartA = otherTickets
				.SelectMany(t => t)
				.Where(n => !validNumbers.Any(x => n.Between(x.lo, x.hi)))
				.Sum()
				.ToString();

			var validTickets = otherTickets
				.Where(t => t.All(n => validNumbers.Any(x => n.Between(x.lo, x.hi))))
				.ToArray();

			var candidates = rules
				.Select(r => (
					r.name,
					indexes: Enumerable.Range(0, validTickets[0].Length)
						.Where(i => validTickets.All(t => r.values.Any(v => t[i].Between(v.lo, v.hi))))
						.ToList()))
				.ToList();

			var map = new List<(string name, int index)>();
			while (map.Count < rules.Length)
			{
				var nextCandidate = candidates.First(c => c.indexes.Count == 1);
				var index = nextCandidate.indexes[0];
				map.Add((nextCandidate.name, index));
				candidates.RemoveAll(c => c.name == nextCandidate.name);
				foreach (var (_, indexes) in candidates)
					indexes.Remove(index);
			}

			PartB = map
				.Where(r => r.name.StartsWith("departure"))
				.Select(r => myTicket[r.index])
				.Aggregate(1L, (l, r) => l * r)
				.ToString();
		}
	}
}
