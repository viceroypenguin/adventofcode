namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 01, CodeType.Fastest)]
public class Day_01_Fastest : IPuzzle<(int, int, int)>
{
	public (int, int, int) Parse(PuzzleInput input)
	{
		Span<int> numbers = stackalloc int[3];
		var elf = 0;

		var span = new ReadOnlySpan<byte>(input.Bytes);
		for (int i = 0; i < span.Length;)
		{
			if (span[i] == '\n')
			{
				if (elf > numbers[2])
					numbers[2] = elf;
				if (elf > numbers[1])
					(numbers[1], numbers[2]) =
						(elf, numbers[1]);
				if (elf > numbers[0])
					(numbers[0], numbers[1]) =
						(elf, numbers[0]);

				elf = 0;
				i++;
			}

			var (value, numChars) = span[i..].AtoI();
			i += numChars + 1;

			elf += value;
		}

		return (numbers[0], numbers[1], numbers[2]);
	}

	public string Part1((int, int, int) input) =>
		input.Item1.ToString();
	public string Part2((int, int, int) input) =>
		(input.Item1 + input.Item2 + input.Item3)
			.ToString();
}
