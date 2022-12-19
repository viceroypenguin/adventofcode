namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 05, CodeType.Original)]
public class Day_05_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var poly = input.Text;

		int GetReducedPolymerLength(string polymer)
		{
			var characters = polymer
				.Select(c => (c, isActive: true))
				.ToArray();

			for (int i = 1; i < characters.Length; i++)
			{
				int j = i - 1;
				while (j >= 0 && !characters[j].isActive)
					j--;
				if (j >= 0 &&
					char.IsUpper(characters[i].c) != char.IsUpper(characters[j].c)
					&& char.ToUpper(characters[i].c) == char.ToUpper(characters[j].c))
				{
					characters[i].isActive = false;
					characters[j].isActive = false;
				}
			}

			return characters.Where(x => x.isActive).Count();
		}

		var part1 = GetReducedPolymerLength(poly).ToString();

		var part2 = Enumerable.Range(0, 26)
			.Select(i => (char)(i + (int)'a'))
			.Select(c => Regex.Replace(poly, c.ToString(), "", RegexOptions.IgnoreCase))
			.Min(s => GetReducedPolymerLength(s))
			.ToString();

		return (part1, part2);
	}
}
