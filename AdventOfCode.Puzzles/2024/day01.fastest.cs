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

		var part1 = 0;
		var part2 = 0;
		var l2Idx = 0;
		for (var i = 0; i < idx; i++)
		{

			var l1 = list1[i];
			part1 += Math.Abs(l1 - list2[i]);

			while (l2Idx < list2.Length && list2[l2Idx] < l1)
				l2Idx++;

			while (l2Idx < list2.Length && list2[l2Idx] == l1)
			{
				l2Idx++;
				part2 += l1;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
