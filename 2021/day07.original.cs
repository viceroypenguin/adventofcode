using System.Collections;

namespace AdventOfCode;

public class Day_2021_07_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 7;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var crabs = input.GetString()
			.Split(',')
			.Select(int.Parse)
			.ToList();

		// start with all possible positions
		PartA = Enumerable.Range(0, crabs.Max())
			// for each position, get the sum of the
			// absolute difference between each crab
			// and that position
			.Select(c => crabs.Sum(c2 => Math.Abs(c2 - c)))
			// get the minimum total sum
			.Min()
			.ToString();

		// start with all possible positions
		PartB = Enumerable.Range(0, crabs.Max())
			// for each position, get the sum of the fuel used
			.Select(c => crabs
				// absolute difference
				.Select(c2 => Math.Abs(c2 - c))
				// fuel: sum of numbers 1..n
				.Sum(n => n * (n + 1) / 2))
			// minimum total sum
			.Min()
			.ToString();
	}
}
