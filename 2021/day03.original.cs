using System.Collections;

namespace AdventOfCode;

public class Day_2021_03_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 3;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines();

		DoPartA(lines);
		DoPartB(lines);
	}

	private void DoPartA(string[] lines)
	{
		var (epsilon, gamma) = lines
			// start w/ array of zeros for each column
			.Aggregate(new int[lines[0].Length], (x, p) =>
				// for each line in input
				// match pair-wise each character to count in column
				// then increment count in column if char is '1'
				// wasteful - new int[] for each line in input
				x.Zip(p, (a, b) => a + (b == '1' ? 1 : 0)).ToArray())
			// build up epsilon and gamma from the completed column counts
			.Aggregate((epsilon: 0, gamma: 0), (x, cnt) =>
				// in both cases, shift left, to get the new bit
				cnt < (lines.Length / 2)
					// if column count < half, then epsilon gets a 1
					? ((x.epsilon << 1) + 1, x.gamma << 1)
					// otherwise, gamma gets a 1
					: (x.epsilon << 1, (x.gamma << 1) + 1));

		PartA = (epsilon * gamma).ToString();
	}

	private void DoPartB(string[] lines)
	{
		// technically O(n^2) algorithm;
		// not enough data to justify improving further
		
		// start with full list
		var tmp = lines.ToList();
		// we're narrowing down to single element
		for (int i = 0; tmp.Count != 1; i++)
		{
			// how many rows have a 1 in the i-th column?
			var cnt = tmp.Count(s => s[i] == '1');
			// if we're >= exactly half (not integer half)
			if (cnt >= tmp.Count / 2.0)
				// keep track of 1's, so remove 0's
				tmp.RemoveAll(x => x[i] == '0');
			else
				// vice-versa
				tmp.RemoveAll(x => x[i] == '1');
		}
		// convert number to integer
		var oxygenCount = Convert.ToInt32(tmp[0], 2);

		// start with full list
		tmp = lines.ToList();
		// we're narrowing down to single element
		for (int i = 0; tmp.Count != 1; i++)
		{
			// how many rows have a 0 in the i-th column?
			var cnt = tmp.Count(s => s[i] == '0');
			// if we're <= exactly half (not integer half)
			// technically should be the same here, but for consistency...
			if (cnt <= tmp.Count / 2.0)
				// keep track of 0's, so remove 1's
				tmp.RemoveAll(x => x[i] == '1');
			else
				// vice-versa
				tmp.RemoveAll(x => x[i] == '0');
		}
		// convert number to integer
		var coScrub = Convert.ToInt32(tmp[0], 2);

		PartB = (oxygenCount * coScrub).ToString();
	}
}
