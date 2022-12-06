namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 21, CodeType.Original)]
public partial class Day_21_Original : IPuzzle
{
	[GeneratedRegex("^((?<ingredient>\\w+) )+\\(contains ((?<allergen>\\w+)(, )?)+\\)$", RegexOptions.ExplicitCapture)]
	private static partial Regex IngredientRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = IngredientRegex();
		var recipes = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => (
				ingredients: m.Groups["ingredient"].Captures.Select(c => c.Value).ToList(),
				allergens: m.Groups["allergen"].Captures.Select(c => c.Value).ToList()))
			.ToList();

		var allergenMap = new Dictionary<string, string>();
		var allergens = recipes.SelectMany(r => r.allergens).Distinct().ToList();
		do
		{
			foreach (var a in allergens.ToList())
			{
				var candidates = recipes.First(r => r.allergens.Contains(a)).ingredients.ToHashSet();
				foreach (var r in recipes.Where(r => r.allergens.Contains(a)).Skip(1))
					candidates.IntersectWith(r.ingredients);
				if (candidates.Count == 1)
				{
					var i = allergenMap[a] = candidates.Single();
					foreach (var r in recipes)
						r.ingredients.Remove(i);
					allergens.Remove(a);
				}
			}
		} while (allergens.Count != 0);

		var part1 = recipes.SelectMany(r => r.ingredients).Count().ToString();
		var part2 = string.Join(",", allergenMap.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));

		return (part1, part2);
	}
}
