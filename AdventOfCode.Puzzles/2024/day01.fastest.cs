using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 01, CodeType.Fastest)]
public partial class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<int> list1 = new int[input.Bytes.Length / 8];
		Span<int> list2 = new int[input.Bytes.Length / 8];

		var idx = 0;
		foreach (var line in input.Span[..^1].EnumerateLines())
		{
			int i = 0, n = 0;

			while (line[i] != ' ')
				n = (n * 10) + (line[i++] - '0');

			list1[idx] = n;

			i += 3;
			n = 0;
			while (i < line.Length)
				n = (n * 10) + (line[i++] - '0');

			list2[idx] = n;

			idx++;
		}

		list1 = list1[..idx];
		list2 = list2[..idx];

		list1.Sort();
		list2.Sort();

		var vec1 = MemoryMarshal.Cast<int, Vector256<int>>(list1);
		var vec2 = MemoryMarshal.Cast<int, Vector256<int>>(list2);

		var part1 = Vector256<int>.Zero;
		var part2 = Vector256<int>.Zero;
		var l1Idx = 0;
		for (var i = 0; i < vec1.Length && i < vec2.Length; i++)
		{
			var v2 = vec2[i];
			part1 += Vector256.Abs(vec1[i] - v2);

			var max = v2.GetElement(Vector256<int>.Count - 1);
			while (l1Idx < list1.Length && list1[l1Idx] <= max)
			{
				var v1 = Vector256.Create(list1[l1Idx]);
				part2 += Vector256.BitwiseAnd(v1, Vector256.Equals(v1, v2));

				if (list1[l1Idx] == max)
					break;

				l1Idx++;
			}
		}

		return (Vector256.Sum(part1).ToString(), Vector256.Sum(part2).ToString());
	}
}
