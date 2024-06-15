namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 07, CodeType.Original)]
public class Day_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		static bool IsABBA(string str)
		{
			for (var i = 0; i < str.Length - 3; i++)
			{
				if (str[i] == str[i + 3] && str[i + 1] == str[i + 2] && str[i] != str[i + 1])
					return true;
			}

			return false;
		}

		var splits =
			input.Lines
				.Select(str =>
				{
					var outside = new List<string>();
					var inside = new List<string>();

					var idx = 0;
					while (idx < str.Length)
					{
						var newIdx = str.IndexOf('[', idx);
						if (newIdx < 0)
						{
							outside.Add(str[idx..]);
							break;
						}

						outside.Add(str[idx..newIdx]);
						idx = newIdx;

						newIdx = str.IndexOf(']', idx);
						inside.Add(str.Substring(idx + 1, newIdx - idx - 1));
						idx = newIdx + 1;
					}

					return new { str, inside, outside };
				})
				.ToList();

		var partA =
			splits
				.Where(x => x.outside.Any(IsABBA) && !x.inside.Any(IsABBA))
				.Count();

		static IList<string> GetABAs(IList<string> strs)
		{
			var abas = new List<string>();
			foreach (var str in strs)
			{
				for (var i = 0; i < str.Length - 2; i++)
				{
					if (str[i] == str[i + 2] && str[i] != str[i + 1])
						abas.Add(str.Substring(i, 3));
				}
			}

			return abas;
		}

		bool CheckBABs(IList<string> strs, IList<string> abas)
		{
			foreach (var aba in abas)
			{
				var bab = string.Join("", new[] { aba[1], aba[0], aba[1] });
				if (strs.Any(s => s.Contains(bab)))
					return true;
			}

			return false;
		}

		var partB =
			splits
				.Select(x => new
				{
					x.str,
					x.inside,
					x.outside,
					abas = GetABAs(x.outside),
				})
				.Count(x => CheckBABs(x.inside, x.abas));

		return (partA.ToString(), partB.ToString());
	}
}
