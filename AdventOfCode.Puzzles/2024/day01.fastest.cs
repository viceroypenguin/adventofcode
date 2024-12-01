namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 01, CodeType.Fastest)]
public partial class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<int> list1 = stackalloc int[input.Bytes.Length / 8];
		Span<int> list2 = stackalloc int[input.Bytes.Length / 8];

		var span = input.Span;
		var idx = 0;
		while (span.Length > 0)
		{
			(list1[idx], var n) = span.AtoI();
			span = span[n..];
			n = span.IndexOfAnyExcept((byte)' ');
			span = span[n..];
			(list2[idx], n) = span.AtoI();
			span = span[(n + 1)..];
			idx++;
		}

		list1 = list1[..idx];
		list2 = list2[..idx];

		list1.Sort();
		list2.Sort();

		var part1 = 0;
		var part2 = 0;
		var l2Idx = 0;
		for (var i = 0; i < idx; i++)
		{
			part1 += Math.Abs(list1[i] - list2[i]);

			while (l2Idx < list2.Length && list2[l2Idx] < list1[i])
				l2Idx++;

			var n = 0;
			while (l2Idx < list2.Length && list2[l2Idx] == list1[i])
			{
				l2Idx++;
				n++;
			}

			part2 += list1[i] * n;
		}

		return (part1.ToString(), part2.ToString());
	}
}
