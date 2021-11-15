using System.Numerics;

namespace AdventOfCode;

public class Day_2019_16_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 16;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var signal = input.Where(i => i != '\n').Select(i => i - 0x30).ToArray();

		var transforms = BuildTransforms(signal.Length);

		for (int phase = 0; phase < 100; phase++)
			signal = Enumerable.Range(0, signal.Length)
				.Select(i => Math.Abs(signal.Zip(transforms[i], (s, t) => s * t).Sum()) % 10)
				.ToArray();

		PartA = string.Join("", signal.Take(8));

		signal = input.Where(i => i != '\n').Select(i => i - 0x30).ToArray();

		var phases = 100;
		var totalLength = signal.Length * 10000;
		var skip = Enumerable.Range(0, 7).Aggregate(0, (n, i) => n * 10 + signal[i]);
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

		PartB = string.Join("", final);
	}

	private static readonly int[] BasePattern = { 0, 1, 0, -1 };
	private int[][] BuildTransforms(int count) =>
		Enumerable.Range(1, count)
			.Select(i => Enumerable.Range(0, count)
				.Select(j => BasePattern[((j + 1) / i) % 4])
				.ToArray())
			.ToArray();
}
