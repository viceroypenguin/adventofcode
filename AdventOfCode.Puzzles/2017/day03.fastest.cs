namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 03, CodeType.Fastest)]
public class Day_03_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day03.c
		var bytes = input.Bytes;

		var number = 0;
		for (var i = 0; i < bytes.Length; i++)
		{
			if (bytes[i] >= '0')
				number = (number * 10) + bytes[i] - '0';
		}

		var x = number - 1;
		var ring = (int)(Math.Sqrt((uint)x) + 1) / 2;
		var partA = ring + Math.Abs((x % (ring * 2)) - ring);

		return (partA.ToString(), DoPartB(number).ToString());
	}

	private static long DoPartB(int number)
	{
		var arr = new long[512];
		arr[0] = 1; arr[1] = 1; arr[2] = 1;
		arr[3] = 1; arr[4] = 1; arr[5] = 1;
		arr[6] = 1; arr[7] = 2; arr[8] = 2;

		long p = 1, q = 9;
		for (var length = 2; ; length += 2)
		{
			for (var side = 0; side < 4; side++)
			{
				for (var i = length; i-- > 0;)
				{
					if (arr[p] > number)
					{
						return arr[p];
					}

					arr[q++] += arr[p];
					arr[q] += arr[p];
					arr[p + 1] += arr[p];
					arr[q + 1] += arr[p++];
				}

				if (side == 3) break;

				// Turn the corner
				arr[(++q) + 1] += arr[p - 1];
				arr[(++q) + 1] += arr[p - 1];
				arr[p] += arr[p - 2];

			}

			// Advance to the next ring
			arr[q++] += arr[p];
			arr[q++] += arr[p];
			arr[p + 1] += arr[p - 1];
		}
	}
}
