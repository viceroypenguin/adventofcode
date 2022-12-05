namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 10, CodeType.Original)]
public class Day_10_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var lines = input.Lines
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

		var part1 = lines
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
		var part2 = scores[scores.Count / 2].ToString();

		return (part1, part2);
	}
}
