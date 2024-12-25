namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 25, CodeType.Original)]
public partial class Day_25_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var locks = new List<List<int>>();
		var keys = new List<List<int>>();

		foreach (var grid in input.Lines.Split(string.Empty))
		{
			if (grid[0][0] is '#')
			{
				var ys = Enumerable.Range(0, grid[0].Length)
					.Select(x => Enumerable.Range(1, 5).TakeWhile(y => grid[y][x] is '#').Count())
					.ToList();
				locks.Add(ys);
			}
			else
			{
				var ys = Enumerable.Range(0, grid[0].Length)
					.Select(x => SuperEnumerable.Sequence(5, 1).TakeWhile(y => grid[y][x] is '#').Count())
					.ToList();
				keys.Add(ys);
			}
		}

		var part1 = locks.SelectMany(l => keys.Where(k => l.Zip(k).All(x => x.First + x.Second <= 5))).Count();

		return (part1.ToString(), "");
	}
}
