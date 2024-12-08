using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 08, CodeType.Original)]
public partial class Day_08_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		var antennas = map.GetMapPoints()
			.Where(p => p.item != '.')
			.ToLookup(p => p.item, p => p.p);

		var part1 = antennas
			.SelectMany(g => g.Subsets(2)
				.SelectMany(w => new[]
				{
					(x: w[0].x - (w[1].x - w[0].x), y: w[0].y - (w[1].y - w[0].y)),
					(x: w[1].x - (w[0].x - w[1].x), y: w[1].y - (w[0].y - w[1].y)),
				})
			)
			.Distinct()
			.Count(p => p.x.Between(0, map[0].Length - 1) && p.y.Between(0, map.Length - 1))
			.ToString();

		var part2 = antennas
			.SelectMany(g => g.Subsets(2)
				.SelectMany(w => Enumerable.Range(0, 30)
					.SelectMany(i => new[]
					{
						(x: w[0].x - (w[1].x - w[0].x) * i, y: w[0].y - (w[1].y - w[0].y) * i),
						(x: w[1].x - (w[0].x - w[1].x) * i, y: w[1].y - (w[0].y - w[1].y) * i),
					})
				)
			)
			.Distinct()
			.Count(p => p.x.Between(0, map[0].Length - 1) && p.y.Between(0, map.Length - 1))
			.ToString();

		return (part1, part2);
	}
}
