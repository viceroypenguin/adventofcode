namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 08, CodeType.Original)]
public partial class Day_08_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{

		var steps = input.Lines[0].ToCharArray();

		var regex = new Regex(@"^(?<from>\w+) = \((?<to_l>\w+), (?<to_r>\w+)\)$");
		var instructions = input.Lines.Skip(2)
			.Select(l => regex.Match(l))
			.ToDictionary(
				m => m.Groups["from"].Value,
				m => new { Left = m.Groups["to_l"].Value, Right = m.Groups["to_r"].Value });

		var part1 = steps.Repeat()
					.Scan("AAA", (s, i) => i == 'L' ? instructions[s].Left : instructions[s].Right)
					.TakeUntil(x => x == "ZZZ")
					.Count() - 1;

		var points = instructions.Keys.Where(x => x.EndsWith('A')).ToList();
		var doublePoints = points.ToList();

		var cycleCounts = new List<int>(points.Count);
		var count = 0;

		foreach (var i in steps.Repeat())
		{
			List<string> ProcessStep(List<string> points) =>
				points.Select(p => i == 'L' ? instructions[p].Left : instructions[p].Right).ToList();

			var newPoints = new List<string>(points.Count);
			foreach (var p in points)
			{
				newPoints.Add(i == 'L' ? instructions[p].Left : instructions[p].Right);
			}

			points = ProcessStep(points);
			doublePoints = ProcessStep(doublePoints);
			count++;

			for (var idx = 0; idx < points.Count; idx++)
			{
				if (points[idx].EndsWith('Z') && points[idx] == doublePoints[idx])
				{
					cycleCounts.Add(count);
					points.RemoveAt(idx);
					doublePoints.RemoveAt(idx);
				}
			}

			if (points.Count == 0)
				break;
		}

		var part2 = cycleCounts.Aggregate(1L, (a, b) => NumberExtensions.Lcm(a, b));

		return (part1.ToString(), part2.ToString());
	}
}
