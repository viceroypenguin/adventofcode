namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 21, CodeType.Original)]
public partial class Day_21_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var values = new Dictionary<string, Func<decimal>>();
		var l = string.Empty;
		var r = string.Empty;

		foreach (var (name, expr) in input.Lines
			.Select(l => l.Split(": "))
			.Select(l => (name: l[0], expr: l[1])))
		{
			if (name == "root")
			{
				var s = expr.Split();
				l = s[0];
				r = s[^1];
			}

			values[name] = expr.Split() switch
			{
				[var number] when decimal.TryParse(number, out var n) =>
					() => n,

				[var a, "+", var b] =>
					() => values[a]() + values[b](),
				[var a, "*", var b] =>
					() => values[a]() * values[b](),
				[var a, "-", var b] =>
					() => values[a]() - values[b](),
				[var a, "/", var b] =>
					() => values[a]() / values[b](),
				_ => throw new InvalidOperationException(),
			};
		}

		var part1 = values["root"]().ToString();

		// value is constant relative to humn, so cache
		var ri = values[r]();

		// starting point
		var i = values["humn"]();

		// build up scale
		var lo = i;
		var hi = i;
		while (true)
		{
			values["humn"] = () => i;
			if (values[l]() - ri < 0)
				break;

			lo = i;
			hi = i *= 2;
		}
		
		// binary search down
		i = (hi + lo) / 2;
		while (true)
		{
			values["humn"] = () => i;
			var diff = values[l]() - ri;
			if (diff == 0)
			{
				var part2 = i.ToString();
				return (part1, part2);
			}

			if (diff < 0)
				hi = i;
			else
				lo = i;

			i = (hi + lo) / 2;
		}
	}
}
