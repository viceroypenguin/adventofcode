<Query Kind="Statements" />

var input =
@"Frosting: capacity 4, durability -2, flavor 0, texture 0, calories 5
Candy: capacity 0, durability 5, flavor -1, texture 0, calories 8
Butterscotch: capacity -1, durability 0, flavor 5, texture 0, calories 6
Sugar: capacity 0, durability 0, flavor -2, texture 2, calories 1

";
var totalIngredients = 100;
var totalCalories = 500;

var regex = new Regex(@"(\w+): capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)");

var ingredients = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
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

Func<IEnumerable<int>, bool> validCalories = (qtys) => qtys
	.Zip(
		ingredients,
		(q, i) => new { q, i })
	.Aggregate(
		0,
		(calories, _) => calories + _.q * _.i.calories,
		calories => calories == totalCalories);

Func<IEnumerable<int>, int> scoreFunc = (IEnumerable<int> qtys) => qtys
	.Zip(
		ingredients,
		(q, i) => new { q, i })
	.Aggregate(
		new { capacity = 0, durability = 0, flavor = 0, texture = 0, calories = 0 },
		(agg, _) => new
		{
			capacity = agg.capacity + _.q * _.i.capacity,
			durability = agg.durability + _.q * _.i.durability,
			flavor = agg.flavor + _.q * _.i.flavor,
			texture = agg.texture + _.q * _.i.texture,
			calories = agg.calories + _.q * _.i.calories,
		},
	 agg => Math.Max(agg.capacity, 0) * Math.Max(agg.durability, 0) * Math.Max(agg.flavor, 0) * Math.Max(agg.texture, 0));

var numIngredients = ingredients.Count;
var baseList = Enumerable.Range(0, ingredients.Count).ToArray();

var currValue = baseList.Select(_ => 0).ToArray();
currValue[0] = -1;

var maxValue = currValue.ToArray();
var maxScore = 0;

Func<bool> getNextValue = () =>
{
	var i = 0;
	do
	{
		if (++currValue[i] == 101)
		{ currValue[i] = 0; i++; }
		else
			break;

		if (i == numIngredients - 1)
			return false;
	} while (true);

	currValue[numIngredients - 1] = 0;
	currValue[numIngredients - 1] = totalIngredients - currValue.Sum();

	return true;
};

// full space search.  terrible, but few parameters so brute force works
// don't want to write full optimizing engine
while (getNextValue())
{
	if (!validCalories(currValue))
		continue;
	
	var score = scoreFunc(currValue);

	if (score > maxScore)
	{
		maxValue = currValue.ToArray();
		maxScore = score;
	}
}

maxValue.Dump();
maxScore.Dump();
