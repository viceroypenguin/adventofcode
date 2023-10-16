namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 12, CodeType.Fastest)]
public class Day_12_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day12.c
		var span = input.Span;

		Span<int> parents = stackalloc int[span.Length / 16];
		Span<int> rowNumbers = stackalloc int[16];
		int entryCount = 0, rowCount = 0, n = 0;

		foreach (var c in span)
		{
			if (c is >= (byte)'0' and <= (byte)'9')
			{
				n = (n * 10) + c - '0';
			}
			else if (c is (byte)',' or (byte)'<')
			{
				// 0 is our key value, so shift everything up one
				rowNumbers[rowCount++] = n + 1;
				n = 0;
			}
			else if (c == '\n')
			{
				// 0 is our key value, so shift everything up one
				rowNumbers[rowCount++] = n + 1;
				n = 0;

				var min = int.MaxValue;
				for (var i = 0; i < rowCount; i++)
				{
					if (rowNumbers[i] < min)
						min = rowNumbers[i];
				}

				var parent = min;
				var tmp = rowCount;
				for (var i = 0; i < tmp; i++)
				{
					n = parents[rowNumbers[i]];
					if (n != 0)
					{
						while (parents[n] != 0 && parents[n] != n)
							n = parents[n];
						// reset the root of each tree as well
						rowNumbers[rowCount++] = n;

						if (n < parent)
							parent = n;
					}
				}

				for (var i = 0; i < rowCount; i++)
					parents[rowNumbers[i]] = parent;
				entryCount++;
				rowCount = 0;
				n = 0;
			}
		}

		int numGroups = 0, zeroGroupSize = 1;
		Span<int> zeroGroup = stackalloc int[entryCount];
		zeroGroup[0] = 1;
		for (var i = 1; i < entryCount; i++)
		{
			if (parents[i] == i)
			{
				numGroups++;
			}
			else
			{
				for (var j = zeroGroupSize - 1; j >= 0; j--)
				{
					if (zeroGroup[j] == parents[i])
					{
						zeroGroup[zeroGroupSize++] = i;
						break;
					}
				}
			}
		}

		return (
			zeroGroupSize.ToString(),
			numGroups.ToString());
	}
}
