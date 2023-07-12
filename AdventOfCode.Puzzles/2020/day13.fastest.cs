using static AdventOfCode.Common.Helpers;

namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 13, CodeType.Fastest)]
public class Day_13_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;
		var (myTime, i) = span.AtoI();
		i++;

		var minTimeAfter = (id: 0, timeAfter: int.MaxValue);
		var busNumber = -1;
		long time = 1, increment = 1;
		while (i < span.Length)
		{
			busNumber++;
			if (span[i] == 'x')
			{
				i += 2;
				continue;
			}

			var (id, x) = span[i..].AtoI();
			i += x + 1;

			var valueAfter = id - (myTime % id);
			if (valueAfter < minTimeAfter.timeAfter)
				minTimeAfter = (id, valueAfter);

			if (busNumber == 0)
			{
				time = increment = id;
				continue;
			}

			var modValue = id - (busNumber % id);
			while (time % id != modValue)
				time += increment;
			increment = Lcm(increment, id);
		}

		var part1 = (minTimeAfter.id * minTimeAfter.timeAfter).ToString();
		var part2 = time.ToString();
		return (part1, part2);
	}
}
