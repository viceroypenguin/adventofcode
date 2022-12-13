namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 03, CodeType.Original)]
public class Day_03_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var wirePaths = input.Lines
			.Select(w => w
				.Split(',')
				.ScanEx(
					new[] { (x: 0, y: 0, steps: 0), },
					(pos, dir) =>
					{
						var number = Convert.ToInt32(dir[1..]);
						var last = pos.Last();
						return dir[0] switch
						{
							'R' => Enumerable.Range(1, number).Select(x => (last.x + x, last.y, last.steps + x)).ToArray(),
							'L' => Enumerable.Range(1, number).Select(x => (last.x - x, last.y, last.steps + x)).ToArray(),
							'U' => Enumerable.Range(1, number).Select(y => (last.x, last.y + y, last.steps + y)).ToArray(),
							'D' => Enumerable.Range(1, number).Select(y => (last.x, last.y - y, last.steps + y)).ToArray(),
						};
					})
				.SelectMany(x => x))
			.ToList();

		var hash = wirePaths[0]
			.ToLookup(pos => (pos.x, pos.y), pos => pos.steps)
			.ToDictionary(g => g.Key, g => g.Min());

		var intersections = wirePaths[1]
			.Where(pos => pos != (0, 0, 0))
			.Where(pos => hash.ContainsKey((pos.x, pos.y)))
			.ToList();

		var part1 = intersections
			.Select(pos => Math.Abs(pos.x) + Math.Abs(pos.y))
			.Min()
			.ToString();

		var part2 = intersections
			.Select(pos => pos.steps + hash[(pos.x, pos.y)])
			.Min()
			.ToString();

		return (part1, part2);
	}
}
