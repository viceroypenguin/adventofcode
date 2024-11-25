namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 03, CodeType.Original)]
public class Day_03_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var current = (x: 0, y: 0);
		var santaHouses = input.Bytes
			.Select(c =>
			{
				if (c == '>') current = (current.x + 1, current.y);
				if (c == '<') current = (current.x - 1, current.y);
				if (c == '^') current = (current.x, current.y + 1);
				if (c == 'v') current = (current.x, current.y - 1);
				return current;
			})
			.Concat([(0, 0)])
			.Distinct()
			.Count();

		current = (x: 0, y: 0);
		var other = current;
		var bothHouses = input.Bytes
			.Select(c =>
			{
				var t = other;
				if (c == '>') other = (current.x + 1, current.y);
				if (c == '<') other = (current.x - 1, current.y);
				if (c == '^') other = (current.x, current.y + 1);
				if (c == 'v') other = (current.x, current.y - 1);
				current = t;
				return other;
			})
			.Concat([(0, 0)])
			.Distinct()
			.Count();

		return (
			santaHouses.ToString(),
			bothHouses.ToString());
	}
}
