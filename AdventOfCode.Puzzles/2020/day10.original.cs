namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 10, CodeType.Original)]
public class Day_10_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var numbers = input.Lines
			.Select(int.Parse)
			.OrderBy(x => x)
			.ToArray();

		var differences = numbers
			.Prepend(0)
			.Append(numbers[^1] + 3)
			.Window(2)
			.Select(x => x[1] - x[0])
			.ToArray();

		var counts = differences
			.GroupBy(x => x, (d, _) => (diff: d, count: _.Count()))
			.ToArray();

		var num1 = counts.Single(x => x.diff == 1).count;
		var num3 = counts.Single(x => x.diff == 3).count;

		var part1 = (num1 * num3).ToString();

		var sequences = differences
			.Segment((cur, prev, _) => cur != prev)
			.Where(x => x[0] == 1)
			.Select(x => x.Count switch
			{
				1 => 1,
				2 => 2,
				3 => 4,
				4 => 7,
				5 => 15,
				_ => throw new InvalidOperationException("??"),
			})
			.Aggregate(1L, (agg, x) => agg * x);

		var part2 = sequences.ToString();

		return (part1, part2);
	}
}
