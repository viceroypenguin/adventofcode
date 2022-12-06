using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 3, CodeType.Fastest)]
public class Day_03_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		static long GetTreesOnSlope(byte[] input, int vx, int vy)
		{
			vx *= 32;

			var count = 0;
			for (int x = 0, y = 0; x < input.Length; x += vx, y += vy)
				count += input[x + (y % 31)] == '#' ? 1 : 0;
			return count;
		}

		var part1 = GetTreesOnSlope(input.Bytes, 1, 3).ToString();

		var part2 = (
			GetTreesOnSlope(input.Bytes, 1, 1) *
			GetTreesOnSlope(input.Bytes, 1, 3) *
			GetTreesOnSlope(input.Bytes, 1, 5) *
			GetTreesOnSlope(input.Bytes, 1, 7) *
			GetTreesOnSlope(input.Bytes, 2, 1)).ToString();

		return (part1, part2);
	}
}
