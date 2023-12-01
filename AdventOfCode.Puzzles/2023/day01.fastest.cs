using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 01, CodeType.Fastest)]
public partial class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;
		var part2 = 0;
		foreach (var l in input.Text.AsSpan().EnumerateLines())
		{
			if (l.Length == 0)
				continue;

			{
				var tens = (l[l.IndexOfAnyInRange('0', '9')] - '0') * 10;
				var ones = l[l.LastIndexOfAnyInRange('0', '9')] - '0';
				part1 += tens + ones;
			}

			{
				var span = MemoryMarshal.CreateSpan(
					ref MemoryMarshal.GetReference(l),
					l.Length);

				static void Replace(Span<char> span, ReadOnlySpan<char> oldValue, char newValue)
				{
					while (true)
					{
						var index = span.IndexOf(oldValue);
						if (index < 0)
							return;
						span[index + (oldValue.Length / 2)] = newValue;
						span = span[(index + 1)..];
					}
				}

				Replace(span, "one", '1');
				Replace(span, "two", '2');
				Replace(span, "three", '3');
				Replace(span, "four", '4');
				Replace(span, "five", '5');
				Replace(span, "six", '6');
				Replace(span, "seven", '7');
				Replace(span, "eight", '8');
				Replace(span, "nine", '9');

				var tens = (l[l.IndexOfAnyInRange('0', '9')] - '0') * 10;
				var ones = l[l.LastIndexOfAnyInRange('0', '9')] - '0';
				part2 += tens + ones;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
