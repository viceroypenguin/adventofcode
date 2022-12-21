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

				var (known, unknown, flag) = cache[m.Left] != null
					? (cache[m.Left]!.Value, m.Right, false)
					: (cache[m.Right]!.Value, m.Left, true);

				var v = (m.Operation, flag) switch
				{
					(Operation.Constant, _) => throw new InvalidOperationException("Can't be unknown..."),
					(Operation.Add, _) => val - known,
					(Operation.Subtract, false) => known - val,
					(Operation.Subtract, true) => val + known,
					(Operation.Multiply, _) => val / known,
					(Operation.Divide, false) => known / val,
					(Operation.Divide, true) => val * known,
				};
				(monkey, val) = (unknown, v);
			}
		}

		var part2 = Solve().ToString();
		return (part1, part2);
	}
}
