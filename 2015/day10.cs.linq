<Query Kind="Statements" />

var input = "1113122113";
var inputArr = input.ToCharArray().ToList();

foreach (var _ in Enumerable.Range(1, 50))
{
	var curLength = 1;
	var curChar = inputArr[0];
	var newInput = new List<char>();

	var idx = 1;
	while (idx < inputArr.Count)
	{
		var c = inputArr[idx];
		idx++;

		if (c == curChar)
			curLength++;
		else
		{
			newInput.AddRange(curLength.ToString().ToCharArray());
			newInput.Add(curChar);

			curLength = 1;
			curChar = c;
		}
	}

	newInput.AddRange(curLength.ToString().ToCharArray());
	newInput.Add(curChar);

	inputArr = newInput;
	
	if (_ == 40 || _ == 50)
		inputArr.Count.Dump(_ == 40 ? "Part A" : "Part B");
}
