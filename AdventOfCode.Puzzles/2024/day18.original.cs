namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 18, CodeType.Original)]
public partial class Day_18_Original : IPuzzle
{
	[GeneratedRegex(@"(\d+),(\d+)")]
	private static partial Regex CoordinateRegex { get; }

	public (string, string) Solve(PuzzleInput input)
	{
		var errors = input.Lines
			.Select(l => CoordinateRegex.Match(l))
			.Select(m =>
				(
					x: int.Parse(m.Groups[1].Value),
					y: int.Parse(m.Groups[2].Value)
				)
			)
			.ToList();

		var part1 = GetShortestPathCost(errors.Take(1024).ToHashSet());

		var lo = 1025;
		var hi = errors.Count - 1;
		var mid = (lo + hi) / 2;
		while (lo < hi)
		{
			try
			{
				GetShortestPathCost(errors.Take(mid).ToHashSet());
				lo = mid + 1;
				mid = (lo + hi) / 2;
			}
			catch (InvalidOperationException)
			{
				hi = mid - 1;
				mid = (lo + hi) / 2;
			}
		}

		var (x, y) = errors[mid];
		return (part1.ToString(), $"{x},{y}");
	}

	private static int GetShortestPathCost(HashSet<(int x, int y)> walls) =>
		SuperEnumerable
			.GetShortestPathCost<(int x, int y), int>(
				(0, 0),
				(p, c) => p.GetCartesianNeighbors()
					.Where(p => p.x.Between(0, 70) && p.y.Between(0, 70))
					.Where(p => !walls.Contains(p))
					.Select(p => (p, c + 1)),
				(70, 70)
			);
}
