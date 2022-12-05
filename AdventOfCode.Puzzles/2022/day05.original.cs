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

		var instructions = segments.Last()
			.Select(x => InstructionRegex().Match(x))
			.Select(m => (
				cnt: int.Parse(m.Groups[1].Value),
				from: int.Parse(m.Groups[2].Value),
				to: int.Parse(m.Groups[3].Value)))
			.ToList();

		var stacks = BuildStacks(mapLines);
		foreach (var (cnt, from, to) in instructions)
		{
			for (int i = 0; i < cnt; i++)
			{
				stacks[to - 1].Add(stacks[from - 1][^1]);
				stacks[from - 1].RemoveAt(stacks[from - 1].Count - 1);
			}
		}

		var part1 = string.Join("", stacks.Select(s => s[^1]));

		stacks = BuildStacks(mapLines);
		foreach (var (cnt, from, to) in instructions)
		{
			for (int i = 0; i < cnt; i++)
				stacks[to - 1].Add(stacks[from - 1][^(cnt - i)]);
			stacks[from - 1].RemoveRange(
				stacks[from - 1].Count - cnt,
				cnt);
		}

		var part2 = string.Join("", stacks.Select(s => s[^1]));

		return (part1, part2);
	}

	private static List<List<char>> BuildStacks(List<string> map)
	{
		var stacks = map[^1]
			.Split()
			.Where(s => !string.IsNullOrWhiteSpace(s))
			.Select(s => new List<char>())
			.ToList();

		for (int i = 0; i < stacks.Count; i++)
			for (int j = map.Count - 2; j >= 0; j--)
			{
				var c = map[j][i * 4 + 1];
				if (c != ' ')
					stacks[i].Add(c);
			}

		return stacks;
	}
}
