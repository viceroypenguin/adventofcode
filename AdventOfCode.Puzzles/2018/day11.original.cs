namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var serialNumber = Convert.ToInt32(input.Text);
		var cells = new int[301, 301];

		for (var x = 1; x <= 300; x++)
		{
			var rackId = x + 10;

			for (var y = 1; y <= 300; y++)
			{
				var powerLevel = ((rackId * y) + serialNumber) * rackId;
				cells[x, y] = ((powerLevel % 1000) / 100) - 5;
			}
		}

		var part1 =
			(
				from x in Enumerable.Range(1, 298)
				from y in Enumerable.Range(1, 298)
				select (x, y, sum: (
					from x2 in Enumerable.Range(x, 3)
					from y2 in Enumerable.Range(y, 3)
					select cells[x2, y2]).Sum())
			)
			.OrderByDescending(x => x.sum)
			.First()
			.ToString();

		var part2 =
			(
				from size in Enumerable.Range(1, 30)
				from x in Enumerable.Range(1, 301 - size)
				from y in Enumerable.Range(1, 301 - size)
				select (x, y, size, sum: (
					from x2 in Enumerable.Range(x, size)
					from y2 in Enumerable.Range(y, size)
					select cells[x2, y2]).Sum())
			)
			.OrderByDescending(x => x.sum)
			.First()
			.ToString();

		return (part1, part2);
	}
}
