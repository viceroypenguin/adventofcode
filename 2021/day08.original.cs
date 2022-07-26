using System.Collections;

namespace AdventOfCode;

public class Day_2021_08_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 8;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		PartA = input.GetLines()
			// only care about after the bar
			.Select(l => l.Split(" | ")[1])
			// break up the displays
			// and flatten to single long list
			.SelectMany(l => l.Split())
			// where distinct number of bars are lit up
			.Where(s => s.Length is 2 or 3 or 4 or 7)
			// how many of these are there?
			.Count()
			.ToString();

		PartB = input.GetLines()
			// get the numbers
			.Select(ParseNumber)
			// sum them up
			.Sum()
			.ToString();
	}

	private static int ParseNumber(string line)
	{
		var split = line.Split(" | ");
		var input = split[0].Split();

		// these four are unique
		var one = input.Single(x => x.Length == 2);
		var four = input.Single(x => x.Length == 4);
		var seven = input.Single(x => x.Length == 3);
		var eight = input.Single(x => x.Length == 7);

		// nine has one lit up besides what we know
		var nine = input.Single(x =>
			x.Length == 6
			&& x.Except(seven).Except(four).Count() == 1);
		// of zero/six, only six is distinct from one
		var six = input.Single(x =>
			x.Length == 6
			&& x != nine
			&& one.Except(x).Count() == 1);
		// only six digit left
		var zero = input.Single(x =>
			x.Length == 6
			&& x != nine
			&& x != six);

		// which bars we can conclusively identify
		// only matters for getting last three
		var e = eight.Except(nine).Single();
		var c = eight.Except(six).Single();
		var f = one.Except(new[] { c }).Single();

		// of the five, does not contain topright and bottomleft
		var five = input.Single(x =>
			x.Length == 5
			&& !x.Contains(c)
			&& !x.Contains(e));
		// not five, but contains topright and not bottomright
		var two = input.Single(x =>
			x.Length == 5
			&& x != five
			&& x.Contains(c)
			&& !x.Contains(f));
		// only five bar number left
		var three = input.Single(x =>
			x.Length == 5
			&& x != five
			&& x != two);

		// for easy search below
		var numbers = new[]
		{
			zero,
			one,
			two,
			three,
			four,
			five,
			six,
			seven,
			eight,
			nine,
		};

		return split[1].Split()
			// for each string
			.Select(x => numbers
				// keep track of where in the array
				.Index()
				// filter to exact out of order match
				.Where(n => n.item.Length == x.Length
					&& !n.item.Except(x).Any()
					&& !x.Except(n.item).Any())
				// only one, right??
				.Single()
				// which one was it?
				.index)
			// start with zero.
			// for each number n, i = i * 10 + n
			// return i
			.Aggregate(0, (i, n) => i * 10 + n);
	}
}
