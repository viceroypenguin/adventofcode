using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
	public class Day_2017_18_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 18;
		public override CodeType CodeType => CodeType.Fastest;

		struct Instruction
		{
			public int Operation;
			public int Source;
			public int Destination;
		}

		Instruction[] instructions;
		int instructionCount;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			ParseInstructions(input);

			PartA();
			PartB();
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void ParseInstructions(byte[] input)
		{
			var tmp = new Instruction[input.Length / 8];
			var count = 0;

			ref Instruction l = ref tmp[count];
			int n = 0, state = 0;
			bool neg = false;
			for (int i = 0; i < input.Length; i++)
			{
				var c = input[i];
				if (c == '\r')
				{
					if (state == 1)
						l.Operation = (l.Operation & ~0x100) | ((l.Operation & 0x100) << 1);
					l.Source = neg ? -n : n;
					neg = false;
					n = 0;
					state = 0;

					count++;
					l = ref tmp[count];
				}
				else if (c == '\n')
					;
				else if (c == '-')
					neg = true;
				else if (c == ' ')
				{
					if (state == 1)
					{
						state = 2;
						l.Destination = n;
						n = 0;
					}
				}
				else if (c <= '9')
				{
					n = n * 10 + c - '0';
					l.Operation |= state << 8;
				}
				else if (c <= 'z')
				{
					if (state == 0)
					{
						l.Operation = input[i + 1];
						i += 3;
						state = 1;
					}
					else
						n = c - 'a';
				}
			}

			instructions = tmp;
			instructionCount = count;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private unsafe void PartA()
		{
			fixed (Instruction* instructions = this.instructions)
			{
				var end = &instructions[instructionCount];

				var registers = new long[26];
				var snd = 0L;
				for (var i = instructions; i >= instructions && i < end; i++)
				{
					var source = (i->Operation & 0x200) != 0 ? i->Source : registers[i->Source];
					switch (i->Operation & 0xff)
					{
						case 'e': registers[i->Destination] = source; break;
						case 'd': registers[i->Destination] += source; break;
						case 'u': registers[i->Destination] *= source; break;
						case 'o': registers[i->Destination] %= source; break;

						case 'g':
							{
								var cmp = (i->Operation & 0x100) != 0 ? i->Destination : registers[i->Destination];
								if (cmp > 0)
									i += source - 1;
								break;
							}

						case 'n': snd = source; break;
						case 'c':
							{
								if (source != 0)
								{
									Dump('A', snd);
									return;
								}
								break;
							}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private unsafe void PartB()
		{
			fixed (Instruction* instructions = this.instructions)
			{
				var end = &instructions[instructionCount];

				var pointers = new Instruction*[2] { instructions, instructions, };
				var registerSets = new long[2][];
				registerSets[0] = new long[26];
				registerSets[1] = new long[26];

				registerSets[1]['p' - 'a'] = 1;

				var queues = new[]
				{
					new long[16384],
					new long[16384],
				};
				var fronts = new int[2];
				var backs = new int[2];

				var cpu = 0;
				var registers = registerSets[cpu];
				var i = pointers[cpu];

				for (; i >= instructions && i < end; i++)
				{
					var source = (i->Operation & 0x200) != 0 ? i->Source : registers[i->Source];
					switch (i->Operation & 0xff)
					{
						case 'e': registers[i->Destination] = source; break;
						case 'd': registers[i->Destination] += source; break;
						case 'u': registers[i->Destination] *= source; break;
						case 'o': registers[i->Destination] %= source; break;

						case 'g':
							{
								var cmp = (i->Operation & 0x100) != 0 ? i->Destination : registers[i->Destination];
								if (cmp > 0)
									i += source - 1;
								break;
							}

						case 'n': queues[1 - cpu][backs[1 - cpu]++] = source; break;
						case 'c':
							{
								if (fronts[cpu] == backs[cpu])
								{
									if (fronts[1 - cpu] == backs[1 - cpu])
									{
										// deadlock; return. (return 0, since 1 *sends* to 0's queue)
										Dump('B', backs[0]);
										return;
									}

									pointers[cpu] = i;

									cpu = 1 - cpu;
									registers = registerSets[cpu];
									i = pointers[cpu] - 1;
									continue;
								}

								// source because parsing
								registers[i->Source] = queues[cpu][fronts[cpu]++];
								break;
							}
					}
				}
			}
		}
	}
}
