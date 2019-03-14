using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
	public class Day_2017_18_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 18;
		public override CodeType CodeType => CodeType.Original;

		class Instruction
		{
			public string Operation { get; set; }
			public string Source { get; set; }
			public string Destination { get; set; }
		}

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var regex = new Regex(@"^(?<inst>snd|set|add|mul|mod|rcv|jgz) (?<dst>\w|-?\d+)( (?<src>\w|-?\d+))?$", RegexOptions.Compiled);
			var instructions = input.GetLines()
					.Select(inst => regex.Match(inst))
					.Select(m => new Instruction
					{
						Operation = m.Groups["inst"].Value,
						Source = m.Groups["src"].Value,
						Destination = m.Groups["dst"].Value,
					})
					.ToList();

			PartA(instructions);
			// inconsistent deadlock...
			// PartB(instructions);
		}

		void PartA(IList<Instruction> input)
		{
			var registers = new Dictionary<string, long>();
			long getRegister(string r) => registers.ContainsKey(r) ? registers[r] : 0;
			long getValue(string src)
			{
				if (int.TryParse(src, out var x)) return x;
				return getRegister(src);
			}

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
								getValue(instruction.Source);
							break;
						}

					case "snd":
						{
							sound = getValue(instruction.Destination);
							break;
						}

					case "rcv":
						{
							var value = getValue(instruction.Destination);
							if (value != 0)
								goto @out;
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

@out:
			Dump('A', sound);
		}

		void PartB(IList<Instruction> input)
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
	}
}
