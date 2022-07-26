using System.Collections.Immutable;

namespace AdventOfCode;

public class Day_2015_11_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 11;
	public override CodeType CodeType => CodeType.Original;

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

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var stack = ImmutableStack<int>.Empty;
		foreach (var c in input)
			stack = stack.Push((int)c - (int)'a');

		var passwords = GetPasswords(stack)
			.Take(2)
			.Select(p => PassAsString(p))
			.ToArray();

		Dump('A', passwords[0]);
		Dump('B', passwords[1]);
	}
}
