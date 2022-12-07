namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 7, CodeType.Original)]
public partial class Day_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var cwd = string.Empty;
		var dirs = new Dictionary<string, List<(string name, int? size)>>();

		foreach (var l in input.Lines)
		{
			if (l.Equals("$ cd .."))
			{
				var previousSlash = cwd.LastIndexOf('/', cwd.Length - 2) + 1;
				var name = cwd[previousSlash..^1];
				var parent = cwd[..previousSlash];

				var idx = dirs[parent].FindIndex(x => x.name == name);
				dirs[parent][idx] = (name, dirs[cwd].Sum(x => x.size!.Value));

				cwd = parent;
			}
			else if (l.StartsWith("$ cd "))
			{
				var path = l[5..];
				cwd = path.StartsWith("/") ? path : $"{cwd}{path}/";
			}
			else if (l.StartsWith("dir"))
			{
				dirs.GetOrAdd(cwd, _ => new())
					.Add((name: l[4..], size: null));
			}
			else if (!l.StartsWith("$ ls"))
			{
				var size = int.Parse(l.Split()[0]);
				var name = l.Split()[1];

				dirs.GetOrAdd(cwd, _ => new())
					.Add((name, size));
			}
		}

		while (cwd != "/")
		{
			var previousSlash = cwd.LastIndexOf('/', cwd.Length - 2) + 1;
			var name = cwd[previousSlash..^1];
			var parent = cwd[..previousSlash];

			var idx = dirs[parent].FindIndex(x => x.name == name);
			dirs[parent][idx] = (name, dirs[cwd].Sum(x => x.size!.Value));

			cwd = parent;
		}

		var part1 = dirs.Values.Select(v => v.Sum(x => x.size!.Value))
			.Where(x => x <= 100_000)
			.Sum()
			.ToString();

		var unused = 70_000_000 - dirs["/"].Sum(x => x.size!.Value);
		var needed = 30_000_000 - unused;

		var part2 = dirs.Values.Select(v => v.Sum(x => x.size!.Value))
			.Where(x => x >= needed)
			.Min()
			.ToString();

		return (part1, part2);
	}
}
