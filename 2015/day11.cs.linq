<Query Kind="Program">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

int[] invalidChars =
{
	(int)'i' - (int)'a',
	(int)'o' - (int)'a',
	(int)'l' - (int)'a',
};

ImmutableStack<int> IncrementPassword(ImmutableStack<int> password)
{
	var chr = password.Peek();
	var tail = password.Pop();
	
	if (chr == 25)
		return IncrementPassword(tail).Push(0);
	else if (invalidChars.Contains(chr + 1))
		return tail.Push(chr + 2);
	else
		return tail.Push(chr + 1);
}

IEnumerable<ImmutableStack<int>> GetIncrementingPasswords(ImmutableStack<int> password)
{
	while (true)
	{
		password = IncrementPassword(password);
		yield return password;
	}
}

bool ContainsStrictlyIncreasingTriplet(IEnumerable<int> password)
{
	return password.Zip(password.Skip(1), (a, b) => new { a, b })
		.Zip(password.Skip(2), (_, c) => new { _.a, _.b, c })
		.Where(_ => _.a - 1 == _.b && _.b - 1 == _.c)
		.Any();
}

bool ContainsTwoDuplicates(IEnumerable<int> password)
{
	var duplicates = password
		.Zip(
			password.Skip(1),
			(a, b) => new { a, b })
		.Where(_ => _.a == _.b)
		.ToList();

	return duplicates.Distinct().Count() > 1;
}

IEnumerable<ImmutableStack<int>> GetPasswords(ImmutableStack<int> password)
{
	return GetIncrementingPasswords(password)
		.Where(p =>
			ContainsStrictlyIncreasingTriplet(p) &&
			ContainsTwoDuplicates(p));
}

string PassAsString(IImmutableStack<int> password)
{
	return string.Join("", password.Reverse().Select(i => (char)(i + (int)'a')));
}

void Main()
{
	var input = "hxbxwxba";
	var stack = ImmutableStack<int>.Empty;
	foreach (var c in input)
		stack = stack.Push((int)c - (int)'a');
	
	GetPasswords(stack)
		.Take(2)
		.Select(p => PassAsString(p))
		.Dump();
}

// Define other methods and classes here