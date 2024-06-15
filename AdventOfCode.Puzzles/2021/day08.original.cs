namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 8, CodeType.Original)]
public class Day_08_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = input.Lines
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

		var part2 = input.Lines
			// get the numbers
			.Select(ParseNumber)
			// sum them up
			.Sum()
			.ToString();

		return (part1, part2);
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
		var f = one.Except([c]).Single();

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
				.Where(n => n.Item.Length == x.Length
					&& !n.Item.Except(x).Any()
					&& !x.Except(n.Item).Any())
				// only one, right??
				.Single()
				// which one was it?
				.Index)
			// start with zero.
			// for each number n, i = i * 10 + n
			// return i
			.Aggregate(0, (i, n) => (i * 10) + n);
	}
}
