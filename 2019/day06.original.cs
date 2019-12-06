using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2019_06_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 6;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var directOrbits = input.GetLines()
				.Select(s => s.Split(')'))
				.Select(s => (orbiter: s[1], orbited: s[0]))
				.ToList();

			var lookup1 = directOrbits.ToDictionary(s => s.orbiter);
			var lookup2 = directOrbits.ToLookup(s => s.orbited);

			PartA = directOrbits
				.Select(s =>
				{
					var count = 1;
					while (true)
					{
						if (!lookup1.TryGetValue(s.orbited, out var o))
							return count;
						count++;
						s = o;
					}
				})
				.Sum()
				.ToString();

			var visited = new Dictionary<string, int>();
			var queue = new Queue<(string, int)>();
			queue.Enqueue((lookup1["YOU"].orbited, 0));
			while (true)
			{
				var o = queue.Dequeue();
				if (o.Item1 == "SAN")
				{
					PartB = (o.Item2 - 1).ToString();
					return;
				}

				if (visited.ContainsKey(o.Item1))
					continue;

				visited[o.Item1] = o.Item2;

				if (lookup1.TryGetValue(o.Item1, out var x))
					queue.Enqueue((x.orbited, o.Item2 + 1));
				foreach (var (orbiter, _) in lookup2[o.Item1])
					queue.Enqueue((orbiter, o.Item2 + 1));
			}
		}
	}
}
