using System.Diagnostics;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 18, CodeType.Original)]
public partial class Day_18_Original : IPuzzle
{
	[GeneratedRegex("^(?<inst>snd|set|add|mul|mod|rcv|jgz) (?<dst>\\w|-?\\d+)( (?<src>\\w|-?\\d+))?$", RegexOptions.Compiled)]
	private static partial Regex InstructionRegex();

	private sealed class Instruction
	{
		public string Operation { get; set; }
		public string Source { get; set; }
		public string Destination { get; set; }
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();
		var instructions = input.Lines
			.Select(inst => regex.Match(inst))
			.Select(m => new Instruction
			{
				Operation = m.Groups["inst"].Value,
				Source = m.Groups["src"].Value,
				Destination = m.Groups["dst"].Value,
			})
			.ToList();

		return (
			DoPartA(instructions).ToString(),
			string.Empty);
		// inconsistent deadlock...
		// PartB(instructions);
	}

	private static long DoPartA(IList<Instruction> input)
	{
		var registers = new Dictionary<string, long>();
		long GetValue(string src) => int.TryParse(src, out var x) ? x : registers.GetValueOrDefault(src);

		var ip = 0;
		var sound = 0L;

		while (ip < input.Count)
		{
			var instruction = input[ip];
			switch (instruction.Operation)
			{
				case "set":
				{
					registers[instruction.Destination] =
						GetValue(instruction.Source);
					break;
				}

				case "snd":
				{
					sound = GetValue(instruction.Destination);
					break;
				}

				case "rcv":
				{
					var value = GetValue(instruction.Destination);
					if (value != 0)
						return sound;
					break;
				}

				case "add":
				{
					var register = registers.GetValueOrDefault(instruction.Destination);
					register += GetValue(instruction.Source);
					registers[instruction.Destination] = register;
					break;
				}

				case "mul":
				{
					var register = registers.GetValueOrDefault(instruction.Destination);
					register *= GetValue(instruction.Source);
					registers[instruction.Destination] = register;
					break;
				}

				case "mod":
				{
					var register = registers.GetValueOrDefault(instruction.Destination);
					register %= GetValue(instruction.Source);
					registers[instruction.Destination] = register;
					break;
				}

				case "jgz":
				{
					var value = GetValue(instruction.Destination);
					if (value > 0)
					{
						ip += (int)GetValue(instruction.Source);
						continue;
					}
					break;
				}

				default:
					throw new UnreachableException();
			}

			ip++;
		}

		return -1;
	}

#if false // preserved for shameful posterity
	private long DoPartB(IList<Instruction> input)
	{
		var queues = Enumerable.Repeat(0, 2).Select(_ => new BlockingCollection<long>()).ToList();
		var sendCount = new int[2];
		var isWaiting = new bool[2];

		var task0 = Task.Run(() => Process(0));
		var task1 = Task.Run(() => Process(1));
		Task.WaitAny(task0, task1);

		Dump('B', sendCount);

		void Process(int id)
		{
			var registers = new Dictionary<string, long>();
			registers["p"] = id;
			long getRegister(string r) => registers.ContainsKey(r) ? registers[r] : 0;
			long getValue(string src)
			{
				if (int.TryParse(src, out var x)) return x;
				return getRegister(src);
			}

			var ip = 0;

			while (ip < input.Count)
			{
				var instruction = input[ip];
				switch (instruction.Operation)
				{
					case "set":
					{
						registers[instruction.Destination] =
							getValue(instruction.Source);
						break;
					}

					case "snd":
					{
						isWaiting[1 - id] = false;
						queues[id].Add(getValue(instruction.Destination));
						sendCount[id]++;
						break;
					}

					case "rcv":
					{
						if (isWaiting[1 - id] && queues[1 - id].Count == 0)
							return;

						if (queues[1 - id].Count == 0)
							isWaiting[id] = true;

						var value = queues[1 - id].Take();
						isWaiting[id] = false;
						registers[instruction.Destination] = value;
						break;
					}

					case "add":
					{
						var register = getRegister(instruction.Destination);
						register += getValue(instruction.Source);
						registers[instruction.Destination] = register;
						break;
					}

					case "mul":
					{
						var register = getRegister(instruction.Destination);
						register *= getValue(instruction.Source);
						registers[instruction.Destination] = register;
						break;
					}

					case "mod":
					{
						var register = getRegister(instruction.Destination);
						register %= getValue(instruction.Source);
						registers[instruction.Destination] = register;
						break;
					}

					case "jgz":
					{
						var value = getValue(instruction.Destination);
						if (value > 0)
						{
							ip += (int)getValue(instruction.Source);
							continue;
						}
						break;
					}
				}

				ip++;
			}
		}
	}
#endif
}
