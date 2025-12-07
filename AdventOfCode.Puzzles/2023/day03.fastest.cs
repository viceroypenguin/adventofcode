using System.Buffers;
using CommunityToolkit.HighPerformance;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 03, CodeType.Fastest)]
public sealed partial class Day_03_Fastest : IPuzzle
{
	private static readonly (int x, int y)[] s_adjacent =
	[
		(0, 1),
		(1, 0),
		(0, -1),
		(-1, 0),
		(-1, -1),
		(1, 1),
		(-1, 1),
		(1, -1),
	];

	private static readonly SearchValues<byte> s_symbols = SearchValues.Create("0123456789."u8);

	public (string, string) Solve(PuzzleInput input)
	{
		var stride = input.Span.IndexOf((byte)'\n');

		var part1 = 0;
		var part2 = 0;

		var mutableMap = new ReadOnlySpan2D<byte>(input.Bytes, 0, input.Bytes.Length / (stride + 1), stride, 1);
		for (var y = 0; y < mutableMap.Height; y++)
		{
			var line = mutableMap.GetRowSpan(y);

			for (var x = 0; x < line.Length; x++)
			{
				var advance = line[x..].IndexOfAnyExcept(s_symbols);
				if (advance == -1)
					break;

				x += advance;

				var number1 = 0;
				foreach (var (dx, dy) in s_adjacent)
				{
					if (y + dy < 0 || y + dy >= mutableMap.Height)
						continue;
					if (x + dx < 0 || x + dx >= mutableMap.Width)
						continue;

					if (!mutableMap[y + dy, x + dx].Between((byte)'0', (byte)'9'))
					{
						continue;
					}

					var number = SweepAndReplace(mutableMap, x + dx, y + dy);
					if (mutableMap[y, x] != '*')
					{
						part1 += number;
						break;
					}

					if (number1 == 0)
					{
						part1 += number;
						number1 = number;
					}
					else if (number1 != number)
					{
						part1 += number;
						part2 += number1 * number;
						break;
					}
				}
			}
		}

		return (part1.ToString(), part2.ToString());
	}

	private static int SweepAndReplace(ReadOnlySpan2D<byte> mutableMap, int x, int y)
	{
		var xStart = x;
		while (xStart > 0 && mutableMap[y, xStart - 1].Between((byte)'0', (byte)'9'))
		{
			xStart--;
		}

		var xEnd = x;
		while (xEnd < mutableMap.Width - 1 && mutableMap[y, xEnd + 1].Between((byte)'0', (byte)'9'))
		{
			xEnd++;
		}

		var number = mutableMap.GetRowSpan(y).Slice(xStart, xEnd - xStart + 1);
		var (output, _) = number.AtoI();
		return output;
	}
}
