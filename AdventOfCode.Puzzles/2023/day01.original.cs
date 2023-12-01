using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 01, CodeType.Original)]
public partial class Day_01_Original : IPuzzle
{
	[GeneratedRegex("\\d")]
	private static partial Regex DigitRegex();

	[GeneratedRegex("\\d|one|two|three|four|five|six|seven|eight|nine")]
	private static partial Regex DigitStringRegexLeft();

	[GeneratedRegex("\\d|one|two|three|four|five|six|seven|eight|nine", RegexOptions.RightToLeft)]
	private static partial Regex DigitStringRegexRight();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = DigitRegex();

		var part1 = input.Lines
			.Select(l => regex.Matches(l))
			.Select(m =>
				(int.Parse(m.First().Value) * 10)
				+ int.Parse(m.Last().Value))
			.Sum()
			.ToString();

		var regex1 = DigitStringRegexLeft();
		var regex2 = DigitStringRegexRight();

		static int ParseNumber(string s) =>
			char.IsDigit(s[0]) ? s[0] - '0' :
			s switch
			{
				"one" => 1,
				"two" => 2,
				"three" => 3,
				"four" => 4,
				"five" => 5,
				"six" => 6,
				"seven" => 7,
				"eight" => 8,
				"nine" => 9,
				_ => throw new UnreachableException(),
			};

		var part2 = input.Lines
			.Select(l =>
				(ParseNumber(DigitStringRegexLeft().Match(l).Value) * 10)
				+ ParseNumber(DigitStringRegexRight().Match(l).Value))
			.Sum()
			.ToString();

		return (part1, part2);
	}
}
