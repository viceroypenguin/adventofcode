namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 07, CodeType.Original)]
public partial class Day_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = 0;

		var beams = new Dictionary<int, long>
		{
			[input.Lines[0].IndexOf('S')] = 1,
		};

		foreach (var l in input.Lines.Skip(1))
		{
			foreach (var (b, num) in beams.ToList())
			{
				if (l[b] is '^')
				{
					part1++;
					beams.Remove(b);
					beams[b - 1] = beams.GetValueOrDefault(b - 1) + num;
					beams[b + 1] = beams.GetValueOrDefault(b + 1) + num;
				}
			}
		}

		var part2 = beams.Values.Sum();

		return (part1.ToString(), part2.ToString());
	}
}
