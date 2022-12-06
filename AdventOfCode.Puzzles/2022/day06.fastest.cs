using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 6, CodeType.Fastest)]
public partial class Day_06_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input) =>
		(
			GetIndex(input.Bytes, 4).ToString(),
			GetIndex(input.Bytes, 14).ToString());

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static int GetIndex(ReadOnlySpan<byte> bytes, int numDistinct)
	{
		for (int i = numDistinct - 1; i < bytes.Length; i++)
		{
			for (int j = i - numDistinct + 1; j <= i; j++)
			{
				for (int k = j + 1; k <= i; k++)
				{
					if (bytes[j] == bytes[k])
						goto next;
				}
			}

			// if we make it through without goto, then we're done
			return i + 1;

next:
			;
		}

		return bytes.Length;
	}
}
