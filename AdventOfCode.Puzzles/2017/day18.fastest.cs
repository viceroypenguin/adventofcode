using System.Diagnostics;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 18, CodeType.Fastest)]
public class Day_18_Fastest : IPuzzle
{
	private struct Instruction
	{
		public int Operation { get; set; }
		public int Source { get; set; }
		public int Destination { get; set; }
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = ParseInstructions(input.Bytes);

		return (
			DoPartA(instructions).ToString(),
			DoPartB(instructions).ToString());
	}

	private static Instruction[] ParseInstructions(byte[] input)
	{
		var tmp = new Instruction[input.Length / 8];
		var count = 0;

		ref var l = ref tmp[count];
		int n = 0, state = 0;
		var neg = false;
		for (var i = 0; i < input.Length; i++)
		{
			var c = input[i];
			if (c == '\n')
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
			{
			}
			else if (c == '-')
			{
				neg = true;
			}
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
				n = (n * 10) + c - '0';
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
				{
					n = c - 'a';
				}
			}
		}

		Array.Resize(ref tmp, count);
		return tmp;
	}

	private static long DoPartA(Instruction[] instructions)
	{
		var registers = new long[26];
		var cnt = (uint)instructions.Length;

		var snd = 0L;
		for (var i = 0L; i >= 0 && i < cnt; i++)
		{
			var ins = instructions[i];
			var source = (ins.Operation & 0x200) != 0 ? ins.Source : registers[ins.Source];
			switch (ins.Operation & 0xff)
			{
				case 'e': registers[ins.Destination] = source; break;
				case 'd': registers[ins.Destination] += source; break;
				case 'u': registers[ins.Destination] *= source; break;
				case 'o': registers[ins.Destination] %= source; break;

				case 'g':
				{
					var cmp = (ins.Operation & 0x100) != 0 ? ins.Destination : registers[ins.Destination];
					if (cmp > 0)
						i += source - 1;
					break;
				}

				case 'n': snd = source; break;
				case 'c':
				{
					if (source != 0)
					{
						return snd;
					}

					break;
				}

				default:
					throw new UnreachableException();
			}
		}

		return -1;
	}

	private static long DoPartB(Instruction[] instructions)
	{
		var cnt = (uint)instructions.Length;

		Span<long> ips = stackalloc long[2];
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
		var i = ips[cpu];

		for (; i >= 0 && i < cnt; i++)
		{
			var ins = instructions[i];
			var source = (ins.Operation & 0x200) != 0 ? ins.Source : registers[ins.Source];
			switch (ins.Operation & 0xff)
			{
				case 'e': registers[ins.Destination] = source; break;
				case 'd': registers[ins.Destination] += source; break;
				case 'u': registers[ins.Destination] *= source; break;
				case 'o': registers[ins.Destination] %= source; break;

				case 'g':
				{
					var cmp = (ins.Operation & 0x100) != 0 ? ins.Destination : registers[ins.Destination];
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
							return backs[0];
						}

						ips[cpu] = i;

						cpu = 1 - cpu;
						registers = registerSets[cpu];
						i = ips[cpu] - 1;
						continue;
					}

					// source because parsing
					registers[ins.Source] = queues[cpu][fronts[cpu]++];
					break;
				}

				default:
					throw new UnreachableException();
			}
		}

		return -1;
	}
}
