namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 06, CodeType.Original)]
public partial class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var actions = input.Lines[^1].Index()
			.Where(x => x.Item != ' ')
			.ToList();

		var part1 = actions.Select(x => x.Item)
			.EquiZip(
				input.Lines.SkipLast(1)
					.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
						.Select(long.Parse)
						.ToList()
					)
					.Transpose(),
				(action, numbers) => action switch
				{
					'*' => numbers.Aggregate(1L, (a, b) => a * b),
					_ => numbers.Sum(),
				}
			)
			.Sum();

		var indices = actions.Select(x => x.Index - 1).ToHashSet();

		var part2 = actions.Select(x => x.Item)
			.EquiZip(
				input.Lines.SkipLast(1)
					.Select(l => l.Segment((_, i) => indices.Contains(i)))
					.Transpose()
					.Select(
						x => x.Transpose()
							.Where(x => !x.All(c => c == ' '))
							.Select(x => long.Parse(new string(x.ToArray())))
					),
				(action, numbers) => action switch
				{
					'*' => numbers.Aggregate(1L, (a, b) => a * b),
					_ => numbers.Sum(),
				}
			)
			.Sum();

		return (part1.ToString(), part2.ToString());
	}
}
