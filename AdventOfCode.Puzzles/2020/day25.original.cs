namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 25, CodeType.Original)]
public class Day_25_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;
		var x = span.AtoI();
		var key1 = x.value;

		span = span[(x.numChars + 1)..];
		x = span.AtoI();
		var key2 = x.value;

		var loopSize = GetLoopSize(key1);
		var eKey = GetKey(key2, loopSize);

		var part1 = eKey.ToString();

		return (part1, string.Empty);
	}

	private static int GetLoopSize(int publicKey)
	{
		var sn = 7L;
		var value = 1L;
		var i = 0;

		while (value != publicKey)
		{
			value = (value * sn) % 20201227;
			i++;
		}
		return i;
	}

	private static int GetKey(int sn, int loopSize)
	{
		var value = 1L;
		for (var i = 0; i < loopSize; i++)
			value = (value * sn) % 20201227;

		return (int)value;
	}
}
