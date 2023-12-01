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
				var tens = -1;
				var ones = -1;

				for (var i = 0; i < l.Length; i++)
				{
					ones = l[i] switch
					{
						var c when c is >= '0' and <= '9' => c - '0',
						'o' when i + 3 <= l.Length && l[i..(i + 3)] is "one" => 1,
						't' when i + 3 <= l.Length && l[i..(i + 3)] is "two" => 2,
						't' when i + 5 <= l.Length && l[i..(i + 5)] is "three" => 3,
						'f' when i + 4 <= l.Length && l[i..(i + 4)] is "four" => 4,
						'f' when i + 4 <= l.Length && l[i..(i + 4)] is "five" => 5,
						's' when i + 3 <= l.Length && l[i..(i + 3)] is "six" => 6,
						's' when i + 5 <= l.Length && l[i..(i + 5)] is "seven" => 7,
						'e' when i + 5 <= l.Length && l[i..(i + 5)] is "eight" => 8,
						'n' when i + 4 <= l.Length && l[i..(i + 4)] is "nine" => 9,
						_ => ones,
					};

					if (ones != -1 && tens == -1)
						tens = ones * 10;
				}

				part2 += tens + ones;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
