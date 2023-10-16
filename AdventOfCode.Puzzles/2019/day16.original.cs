using System.Numerics;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 16, CodeType.Original)]
public class Day_16_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var signal = input.Bytes.Where(i => i != '\n').Select(i => i - 0x30).ToArray();

		var transforms = BuildTransforms(signal.Length);

		for (var phase = 0; phase < 100; phase++)
		{
			signal = Enumerable.Range(0, signal.Length)
				.Select(i => Math.Abs(signal.Zip(transforms[i], (s, t) => s * t).Sum()) % 10)
				.ToArray();
		}

		var part1 = string.Join("", signal.Take(8));

		signal = input.Bytes.Where(i => i != '\n').Select(i => i - 0x30).ToArray();

		var phases = 100;
		var totalLength = signal.Length * 10000;
		var skip = Enumerable.Range(0, 7).Aggregate(0, (n, i) => (n * 10) + signal[i]);
		var remainder = totalLength - skip;

		// https://math.stackexchange.com/questions/234304/sum-of-the-sum-of-the-sum-of-the-first-n-natural-numbers
		var factors = Enumerable.Range(1, remainder - 1)
			.Scan(new BigInteger(1), (factor, i) => factor * (i + (phases - 1)) / i)
			.Select(i => (int)(i % 10))
			.ToList();

		var final = Enumerable.Range(0, 8)
			.Select(i => factors
				.SkipLast(i)
				.Select((f, j) => f * signal[(skip + i + j) % signal.Length])
				.Sum() % 10)
			.ToArray();

		var part2 = string.Join("", final);
		return (part1, part2);
	}

	private static readonly int[] BasePattern = [0, 1, 0, -1];
	private static int[][] BuildTransforms(int count) =>
		Enumerable.Range(1, count)
			.Select(i => Enumerable.Range(0, count)
				.Select(j => BasePattern[(j + 1) / i % 4])
				.ToArray())
			.ToArray();
}
