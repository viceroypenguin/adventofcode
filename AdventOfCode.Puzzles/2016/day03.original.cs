namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 03, CodeType.Original)]
public class Day_03_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var partA = input.Lines
			.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
			.Select(line => new { A = Convert.ToInt32(line[0]), B = Convert.ToInt32(line[1]), C = Convert.ToInt32(line[2]), })
			.Count(tri => tri.A + tri.B > tri.C && tri.A + tri.C > tri.B && tri.B + tri.C > tri.A);

		var partB = input.Lines
			.Select((line, idx) => new { parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries), idx })
			.GroupBy(x => x.idx / 3)
			.SelectMany(x =>
			{
				var arr = x.Select(z => z.parts).ToArray();
				return new[]
				{
					new { A = Convert.ToInt32(arr[0][0]), B = Convert.ToInt32(arr[1][0]), C = Convert.ToInt32(arr[2][0]), },
					new { A = Convert.ToInt32(arr[0][1]), B = Convert.ToInt32(arr[1][1]), C = Convert.ToInt32(arr[2][1]), },
					new { A = Convert.ToInt32(arr[0][2]), B = Convert.ToInt32(arr[1][2]), C = Convert.ToInt32(arr[2][2]), },
				};
			})
			.Count(tri => tri.A + tri.B > tri.C && tri.A + tri.C > tri.B && tri.B + tri.C > tri.A);

		return (partA.ToString(), partB.ToString());
	}
}
