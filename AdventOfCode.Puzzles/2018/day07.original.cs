namespace AdventOfCode.Puzzles._2018;

#pragma warning disable IDE0305 // Simplify collection initialization

[Puzzle(2018, 07, CodeType.Original)]
public class Day_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var steps = input.Lines
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
		while (queue.Count != 0)
		{
			var step = queue.First();
			if (!visitedSteps.Contains(step.after))
			{
				orderedSteps.Add(step.after);
				_ = visitedSteps.Add(step.after);
			}

			_ = queue.Remove(step);
			if (befores[step.after].Any())
			{
				queue = queue
					.Concat(befores[step.after]
						.Where(x => afters[x.after]
							.All(y => visitedSteps.Contains(y.before))))
					.OrderBy(x => x.after)
					.ToList();
			}
		}

		var part1 = string.Join("", orderedSteps);

		var workers = 5;
		var stepTime = 60;

		var time = 0;
		var timedSteps = new SortedList<string, int>();

		visitedSteps = [string.Empty,];
		queue = befores
			.Where(s => !afters.Any(a => a.Key == s.Key))
			.Select(s => (before: string.Empty, after: s.Key))
			.OrderBy(x => x.after)
			.ToList();
		while (queue.Count != 0 || timedSteps.Count != 0)
		{
			while (workers > 0 && queue.Count != 0)
			{
				var step = queue.First();
				if (!visitedSteps.Contains(step.after))
				{
					timedSteps.Add(step.after, stepTime + step.after[0] - 'A' + 1);
					workers--;
				}
				_ = queue.Remove(step);
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
				_ = timedSteps.Remove(s.Key);
				_ = visitedSteps.Add(s.Key);
				if (befores[s.Key].Any())
				{
					queue = queue
						.Concat(befores[s.Key]
							.Where(x => afters[x.after]
								.All(y => visitedSteps.Contains(y.before))))
						.OrderBy(x => x.after)
						.ToList();
				}
			}
		}

		var part2 = time.ToString();
		return (part1, part2);
	}
}
