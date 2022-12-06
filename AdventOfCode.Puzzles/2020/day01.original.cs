using System.Collections;

namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var numbers = input.Lines
			.Select(int.Parse)
			.ToArray();

		var part1 = string.Empty;
		var bitArray = new BitArray(2020);
		foreach (var n in numbers)
		{
			if (bitArray[2020 - n])
				part1 = ((2020 - n) * n).ToString();
			bitArray[n] = true;
		}

		for (var xi = 0; ; xi++)
		{
			var x = numbers[xi];
			for (var yi = xi + 1; yi < numbers.Length; yi++)
			{
				var y = numbers[yi];
				var z = 2020 - x - y;
				if (z >= 0 && bitArray[z])
				{
					var part2 = (x * y * z).ToString();
					return (part1, part2);
				}
			}
		}
	}
}
