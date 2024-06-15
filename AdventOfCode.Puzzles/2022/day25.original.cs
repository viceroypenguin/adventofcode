using System.Diagnostics;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 25, CodeType.Original)]
public class Day_25_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		static long ParseSnafu(string snafu)
		{
			var value = 0L;
			var scale = 1L;
			foreach (var c in snafu.Reverse())
			{
				var n = c switch
				{
					'2' => 2,
					'1' => 1,
					'0' => 0,
					'-' => -1,
					'=' => -2,
					_ => throw new UnreachableException(),
				};

				value += scale * n;
				scale *= 5;
			}

			return value;
		}

		static string ToSnafu(long value)
		{
			var digits = new char[32];
			var i = 0;
			for (; value != 0; i++)
			{
				var rem = (int)(value % 5);
				value /= 5;

				if (rem == 3)
				{
					digits[i] = '=';
					value += 1;
				}
				else if (rem == 4)
				{
					digits[i] = '-';
					value += 1;
				}
				else
				{
					digits[i] = (char)(rem + '0');
				}
			}

			return string.Join("", digits.Reverse().Where(d => d != 0));
		}

		var sum = input.Lines
			.Select(ParseSnafu)
			.Sum();
		var part1 = ToSnafu(sum);
		var part2 = string.Empty;
		return (part1, part2);
	}
}
