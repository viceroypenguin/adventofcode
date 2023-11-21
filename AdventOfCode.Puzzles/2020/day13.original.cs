using static AdventOfCode.Common.Extensions.NumberExtensions;

namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 13, CodeType.Original)]
public class Day_13_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var lines = input.Lines;
		var times = lines[1].Split(',');

		var myEarliestTime = int.Parse(lines[0]);
		var part1 = times
			.Where(s => s != "x")
			.Select(int.Parse)
			.Select(b => (bus: b, firstTimeAfter: (myEarliestTime / b + 1) * b - myEarliestTime))
			.PartialSortBy(1, x => x.firstTimeAfter)
			.Select(x => x.bus * x.firstTimeAfter)
			.First()
			.ToString();

		var earliestTime = long.Parse(times[0]);
		var increment = earliestTime;
		for (var i = 1; i < times.Length; i++)
		{
			if (times[i] == "x") continue;

			var curTime = long.Parse(times[i]);
			var modValue = curTime - (i % curTime);
			while (earliestTime % curTime != modValue)
				earliestTime += increment;
			increment = Lcm(increment, curTime);
		}

		var part2 = earliestTime.ToString();

		return (part1, part2);
	}
}
