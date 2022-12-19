namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 16, CodeType.Original)]
public class Day_16_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Lines;

		int[] ParseRegisters(string str) =>
			str.Substring(9, 10)
				.Split(',')
				.Select(s => Convert.ToInt32(s))
				.ToArray();

		var operations = new (HashSet<int> opcodes, Func<int[], int, int, int> method)[]
		{
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] + r[b]),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] + b),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] * r[b]),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] * b),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] & r[b]),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] & b),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] | r[b]),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] | b),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a]),
				(opcodes: new HashSet<int>(), method: (r, a, b) => a),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] > r[b] ? 1 : 0),
				(opcodes: new HashSet<int>(), method: (r, a, b) => a > r[b] ? 1 : 0),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] > b ? 1 : 0),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] == r[b] ? 1 : 0),
				(opcodes: new HashSet<int>(), method: (r, a, b) => a == r[b] ? 1 : 0),
				(opcodes: new HashSet<int>(), method: (r, a, b) => r[a] == b ? 1 : 0),
		};

		var key = data
			.Batch(4)
			.TakeWhile(b => !string.IsNullOrWhiteSpace(b.First()))
			.Select(batch =>
			{
				var before = ParseRegisters(batch.First());
				var after = ParseRegisters(batch.Skip(2).First());
				var instruction = batch.Skip(1).First()
					.Split()
					.Select(s => Convert.ToInt32(s))
					.ToArray();

				if ((instruction[3] == 0 || before[0] == after[0]) &&
					(instruction[3] == 1 || before[1] == after[1]) &&
					(instruction[3] == 2 || before[2] == after[2]) &&
					(instruction[3] == 3 || before[3] == after[3]))
				{
					var possibles = 0;

					foreach (var op in operations)
						if (after[instruction[3]] == op.method(before, instruction[1], instruction[2]))
						{
							possibles++;
							op.opcodes.Add(instruction[0]);
						}

					return possibles;
				}
				else
					return 0;
			})
			.ToList();

		var part1 = key.Count(cnt => cnt >= 3);

		var knownOpcodes = operations
			.Where(o => o.opcodes.Count == 1)
			.SelectMany(o => o.opcodes)
			.ToList();
		var flag = false;
		do
		{
			flag = false;

			foreach (var o in operations)
			{
				var hs = o.opcodes;
				if (hs.Count == 1)
					continue;
				hs.ExceptWith(knownOpcodes);
				if (hs.Count == 1)
					knownOpcodes.Add(hs.Single());
				else
					flag = true;
			}
		} while (flag);

		var opCodes = operations
			.OrderBy(o => o.opcodes.Single())
			.Select(o => o.method)
			.ToArray();

		var program = data
			.Batch(4)
			.SkipWhile(b => !string.IsNullOrWhiteSpace(b.First()))
			.SelectMany(x => x)
			.Skip(2)
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(b => b.Split()
				.Select(x => Convert.ToInt32(x))
				.ToArray())
			.ToArray();

		var registers = new int[] { 0, 0, 0, 0 };

		foreach (var i in program)
			registers[i[3]] = opCodes[i[0]](registers, i[1], i[2]);

		var part2= registers[0];

		return (part1.ToString(), part2.ToString());
	}
}
