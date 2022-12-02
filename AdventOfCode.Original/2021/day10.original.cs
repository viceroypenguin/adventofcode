using System.Collections;

namespace AdventOfCode;

public class Day_2021_10_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 10;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines()
			// for each line
			.Select(static l =>
			{
				// keep track of what we've seen
				var stack = new Stack<char>();
				foreach (var c in l)
				{
					// if we open a chunk, then keep track of it
					if (c is '{' or '(' or '[' or '<')
						stack.Push(c);
					else
					{
						// ending a chunk - is it the right value?
						var v = (stack.Pop(), c) switch
						{
							// if (), then we're good
							// if {), [), <), then bail
							('(', ')') => -1,
							(_, ')') => 3,

							// etc.
							('[', ']') => -1,
							(_, ']') => 57,

							('{', '}') => -1,
							(_, '}') => 1197,

							('<', '>') => -1,
							(_, '>') => 25137,
						};
						// corrupted? return the value
						if (v != -1)
							return (corrupted: true, value: v);
					}
				}

				// not corrupted, calculate score
				var score = stack
					// i = 0
					// i = i * 5 + chunk value
					// return i
					.Aggregate(
						0L,
						(i, c) => i * 5 + c switch
						{
							'(' => 1,
							'[' => 2,
							'{' => 3,
							'<' => 4,
						});
				return (corrupted: false, value: score);
			})
			.ToList();

		PartA = lines
			// get corrupted lines
			.Where(x => x.corrupted)
			// sum their values
			.Sum(x => x.value)
			.ToString();

		// sort the non-corrupted lines by their score
		var scores = lines
			.Where(x => !x.corrupted)
			.Select(x => x.value)
			.OrderBy(x => x)
			.ToList();

		// get the median value
		PartB = scores[scores.Count / 2].ToString();
	}
}
