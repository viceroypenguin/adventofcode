namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 1, CodeType.Fastest)]
public class Day_01_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<int> numbers = stackalloc int[4];
		numbers[0] = numbers[1] = numbers[2] = numbers[3] = int.MaxValue;
		int numA = 0, numB = 0;

		var span = new ReadOnlySpan<byte>(input.Bytes);
		for (int i = 0; i < span.Length;)
		{
			var (value, numChars) = span[i..].AtoI();
			i += numChars + 1;

			(numbers[0], numbers[1], numbers[2], numbers[3]) =
				(numbers[1], numbers[2], numbers[3], value);

			if (numbers[3] > numbers[2]) numA++;
			if (numbers[3] > numbers[0]) numB++;
		}

		return (numA.ToString(), numB.ToString());
	}
}
