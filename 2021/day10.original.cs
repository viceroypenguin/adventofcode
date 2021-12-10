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

		var lines = input.GetLines();

		PartA = lines
			.Select(static l =>
			{
				var stack = new Stack<char>();
				foreach (var c in l)
				{
					if (c is '{' or '(' or '[' or '<')
						stack.Push(c);
					else
					{
						var o = stack.Pop();
						var v = (o, c) switch
						{
							('(', ')') => -1,
							(_, ')') => 3,

							('[', ']') => -1,
							(_, ']') => 57,

							('{', '}') => -1,
							(_, '}') => 1197,

							('<', '>') => -1,
							(_, '>') => 25137,
						};
						if (v != -1)
							return v;
					}
				}

				return 0;
			})
			.Sum()
			.ToString();

		var scores = lines
			.Select(static l =>
			{
				var stack = new Stack<char>();
				foreach (var c in l)
				{
					if (c is '{' or '(' or '[' or '<')
						stack.Push(c);
					else
					{
						var o = stack.Pop();
						var v = (o, c) switch
						{
							('(', ')') => -1,
							(_, ')') => 3,

							('[', ']') => -1,
							(_, ']') => 57,

							('{', '}') => -1,
							(_, '}') => 1197,

							('<', '>') => -1,
							(_, '>') => 25137,
						};
						if (v != -1)
							return 0;
					}
				}

				return stack
					.Aggregate(
						0L,
						(i, c) => i * 5 + c switch
						{
							'(' => 1,
							'[' => 2,
							'{' => 3,
							'<' => 4,
						});
			})
			.Where(x => x != 0)
			.OrderBy(x => x)
			.ToList();

		PartB = scores[scores.Count / 2].ToString();
	}
}
