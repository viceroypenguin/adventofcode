namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 06, CodeType.Fastest)]
public sealed partial class Day_06_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		var time = 0L;
		var distance = 0L;

		Span<int> times = stackalloc int[4];
		Span<int> distances = stackalloc int[4];

		var line = span[..span.IndexOf((byte)'\n')];
		span = span[(line.Length + 1)..];

		var i = 0;
		while (line.Length > 0)
		{
			line = line[line.IndexOfAnyInRange((byte)'0', (byte)'9')..];
			(times[i++], var n) = line.AtoI();

			for (var j = 0; j < n; j++)
				time = (time * 10) + line[j] - '0';

			line = line[n..];
		}

		line = span[..span.IndexOf((byte)'\n')];
		span = span[(line.Length + 1)..];

		i = 0;
		while (line.Length > 0)
		{
			line = line[line.IndexOfAnyInRange((byte)'0', (byte)'9')..];
			(distances[i++], var n) = line.AtoI();

			for (var j = 0; j < n; j++)
				distance = (distance * 10) + line[j] - '0';

			line = line[n..];
		}

		static long GetWinPossibilities(long time, long distance)
		{
			var radical = Math.Sqrt(((double)time * time) - (4 * distance));
			var root1 = (long)Math.Ceiling((time + radical) / 2);
			var root2 = (long)Math.Floor((time - radical) / 2);
			return root1 - root2 - 1;
		}

		var part1 = 1L;
		for (i = 0; i < 4; i++)
			part1 *= GetWinPossibilities(times[i], distances[i]);
		var part2 = GetWinPossibilities(time, distance);

		return (part1.ToString(), part2.ToString());
	}
}
