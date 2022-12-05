namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 13, CodeType.Original)]
public class Day_13_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		// get the initial dots
		var dots = input.Lines
			// don't include the fold instructions
			.TakeWhile(l => !string.IsNullOrWhiteSpace(l))
			// parse (x,y)
			.Select(x => x.Split(','))
			.Select(x => (x: Convert.ToInt32(x[0]), y: Convert.ToInt32(x[1])))
			.ToList();

		// get the fold instructinos
		var folds = input.Lines
			// has to start with fold
			.Where(l => l.StartsWith("fold"))
			// don't care about "fold along "
			.Select(x => x[11..])
			// parse direction and value
			.Select(x => (dir: x[0], coord: Convert.ToInt32(x[2..])))
			.ToList();

		// a single fold instruction:
		static List<(int x, int y)> fold(List<(int x, int y)> dots, char dir, int coord) =>
			// which coordinate
			dir == 'x'
				// x-coord folds over the x-axis
				? dots
					// example: if fold is at 500
					// then 400 will be at 500 - (500 - 400) = 400
					// then 600 will be at 500 - (500 - 400) = 400
					.Select(d => (coord - Math.Abs(d.x - coord), d.y))
					.Distinct()
					.ToList()
				// fold over the y-axis
				: dots
					.Select(d => (d.x, coord - Math.Abs(d.y - coord)))
					.Distinct()
					.ToList();

		// fold once, count the distinct points, and dump it
		var part1 = fold(dots, folds[0].dir, folds[0].coord).Count.ToString();

		// follow every fold instruction
		foreach (var (dir, coord) in folds)
			dots = fold(dots, dir, coord);

		// build an empty character map for display
		var map = Enumerable.Range(0, dots.Max(x => x.y + 1))
			.Select(x => Enumerable.Repeat(' ', dots.Max(x => x.x+ 1)).ToArray())
			.ToArray();

		// set each remaining dot on the display
		foreach (var (x, y) in dots)
			map[y][x] = '█';

		// dump it out - no need to OCR this
		var part2 = string.Join(Environment.NewLine,
			map.Select(y => string.Join(string.Empty, y)));

		return (part1, part2);
	}
}
