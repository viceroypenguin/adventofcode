namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 6, CodeType.Fastest)]
public class Day_06_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<long> fish = stackalloc long[9];

		var span = input.Span;
		for (var i = 0; i < span.Length;)
		{
			var (value, numChars) = span[i..].AtoI();
			fish[value]++;
			i += numChars + 1;
		}

		// comment and idea copied from @ensce (https://www.reddit.com/r/adventofcode/comments/r9z49j/2021_day_6_solutions/hnfhi24/)
		// we will model a circular shift register, with an additional feedback:
		//       0123456           78 
		//   ┌──[       ]─<─(+)───[  ]──┐
		//   └──────>────────┴─────>────┘

		for (var i = 0; i < 80; i++)
			fish[(i + 7) % 9] += fish[i % 9];

		var totalFish = 0L;
		foreach (var f in fish)
			totalFish += f;
		var part1 = totalFish.ToString();

		for (var i = 80; i < 256; i++)
			fish[(i + 7) % 9] += fish[i % 9];

		totalFish = 0L;
		foreach (var f in fish)
			totalFish += f;
		var part2 = totalFish.ToString();

		return (part1, part2);
	}
}
