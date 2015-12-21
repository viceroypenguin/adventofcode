<Query Kind="Statements" />

var numberToBeat = 29000000 / 10;

var numbers = new List<Tuple<int, Dictionary<int, int>>>()
{
	Tuple.Create(0, new Dictionary<int, int>()             ),
	Tuple.Create(1, new Dictionary<int, int>() { { 1, 1 } }),
	Tuple.Create(3, new Dictionary<int, int>() { { 2, 1 } }),
};
for (var num = 3; ; num++)
{
	var factors = new Dictionary<int, int>();

	var numDiv = num;
	for (var factor = (int)Math.Sqrt(num); factor > 1; factor--)
		if (factor * (numDiv / factor) == numDiv)
		{
			foreach (var x in numbers[factor].Item2)
				if (factors.ContainsKey(x.Key))
					factors[x.Key] = factors[x.Key] + x.Value;
				else
					factors[x.Key] = x.Value;
			
			numDiv /= factor;
			factor = numDiv + 1;
		}
	
	if (!factors.Any())
		factors.Add(num, 1);
	
	var sum = factors.Select(x=>Enumerable.Range(0, x.Value + 1).Select(y=>(int)Math.Pow(x.Key, y)).Sum()).ToList();
	var prod = 1;
	foreach (var x in sum) prod *= x;
	
	numbers.Add(Tuple.Create(prod, factors));
	
	if (prod >= numberToBeat)
	break;
}

(numbers.Count - 1).Dump();
