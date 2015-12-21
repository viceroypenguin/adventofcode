<Query Kind="Statements" />

var numberToBeat = 29000000;

var numbers = new List<int>();

var candidateIdx = default(int?);

for (int factor = 1; !candidateIdx.HasValue || factor <= candidateIdx; factor++)
{
	foreach (var mul in Enumerable.Range(1, 50).Reverse().Select(x => x * factor))
	{
		if (mul > candidateIdx)
			continue;
			
		if (numbers.Count < mul)
			numbers.AddRange(Enumerable.Range(numbers.Count, mul - numbers.Count + 1).Select(_ => 0));

		numbers[mul] += factor;
		
		if (numbers[mul] * 11 >= numberToBeat && 
			(!candidateIdx.HasValue || (mul < candidateIdx.Value)))
		{
			mul.Dump();
			numbers[mul].Dump();
			
			candidateIdx = mul;
		}
	}
}

