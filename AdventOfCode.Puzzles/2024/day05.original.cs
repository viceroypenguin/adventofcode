namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 05, CodeType.Original)]
public partial class Day_05_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var sections = input.Lines.Split(string.Empty).ToList();

		var regex1 = new Regex(@"^(\d+)\|(\d+)$");
		var beforeInstructions = sections[0]
			.Select(l => regex1.Match(l))
			.ToLookup(x => int.Parse(x.Groups[1].Value), x => int.Parse(x.Groups[2].Value));

		var updates = sections[1]
			.Select(l => l.Split(',').Select(p => int.Parse(p)).ToList())
			.ToList();

		var part1 = updates
			.Where(l => IsValidUpdate(l, beforeInstructions))
			.Sum(l => l[l.Count / 2])
			.ToString();

		var part2 = updates
			.Where(l => !IsValidUpdate(l, beforeInstructions))
			.Select(l => CorrectList(l, beforeInstructions))
			.Sum(l => l[l.Count / 2])
			.ToString();

		return (part1, part2);
	}

	private static bool IsValidUpdate(List<int> l, ILookup<int, int> beforeInstructions)
	{
		for (var i = 1; i < l.Count; i++)
		{
			for (var j = 0; j < i; j++)
			{
				if (beforeInstructions[l[i]].Contains(l[j]))
					return false;
			}
		}

		return true;
	}

	private static List<int> CorrectList(List<int> l, ILookup<int, int> beforeInstructions)
	{
		var newList = new List<int>();

		while (l.Count != 0)
		{
			for (var i = 0; i < l.Count; i++)
			{
				if (!l.Any(j => beforeInstructions[j].Contains(l[i])))
				{
					newList.Add(l[i]);
					l.RemoveAt(i);
					break;
				}
			}
		}

		return newList;
	}
}
