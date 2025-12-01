namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var value = 50;
		var part1 = 0;
		var part2 = 0;

		foreach (var l in input.Lines)
		{
			var num = int.Parse(l.AsSpan()[1..]);

			(value, part2) = l[0] switch
			{
				'R' => ((value + num) % 100, part2 + ((value + num) / 100)),
				_ => ((value - num + 10_000) % 100, part2 + ((num - value) / 100) + (value > 0 && num >= value ? 1 : 0)),
			};

#pragma warning disable CA1508 // Avoid dead conditional code
			if (value == 0)
				part1++;
#pragma warning restore CA1508 // Avoid dead conditional code
		}

		return (part1.ToString(), part2.ToString());
	}
}
