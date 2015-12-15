<Query Kind="Program" />

bool ContainsStrictlyIncreasingTriplet(char[] password)
{
	var intList = password
		.Select(c => (int)c)
		.ToList();

	return intList.Zip(intList.Skip(1), (a, b) => new { a, b })
		.Zip(intList.Skip(2), (_, c) => new { _.a, _.b, c })
		.Where(_ => _.a + 1 == _.b && _.b + 1 == _.c)
		.Any();
}

bool ContainsTwoDuplicates(char[] password)
{
	var duplicates = password
		.Zip(
			password.Skip(1),
			(a, b) => new { a, b })
		.Where(_ => _.a == _.b)
		.ToList();

	return duplicates.Distinct().Count() > 1;
}

char IncrementChar(char c)
{
	switch (c)
	{
		case 'i':
		case 'o':
		case 'l':
			return (char)((int)c + 2);

		case 'z':
			return 'a';

		default:
			return (char)((int)c + 1);
	}
}

void IncrementCharRef(ref char c)
{
	c = IncrementChar(c);
}

void ResetInvalidChars(char[] password)
{
	var flag = false;
	for (int i = 0; i < password.Length; i++)
	{
		if (flag)
			password[i] = 'a';
		else
			switch (password[i])
			{
				case 'i':
				case 'o':
				case 'l':
					password[i] = (char)((int)password[i] + 1);
					flag = true;
					break;
			}
	}
}

void GetNextPassword(char[] password)
{
	while (true)
	{
		var i = password.Length - 1;
		do
		{
			IncrementCharRef(ref password[i]);
			i--;
		} while (i >= 0 && password[i+1] == 'a');

		if (ContainsTwoDuplicates(password) &&
			ContainsStrictlyIncreasingTriplet(password))
			return;
	}
}

void Main()
{
	var input = "hxbxwxba";
	var inputArr = input.ToCharArray();

	ResetInvalidChars(inputArr);

	GetNextPassword(inputArr);
	
	new string(inputArr).Dump();
}

// Define other methods and classes here
