namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 15, CodeType.Original)]
public partial class Day_15_Original : IPuzzle
{
	[GeneratedRegex("(\\w+): capacity (-?\\d+), durability (-?\\d+), flavor (-?\\d+), texture (-?\\d+), calories (-?\\d+)")]
	private static partial Regex IngredientRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var totalIngredients = 100;
		var totalCalories = 500;

		var regex = IngredientRegex();

		var ingredients = input.Lines
			.Select(x => regex.Match(x))
			.Select(x => new
			{
				name = x.Groups[1].Value,
				capacity = Convert.ToInt32(x.Groups[2].Value),
				durability = Convert.ToInt32(x.Groups[3].Value),
				flavor = Convert.ToInt32(x.Groups[4].Value),
				texture = Convert.ToInt32(x.Groups[5].Value),
				calories = Convert.ToInt32(x.Groups[6].Value),
			})
			.ToList();

		bool validCalories(IEnumerable<int> qtys) => qtys
			.Zip(
				ingredients,
				(q, i) => new { q, i })
			.Aggregate(
				0,
				(calories, _) => calories + (_.q * _.i.calories),
				calories => calories == totalCalories);

		int scoreFunc(IEnumerable<int> qtys) => qtys
			.Zip(
				ingredients,
				(q, i) => new { q, i })
			.Aggregate(
				new { capacity = 0, durability = 0, flavor = 0, texture = 0, calories = 0 },
				(agg, _) => new
				{
					capacity = agg.capacity + (_.q * _.i.capacity),
					durability = agg.durability + (_.q * _.i.durability),
					flavor = agg.flavor + (_.q * _.i.flavor),
					texture = agg.texture + (_.q * _.i.texture),
					calories = agg.calories + (_.q * _.i.calories),
				},
				agg => Math.Max(agg.capacity, 0) * Math.Max(agg.durability, 0) * Math.Max(agg.flavor, 0) * Math.Max(agg.texture, 0));

		var numIngredients = ingredients.Count;
		var baseList = Enumerable.Range(0, ingredients.Count).ToArray();

		var currValue = baseList.Select(_ => 0).ToArray();
		currValue[0] = -1;

		var max500Score = 0;
		var maxRawScore = 0;

		bool getNextValue()
		{
			var i = 0;
			do
			{
				if (++currValue[i] == 101)
				{
					currValue[i] = 0;
					i++;
				}
				else
				{
					break;
				}

				if (i == numIngredients - 1)
					return false;
			} while (true);

			currValue[numIngredients - 1] = 0;
			currValue[numIngredients - 1] = totalIngredients - currValue.Sum();

			return true;
		}

		// full space search.  terrible, but few parameters so brute force works
		// don't want to write full optimizing engine
		while (getNextValue())
		{
			var score = scoreFunc(currValue);
			maxRawScore = Math.Max(maxRawScore, score);

			if (validCalories(currValue))
				max500Score = Math.Max(max500Score, score);
		}

		return (
			maxRawScore.ToString(),
			max500Score.ToString());
	}
}
