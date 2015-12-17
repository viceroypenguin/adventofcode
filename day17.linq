<Query Kind="Program" />

void Main()
{
	var input =
@"50
44
11
49
42
46
18
32
26
40
21
7
18
43
10
47
36
24
22
40";

	var total = 150;

	var containers = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(s => Convert.ToInt32(s))
		.ToList();

	var cnt = 0;
	var max = 1 << containers.Count;
	var numCombinations = 0;
	
	var minPop = int.MaxValue;
	var haveMinPop = 0;
	while (cnt < max)
	{
		var sum = 0;
		var bitstream = new BitArray(new[] { cnt });
		foreach (var _ in bitstream.OfType<bool>().Select((b, i) => new { b, i }))
			if (_.b)
				sum += containers[_.i];

		if (sum == total)
		{
			numCombinations++;

			var pop = bitstream.OfType<bool>().Where(b => b).Count();
			if (pop < minPop)
			{
				minPop = pop;
				haveMinPop = 1;
			}
			else if (pop == minPop)
				haveMinPop++;
			//Convert.ToString(cnt, 2).PadLeft(containers.Count, '0').Dump();
		}
		
		cnt++;
	}
	
	numCombinations.Dump();
	minPop.Dump();
	haveMinPop.Dump();
}


