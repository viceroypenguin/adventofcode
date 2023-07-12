using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 04, CodeType.Fastest)]
public class Day_04_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var bytes = input.Bytes;

		var line = new ulong[32];
		var number = 0ul;
		var cnt = (uint)bytes.Length;
		int part1 = 0, part2 = 0;
		bool flag1 = false, flag2 = false;
		for (int i = 0, j = 0; i < cnt; i++)
		{
			var c = bytes[i];
			if (c >= 'a')
			{
				number = (number << 8) + c;
			}
			else
			{
				var sortedNumber = SortBytes(number);
				for (var k = 0; !flag1 && k < j; k += 2)
				{
					if (line[k] == number)
						flag1 = true;
				}

				for (var k = 1; !flag2 && k < j; k += 2)
				{
					if (line[k] == sortedNumber)
						flag2 = true;
				}

				if (c == '\n')
				{
					if (!flag1) part1++;
					if (!flag2) part2++;
					flag1 = false;
					flag2 = false;
					j = 0;
					number = 0;
				}
				else
				{
					line[j++] = number;
					line[j++] = sortedNumber;
					number = 0;
				}
			}
		}

		return (
			part1.ToString(),
			part2.ToString());
	}

	private static ulong SortBytes(ulong bytes)
	{
		var arr = MemoryMarshal.AsBytes(
			MemoryMarshal.CreateSpan(ref bytes, 1));
		for (var i = 0; i < 7; i++)
		{
			for (var j = i + 1; j < 8; j++)
			{
				if (arr[j] < arr[i])
					(arr[i], arr[j]) = (arr[j], arr[i]);
			}
		}

		return bytes;
	}
}
