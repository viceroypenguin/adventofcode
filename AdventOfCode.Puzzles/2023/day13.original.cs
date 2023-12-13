namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 13, CodeType.Original)]
public partial class Day_13_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var patterns = input.Lines
			.Split(string.Empty)
			.Select(p => p.ToArray())
			.ToList();

		var part1 = patterns
			.Select(FindReflection)
			.Sum();

		var part2 = patterns
			.Select(FixSmudge)
			.Sum();

		return (part1.ToString(), part2.ToString());
	}

	private static int FixSmudge(string[] pattern)
	{
		var orig = FindReflection(pattern);

		for (var y = 0; y < pattern.Length; y++)
		{
			var p1 = pattern[y].ToCharArray();
			for (var x = 0; x < p1.Length; x++)
			{
				var c = p1[x];
				p1[x] = c == '.' ? '#' : '.';
				pattern[y] = new string(p1);

				var @new = FindReflection(pattern, orig);
				if (@new != 0 && @new != orig)
					return @new;

				p1[x] = c;
			}
			pattern[y] = new string(p1);
		}
		return -1;
	}

	private static int FindReflection(string[] pattern, int orig = 0)
	{
		var xReflections = GetReflections(pattern)
			.Select(x => (x.length, value: x.value * 100))
			.ToList();

		pattern = pattern
			.Select(p => p.ToCharArray())
			.Transpose()
			.Select(p => new string(p.ToArray()))
			.ToArray();

		var yReflections = GetReflections(pattern).ToList();

		return xReflections.Concat(yReflections)
			.Where(x => x.value != orig)
			.OrderByDescending(x => x.length)
			.Select(x => x.value)
			.FirstOrDefault();
	}

	private static IEnumerable<(int length, int value)> GetReflections(string[] pattern)
	{
		for (var i = 0; i < pattern.Length - 1; i++)
		{
			if (pattern[i] == pattern[i + 1])
			{
				var flag = true;
				var max = Math.Min(i, pattern.Length - i - 2);
				for (var j = 0; flag && j <= max; j++)
				{
					if (pattern[i - j] != pattern[i + j + 1])
						flag = false;
				}

				if (flag)
					yield return (max, i + 1);
			}
		}
	}
}
