using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 18, CodeType.Fastest)]
public sealed partial class Day_18_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		long ap1 = 0, dp1 = 0, ap2 = 0, dp2 = 0;
		(long x, long y) pos1 = (0, 0), pos2 = (0, 0);

		foreach (var l in input.Span.EnumerateLines())
		{
			if (l.Length == 0)
				break;

			var num = l[2] - '0';
			if (l[3] != ' ')
				num = (num * 10) + l[3] - '0';

			var q = l[0] switch
			{
				(byte)'U' => (x: pos1.x, y: pos1.y - num),
				(byte)'D' => (x: pos1.x, y: pos1.y + num),
				(byte)'L' => (x: pos1.x - num, y: pos1.y),
				(byte)'R' => (x: pos1.x + num, y: pos1.y),
				_ => throw new UnreachableException(),
			};

			ap1 += (pos1.x * q.y) - (q.x * pos1.y);
			dp1 += num;
			pos1 = q;

			var span = l[(num >= 10 ? 7 : 6)..];

			num = 0;
			for (var i = 0; i < 5; i++)
				num = (num * 16) + AtoI16(span[i]);

			static int AtoI16(byte b) =>
				(b & 0xF) + (9 * (b >> 6));

			q = span[5] switch
			{
				(byte)'0' => (x: pos2.x + num, y: pos2.y),
				(byte)'1' => (x: pos2.x, y: pos2.y + num),
				(byte)'2' => (x: pos2.x - num, y: pos2.y),
				(byte)'3' => (x: pos2.x, y: pos2.y - num),
				_ => throw new UnreachableException(),
			};

			ap2 += (pos2.x * q.y) - (q.x * pos2.y);
			dp2 += num;
			pos2 = q;
		}

		var part1 = Math.Abs(ap1 / 2) + (dp1 / 2) + 1;
		var part2 = Math.Abs(ap2 / 2) + (dp2 / 2) + 1;

		return (part1.ToString(), part2.ToString());
	}
}
