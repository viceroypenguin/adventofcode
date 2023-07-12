namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 15, CodeType.Original)]
public partial class Day_15_Original : IPuzzle
{
	[GeneratedRegex(@"^Generator (?<gen>\w) starts with (?<init>\d+)$", RegexOptions.Compiled)]
	private static partial Regex GeneratorRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = GeneratorRegex();
		var generators = input.Lines
			.Select(l => regex.Match(l))
			.ToDictionary(
				m => m.Groups["gen"].Value,
				m => (ulong)Convert.ToInt32(m.Groups["init"].Value));

		var partA =
			Enumerable.Range(0, 40_000_000)
				.Scan(
					(a: generators["A"], b: generators["B"]),
					(state, _) => (state.a * 16807 % 2147483647, state.b * 48271 % 2147483647))
				.Skip(1)
				.Count(s => (ushort)s.a == (ushort)s.b);

		var aGenerator = SuperEnumerable
			.Generate(generators["A"], a => a * 16807 % 2147483647)
			.Where(a => a % 4 == 0);
		var bGenerator = SuperEnumerable
			.Generate(generators["B"], b => b * 48271 % 2147483647)
			.Where(b => b % 8 == 0);

		var partB =
			aGenerator.Zip(bGenerator, (a, b) => (a, b))
				.Take(5_000_000)
				.Count(s => (ushort)s.a == (ushort)s.b);

		return (partA.ToString(), partB.ToString());
	}
}
