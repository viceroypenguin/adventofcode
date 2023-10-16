namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 5, CodeType.Original)]
public partial class Day_05_Original : IPuzzle
{
	[GeneratedRegex(@"move (\d+) from (\d+) to (\d+)")]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var segments = input.Lines
			.Split(string.Empty);
		var mapLines = segments.First().ToList();

		var instructions = input.Lines
			.SkipUntil(string.IsNullOrWhiteSpace)
			.Select(x => InstructionRegex().Match(x))
			.Select(m => (
				cnt: int.Parse(m.Groups[1].Value),
				from: int.Parse(m.Groups[2].Value),
				to: int.Parse(m.Groups[3].Value)))
			.ToList();

		var stacks = BuildStacks(input.Lines);
		foreach (var (cnt, from, to) in instructions)
			for (var i = 0; i < cnt; i++)
				stacks[to - 1].Push(stacks[from - 1].Pop());

		var part1 = string.Join("", stacks.Select(s => s.Peek()));

		stacks = BuildStacks(input.Lines);
		foreach (var (cnt, from, to) in instructions)
		{
			var tmp = new Stack<char>();
			for (var i = 0; i < cnt; i++)
				tmp.Push(stacks[from - 1].Pop());
			for (var i = 0; i < cnt; i++)
				stacks[to - 1].Push(tmp.Pop());
		}

		var part2 = string.Join("", stacks.Select(s => s.Peek()));

		return (part1, part2);
	}

	private static List<Stack<char>> BuildStacks(string[] lines) =>
		lines.TakeWhile(l => l[1] != '1')
			.Transpose()
			.Select(l => l.Reverse())
			.Where(l => l.First() is >= 'A' and <= 'Z')
			.Select(l => new Stack<char>(l.Where(c => c != ' ')))
			.ToList();
}
