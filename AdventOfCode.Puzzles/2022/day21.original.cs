namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 21, CodeType.Original)]
public partial class Day_21_Original : IPuzzle
{
	private enum Operation { Constant, Add, Subtract, Multiply, Divide, };
	private readonly record struct Monkey(Operation Operation, decimal? Constant = null, string Left = "", string Right = "");
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var values = input.Lines
			.Select(l => l.Split(": "))
			.ToDictionary(
				x => x[0],
				x => x[1].Split() switch
				{
					[var number] => new Monkey(Operation.Constant, decimal.Parse(number)),
					[var a, var o, var b] => new Monkey(
						Operation: o switch
						{
							"+" => Operation.Add,
							"-" => Operation.Subtract,
							"*" => Operation.Multiply,
							"/" => Operation.Divide,
						},
						Left: a,
						Right: b),
				});

		var cache = new Dictionary<string, decimal?>(values.Count);
		decimal? GetValue(string monkey) =>
			cache[monkey] = values[monkey] switch
			{
				(Operation.Constant, var c, _, _) => c,
				(Operation.Add, _, var l, var r) => GetValue(l) + GetValue(r),
				(Operation.Subtract, _, var l, var r) => GetValue(l) - GetValue(r),
				(Operation.Multiply, _, var l, var r) => GetValue(l) * GetValue(r),
				(Operation.Divide, _, var l, var r) => GetValue(l) / GetValue(r),
			};

		var part1 = GetValue("root")!.Value.ToString();

		values["humn"] = new Monkey(Operation.Constant);
		values["root"] = values["root"] with { Operation = Operation.Subtract, };
		// reset cache
		_ = GetValue("root");

		decimal Solve()
		{
			var (monkey, val) = ("root", 0m);
			while (true)
			{
				if (monkey == "humn")
					return val;

				var m = values[monkey];

				if (cache[m.Left] != null)
				{
					var known = cache[m.Left]!.Value;
					var v = m.Operation switch
					{
						Operation.Constant => throw new InvalidOperationException("Can't be unknown..."),
						Operation.Add => val - known,
						Operation.Subtract => known - val,
						Operation.Multiply => val / known,
						Operation.Divide => known / val,
					};
					(monkey, val) = (m.Right, v);
				}
				else
				{
					var known = cache[m.Right]!.Value;
					var v = m.Operation switch
					{
						Operation.Constant => throw new InvalidOperationException("Can't be unknown..."),
						Operation.Add => val - known,
						Operation.Subtract => val + known,
						Operation.Multiply => val / known,
						Operation.Divide => val * known,
					};
					(monkey, val) = (m.Left, v);
				}
			}
		}

		var part2 = Solve().ToString();
		return (part1, part2);
	}
}
