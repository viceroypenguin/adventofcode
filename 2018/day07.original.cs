using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2018_07_Original : Day
	{
		public override int Year => 2018;
		public override int DayNumber => 7;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var steps = input.GetLines()
				.Select(l => l.Split())
				.Select(l => (before: l[1], after: l[7]))
				.ToList();

			var befores = steps
				.ToLookup(x => x.before);
			var afters = steps
				.ToLookup(x => x.after);

			var visitedSteps = new HashSet<string>() { string.Empty, };

			var orderedSteps = new List<string> { string.Empty };
			var queue = befores
				.Where(s => !afters.Any(a => a.Key == s.Key))
				.Select(s => (before: string.Empty, after: s.Key))
				.OrderBy(x => x.after)
				.ToList();
			while (queue.Any())
			{
				var step = queue.First();
				if (!visitedSteps.Contains(step.after))
				{
					orderedSteps.Add(step.after);
					visitedSteps.Add(step.after);
				}

				queue.Remove(step);
				if (befores[step.after].Any())
					queue = queue
						.Concat(befores[step.after]
							.Where(x => afters[x.after]
								.All(y => visitedSteps.Contains(y.before))))
						.OrderBy(x => x.after)
						.ToList();
			}

			Dump('A', string.Join("", orderedSteps));

			var workers = 5;
			var stepTime = 60;

			var time = 0;
			var timedSteps = new SortedList<string, int>();

			visitedSteps = new HashSet<string> { string.Empty, };
			queue = befores
				.Where(s => !afters.Any(a => a.Key == s.Key))
				.Select(s => (before: string.Empty, after: s.Key))
				.OrderBy(x => x.after)
				.ToList();
			while (queue.Any() || timedSteps.Any())
			{
				while (workers > 0 && queue.Any())
				{
					var step = queue.First();
					if (!visitedSteps.Contains(step.after))
					{
						timedSteps.Add(step.after, stepTime + ((int)step.after[0] - (int)'A' + 1));
						workers--;
					}
					queue.Remove(step);
				}

				var nextTime = timedSteps
					.Min(x => x.Value);
				timedSteps.Keys.ToArray()
					.ForEach(x => timedSteps[x] -= nextTime);
				time += nextTime;

				foreach (var s in timedSteps
					.Where(x => x.Value == 0)
					.ToArray())
				{
					workers++;
					timedSteps.Remove(s.Key);
					visitedSteps.Add(s.Key);
					if (befores[s.Key].Any())
						queue = queue
							.Concat(befores[s.Key]
								.Where(x => afters[x.after]
									.All(y => visitedSteps.Contains(y.before))))
							.OrderBy(x => x.after)
							.ToList();
				}
			}

			Dump('B', time);
		}
	}
}
