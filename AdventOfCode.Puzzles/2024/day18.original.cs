namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 18, CodeType.Original)]
public partial class Day_18_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var regex = new Regex(@"(\d+),(\d+)");
		var errors = input.Lines
			.Select(l => regex.Match(l))
			.Select(m =>
				(
					x: int.Parse(m.Groups[1].Value),
					y: int.Parse(m.Groups[2].Value)
				)
			)
			.ToList();

		var firstKilo = errors.Take(1024).ToHashSet();

		var part1 = SuperEnumerable
			.GetShortestPathCost<(int x, int y), int>(
				(0, 0),
				(p, c) => p.GetCartesianNeighbors()
					.Where(p => p.x.Between(0, 70) && p.y.Between(0, 70))
					.Where(p => !firstKilo.Contains(p))
					.Select(p => (p, c + 1)),
				(70, 70)
			);


		for (var i = 1025; i < errors.Count; i++)
		{
			firstKilo = errors.Take(i).ToHashSet();
			try
			{
				SuperEnumerable
					.GetShortestPathCost<(int x, int y), int>(
						(0, 0),
						(p, c) => p.GetCartesianNeighbors()
							.Where(p => p.x.Between(0, 70) && p.y.Between(0, 70))
							.Where(p => !firstKilo.Contains(p))
							.Select(p => (p, c + 1)),
						(70, 70)
					);
			}
			catch (Exception ex)
			{
				var (x, y) = errors[i - 1];
				return (part1.ToString(), $"{x},{y}");
			}
		}

		var part2 = 0;
		return (part1.ToString(), part2.ToString());
	}
}
