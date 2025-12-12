
namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 12, CodeType.Original)]
public partial class Day_12_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = input.Lines.Skip(30)
			.Select(l => LineRegex.Match(l))
			.Count(
				m =>
					int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)
						>= 9 * m.Groups.OfType<Group>().Skip(3).Sum(x => int.Parse(x.Value))
			);

		var part2 = 0L;

		return (part1.ToString(), part2.ToString());
	}

	[GeneratedRegex(@"^(\d+)x(\d+): (\d+) (\d+) (\d+) (\d+) (\d+) (\d+)")]
	private static partial Regex LineRegex { get; }
}
