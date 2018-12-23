<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
  <Namespace>System.Reflection.Emit</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day21.input.txt"));
var ipRegister = Convert.ToInt32(input.First().Substring(4));

var instructions = input.Skip(1)
	.Select(l => l.Split())
	.Select(l => new
	{
		inst = l[0],
		a = Convert.ToInt32(l[1]),
		b = Convert.ToInt32(l[2]),
		c = Convert.ToInt32(l[3]),
	})
	.ToList();

// rewrite elf to signal w/ register value
instructions[28] = new { inst = "sigr", a = 4, b = 0, c = 2, };

var registers = new[]
{
	Expression.Parameter(typeof(int), "r0"),
	Expression.Variable(typeof(int), "r1"),
	Expression.Variable(typeof(int), "r2"),
	Expression.Variable(typeof(int), "r3"),
	Expression.Variable(typeof(int), "r4"),
	Expression.Variable(typeof(int), "r5"),
};
var signalParam = Expression.Parameter(typeof(Func<int, bool>), "signal");

var labels = Enumerable.Range(0, instructions.Count + 1)
	.Select(i => Expression.Label($"l{i}"))
	.ToArray();
var returnLabel = Expression.Label(typeof(int), "ret");

Expression GetRegisterValue(int reg, int ip) =>
	reg == ipRegister
		? (Expression)Expression.Constant(ip)
		: (Expression)registers[reg];

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
	["sigr"] = (a, b, ip) => Expression.Invoke(signalParam, GetRegisterValue(a, ip)),
};

List<Expression> expressions = new List<Expression>();
for (int i = 0; i < instructions.Count; i++)
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
				throw new InvalidOperationException("Dunno what to do here yet.");
		}
	}
	else if (aluOpcodes.ContainsKey(inst.inst))
	{
		expressions.Add(Expression.Label(labels[i]));
		expressions.Add(
			Expression.Assign(
				registers[inst.c],
				aluOpcodes[inst.inst](inst.a, inst.b, i)));
	}
}

expressions.Add(Expression.Label(labels.Last()));
expressions.Add(Expression.Label(returnLabel, registers[0]));

var lambda = Expression.Lambda<Func<int, Func<int, bool>, int>>(
		Expression.Block(
			registers.Skip(1),
			expressions),
		registers[0],
		signalParam);

var func = lambda.Compile();

var list = new List<int>();
bool HandleCall(int val)
{
	if (list.Count == 0)
		val.Dump("Part A");

	if (list.Contains(val))
	{
		list.Last().Dump("Part B");
		return true;
	}
	list.Add(val);
	return false;
}

func(0, HandleCall);
