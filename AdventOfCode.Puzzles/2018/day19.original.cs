using System.Linq.Expressions;

namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 19, CodeType.Original)]
public class Day_19_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Lines;
		var ipRegister = Convert.ToInt32(data.First()[4..]);

		var instructions = data.Skip(1)
			.Select(l => l.Split())
			.Select(l => new
			{
				inst = l[0],
				a = Convert.ToInt32(l[1]),
				b = Convert.ToInt32(l[2]),
				c = Convert.ToInt32(l[3]),
			})
			.ToList();

		// specific optimization for algorithm in day19 code
		instructions[9] = new { inst = "seti", a = 36, b = 5, c = 2, };
		instructions[10] = new { inst = "addi", a = 1, b = 0, c = 1, };
		instructions.Add(new { inst = "mulr", a = 4, b = 5, c = 1, });
		instructions.Add(new { inst = "gtrr", a = 1, b = 3, c = 1, });
		instructions.Add(new { inst = "addr", a = 2, b = 1, c = 2, });
		instructions.Add(new { inst = "seti", a = 2, b = 0, c = 2, });
		instructions.Add(new { inst = "seti", a = 11, b = 0, c = 2, });

		var registers = new[]
		{
				Expression.Parameter(typeof(int), "r0"),
				Expression.Variable(typeof(int), "r1"),
				Expression.Variable(typeof(int), "r2"),
				Expression.Variable(typeof(int), "r3"),
				Expression.Variable(typeof(int), "r4"),
				Expression.Variable(typeof(int), "r5"),
			};

		var labels = Enumerable.Range(0, instructions.Count + 1)
			.Select(i => Expression.Label($"l{i}"))
			.ToArray();
		var returnLabel = Expression.Label(typeof(int), "ret");

		Expression GetRegisterValue(int reg, int ip) =>
			reg == ipRegister
				? Expression.Constant(ip)
				: registers[reg];

		var aluOpcodes = new Dictionary<string, Func<int, int, int, Expression>>()
		{
			["addr"] = (a, b, ip) => Expression.Add(GetRegisterValue(a, ip), GetRegisterValue(b, ip)),
			["addi"] = (a, b, ip) => Expression.Add(GetRegisterValue(a, ip), Expression.Constant(b)),
			["mulr"] = (a, b, ip) => Expression.Multiply(GetRegisterValue(a, ip), GetRegisterValue(b, ip)),
			["muli"] = (a, b, ip) => Expression.Multiply(GetRegisterValue(a, ip), Expression.Constant(b)),
			["banr"] = (a, b, ip) => Expression.And(GetRegisterValue(a, ip), GetRegisterValue(b, ip)),
			["bani"] = (a, b, ip) => Expression.And(GetRegisterValue(a, ip), Expression.Constant(b)),
			["borr"] = (a, b, ip) => Expression.Or(GetRegisterValue(a, ip), GetRegisterValue(b, ip)),
			["bori"] = (a, b, ip) => Expression.Or(GetRegisterValue(a, ip), Expression.Constant(b)),
			["setr"] = (a, b, ip) => GetRegisterValue(a, ip),
			["seti"] = (a, b, ip) => Expression.Constant(a),
		};

		var condOpcodes = new Dictionary<string, Func<int, int, int, Expression>>()
		{
			["gtrr"] = (a, b, ip) => Expression.GreaterThan(GetRegisterValue(a, ip), GetRegisterValue(b, ip)),
			["gtri"] = (a, b, ip) => Expression.GreaterThan(GetRegisterValue(a, ip), Expression.Constant(b)),
			["gtir"] = (a, b, ip) => Expression.GreaterThan(Expression.Constant(a), GetRegisterValue(b, ip)),
			["eqrr"] = (a, b, ip) => Expression.Equal(GetRegisterValue(a, ip), GetRegisterValue(b, ip)),
			["eqri"] = (a, b, ip) => Expression.Equal(GetRegisterValue(a, ip), Expression.Constant(b)),
			["eqir"] = (a, b, ip) => Expression.Equal(Expression.Constant(a), GetRegisterValue(b, ip)),
		};

		var expressions = new List<Expression>();
		for (var i = 0; i < instructions.Count; i++)
		{
			var inst = instructions[i];
			if (inst.c == ipRegister)
			{
				if (inst.inst == "mulr") // by convention
				{
					expressions.Add(Expression.Label(labels[i]));
					expressions.Add(
						Expression.Return(
							returnLabel,
							registers[0]));
				}
				else if (inst.inst == "seti")
				{
					expressions.Add(Expression.Label(labels[i]));
					expressions.Add(
						Expression.Goto(labels[inst.a + 1]));
				}
				else if (inst.inst == "addi")
				{
					if (inst.a != ipRegister)
						throw new InvalidOperationException("Dunno what to do here yet.");

					expressions.Add(Expression.Label(labels[i]));
					expressions.Add(
						Expression.Goto(labels[inst.b + i + 1]));
				}
				else if (inst.inst == "addr")
				{
					var prevInst = instructions[i - 1];
					if ((prevInst.c == inst.a && inst.b == ipRegister) ||
							(prevInst.c == inst.b && inst.a == ipRegister))
					{
						expressions.Add(Expression.Label(labels[i - 1]));
						expressions.Add(Expression.IfThen(
							condOpcodes[prevInst.inst](prevInst.a, prevInst.b, i - 1),
							Expression.Goto(labels[i + 2])));
					}
					else if ((inst.a == 0 && inst.b == ipRegister) ||
							(inst.b == 0 && inst.a == ipRegister))
					{
						expressions.Add(Expression.Label(labels[i]));
						expressions.Add(Expression.IfThen(
							Expression.Equal(registers[0], Expression.Constant(1)),
							Expression.Goto(labels[i + 2])));
					}
					else
					{
						throw new InvalidOperationException("Dunno what to do here yet.");
					}
				}
			}
			else if (aluOpcodes.TryGetValue(inst.inst, out var value))
			{
				expressions.Add(Expression.Label(labels[i]));
				expressions.Add(
					Expression.Assign(
						registers[inst.c],
						value(inst.a, inst.b, i)));
			}
		}

		expressions.Add(Expression.Label(labels.Last()));
		expressions.Add(Expression.Label(returnLabel, registers[0]));

		var lambda = Expression.Lambda<Func<int, int>>(
				Expression.Block(
					registers.Skip(1),
					expressions),
				registers[0]);

		var func = lambda.Compile();

		var part1 = func(0);
		var part2 = func(1);
		return (part1.ToString(), part2.ToString());
	}
}
