using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 07, CodeType.Fastest)]
public partial class Day_07_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;

		Span<long> beams = stackalloc long[160];

		var min = input.Lines[0].IndexOf('S');
		var max = min;
		beams[min] = 1;

		var lines = input.Lines;
		for (var row = 2; row < lines.Length; row += 2)
		{
			var l = lines[row];

			for (var i = min; i <= max; i++)
			{
				if (beams[i] > 0 && l[i] is '^')
				{
					part1++;

					var num = beams[i];
					beams[i] = 0;
					beams[i - 1] += num;
					beams[i + 1] += num;

					if (i == min)
						min--;

					if (i == max)
						max++;
				}
			}
		}

		var vectors = MemoryMarshal.Cast<long, Vector256<long>>(beams);
		var part2 = Vector256<long>.Zero;
		foreach (var v in vectors)
			part2 += v;

		return (part1.ToString(), Vector256.Sum(part2).ToString());
	}
}
