namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 15, CodeType.Original)]
public class Day_15_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var numbers = input.Text
			.Split(',')
			.Select(int.Parse)
			.ToArray();

		var spokenTimes = new int[30_000_000];
		Array.Fill(spokenTimes, -1);

		var i = 1;
		for (; i < numbers.Length + 1; i++)
			spokenTimes[numbers[i - 1]] = i;

		var curNumber = 0;
		for (; i < 2020; i++)
		{
			var prevTime = spokenTimes[curNumber];
			spokenTimes[curNumber] = i;
			curNumber = prevTime != -1 ? i - prevTime : 0;
		}

		var part1 = curNumber.ToString();

		for (; i < 30_000_000; i++)
		{
			var prevTime = spokenTimes[curNumber];
			spokenTimes[curNumber] = i;
			curNumber = prevTime != -1 ? i - prevTime : 0;
		}

		var part2 = curNumber.ToString();

		return (part1, part2);
	}
}
