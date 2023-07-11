using System.Collections.Immutable;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	private static readonly int[] invalidChars =
	{
		'i' - 'a',
		'o' - 'a',
		'l' - 'a',
	};

	private static ImmutableStack<int> IncrementPassword(ImmutableStack<int> password)
	{
		var chr = password.Peek();
		var tail = password.Pop();

		return chr == 25 ? IncrementPassword(tail).Push(0) :
			invalidChars.Contains(chr + 1) ? tail.Push(chr + 2) :
			tail.Push(chr + 1);
	}

	private static IEnumerable<ImmutableStack<int>> GetIncrementingPasswords(ImmutableStack<int> password)
	{
		while (true)
		{
			password = IncrementPassword(password);
			yield return password;
		}
	}

	private static bool ContainsStrictlyIncreasingTriplet(IEnumerable<int> password) =>
		password.Window(3)
			.Any(w => w[0] - 1 == w[1] && w[1] - 1 == w[2]);

	private static bool ContainsTwoDuplicates(IEnumerable<int> password) =>
		password.Lead(1)
			.Where(x => x.current == x.lead)
			.Distinct()
			.Count() > 1;

	private static IEnumerable<ImmutableStack<int>> GetPasswords(ImmutableStack<int> password) =>
		GetIncrementingPasswords(password)
			.Where(p =>
				ContainsStrictlyIncreasingTriplet(p) &&
				ContainsTwoDuplicates(p));

	private static string PassAsString(IImmutableStack<int> password) =>
		string.Join("", password.Reverse().Select(i => (char)(i + 'a')));

	public (string, string) Solve(PuzzleInput input)
	{
		var stack = ImmutableStack<int>.Empty;
		foreach (var c in input.Bytes.AsSpan()[..^1])
			stack = stack.Push(c - 'a');

		var passwords = GetPasswords(stack)
			.Take(2)
			.Select(PassAsString)
			.ToList();

		return (
			passwords[0],
			passwords[1]);
	}
}
