namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 06, CodeType.Original)]
public partial class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var row1 = input.Lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
		var row2 = input.Lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
		var row3 = input.Lines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
		var row4 = input.Lines[3].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
		var row5 = input.Lines[4].Split(' ', StringSplitOptions.RemoveEmptyEntries);

		var part1 = row5
			.EquiZip(row1.EquiZip(row2, row3, row4))
			.Sum(x => x.Item1 switch
			{
				"*" => x.Item2.Item1 * x.Item2.Item2 * x.Item2.Item3 * x.Item2.Item4,
				_ => x.Item2.Item1 + x.Item2.Item2 + x.Item2.Item3 + x.Item2.Item4,
			});

		var rows = input.Lines;

		var part2 = 0L;

		var action = '+';
		var startColumn = 0;

		for (var i = 0; i <= rows[0].Length; i++)
		{
			if (i == rows[0].Length || rows.All(l => l[i] == ' '))
			{
				var numbers = new long[i - startColumn];

				for (var j = 0; j < numbers.Length; j++)
				{
					var number = 0L;

					for (var k = 0; k < rows.Length - 1; k++)
					{
						if (rows[k][j + startColumn] != ' ')
							number = (number * 10) + rows[k][j + startColumn] - '0';
					}

					numbers[j] = number;
				}

				part2 += action switch
				{
					'*' => numbers.Aggregate(1L, (a, b) => a * b),
					_ => numbers.Sum(),
				};

				continue;
			}

			if (rows[^1][i] != ' ')
			{
				action = rows[^1][i];
				startColumn = i;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
