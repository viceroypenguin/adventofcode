namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 17, CodeType.Fastest)]
public class Day_17_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day17.c
		var span = input.Span;

		var key = 0;
		for (var i = 0; i < span.Length && span[i] >= '0'; i++)
			key = (key * 10) + span[i] - '0';

		var position = 0;
		// Find the index of the 2017th insertion
		for (var i = 1; i <= 2017; i++)
			position = ((position + key) % i) + 1;

		// Reverse the simulation to find the value which follows
		int nextValue, nextPosition = (position + 1) % 2017;
		for (nextValue = 2017; position != nextPosition; nextValue--)
		{
			if (position < nextPosition)
				nextPosition--;
			if ((position -= key + 1) < 0)
				position += nextValue;
		}

		var partA = nextValue;

		var position1 = position = 0;
		// loop runs in O(log i)
		for (var i = 0; i < 50_000_000; position++)
		{
			if (position == 1)
				position1 = i;

			// use n * (n + 1) concept to skip processing every item in loop
			var skip = ((i - position) / key) + 1;
			position += (skip * (key + 1)) - 1;
			position %= i += skip;
		}

		var partB = position1;

		return (partA.ToString(), partB.ToString());
	}
}
