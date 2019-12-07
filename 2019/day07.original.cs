using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2019_07_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 7;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var instructions = input.GetString()
				.Split(',')
				.Select(s => Convert.ToInt32(s))
				.ToList();

			DoPartA(instructions).GetAwaiter().GetResult();
			DoPartB(instructions).GetAwaiter().GetResult();
		}

		async Task DoPartA(List<int> instructions)
		{
			var max = 0;
			for (int a = 0; a <= 4; a++)
			{
				var bufferIn = new BufferBlock<int>();
				bufferIn.Post(a); bufferIn.Post(0);

				var bufferOut = new BufferBlock<int>();
				await RunProgram(instructions.ToList(), bufferIn, bufferOut);
				var aOut = bufferOut.Receive();

				for (int b = 0; b <= 4; b++)
				{
					if (b == a) continue;

					bufferIn.Post(b); bufferIn.Post(aOut);
					await RunProgram(instructions.ToList(), bufferIn, bufferOut);
					var bOut = bufferOut.Receive();

					for (int c = 0; c <= 4; c++)
					{
						if (c == a || c == b) continue;

						bufferIn.Post(c); bufferIn.Post(bOut);
						await RunProgram(instructions.ToList(), bufferIn, bufferOut);
						var cOut = bufferOut.Receive();

						for (int d = 0; d <= 4; d++)
						{
							if (d == c || d == b || d == a) continue;

							bufferIn.Post(d); bufferIn.Post(cOut);
							await RunProgram(instructions.ToList(), bufferIn, bufferOut);
							var dOut = bufferOut.Receive();

							for (int e = 0; e <= 4; e++)
							{
								if (e == a || e == b || e == c || e == d) continue;

								bufferIn.Post(e); bufferIn.Post(dOut);
								await RunProgram(instructions.ToList(), bufferIn, bufferOut);
								var eOut = bufferOut.Receive();

								if (eOut > max)
								{
									max = eOut;
								}
							}
						}
					}
				}
			}

			PartA = max.ToString();
		}

		async Task DoPartB(List<int> instructions)
		{
			var max = 0;
			for (int a = 5; a <= 9; a++)
			{
				for (int b = 5; b <= 9; b++)
				{
					if (b == a) continue;

					for (int c = 5; c <= 9; c++)
					{
						if (c == a || c == b) continue;

						for (int d = 5; d <= 9; d++)
						{
							if (d == c || d == b || d == a) continue;

							for (int e = 5; e <= 9; e++)
							{
								if (e == a || e == b || e == c || e == d) continue;

								var buffers = Enumerable.Range(0, 5).Select(i => new BufferBlock<int>()).ToList();
								buffers[0].Post(a);
								buffers[0].Post(0);
								buffers[1].Post(b);
								buffers[2].Post(c);
								buffers[3].Post(d);
								buffers[4].Post(e);

								await Task.WhenAll(
									RunProgram(instructions.ToList(), buffers[0], buffers[1]),
									RunProgram(instructions.ToList(), buffers[1], buffers[2]),
									RunProgram(instructions.ToList(), buffers[2], buffers[3]),
									RunProgram(instructions.ToList(), buffers[3], buffers[4]),
									RunProgram(instructions.ToList(), buffers[4], buffers[0]));

								var output = buffers[0].Receive();
								if (output > max)
								{
									max = output;
								}
							}
						}
					}
				}
			}

			PartB = max.ToString();
		}

		async Task RunProgram(List<int> instructions, BufferBlock<int> inputs, BufferBlock<int> output)
		{
			var ip = 0;
			while (ip < instructions.Count && instructions[ip] != 99)
			{
				int GetFirstParameterMode(int instruction) =>
					(instruction / 100) % 10;
				int GetSecondParameterMode(int instruction) =>
					instruction / 1000;
				int GetValue(int mode, int value) =>
					mode != 0 ? value : instructions[value];

				switch (instructions[ip] % 100)
				{
					case 1:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							instructions[instructions[ip + 3]] = num1 + num2;
							ip += 4;
							break;
						}
					case 2:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							instructions[instructions[ip + 3]] = num1 * num2;
							ip += 4;
							break;
						}

					case 3:
						{
							instructions[instructions[ip + 1]] = await inputs.ReceiveAsync();
							ip += 2;
							break;
						}

					case 4:
						{
							output.Post(GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]));
							ip += 2;
							break;
						}

					case 5:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							ip = num1 == 0 ? ip + 3 : num2;
							break;
						}

					case 6:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							ip = num1 != 0 ? ip + 3 : num2;
							break;
						}

					case 7:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							instructions[instructions[ip + 3]] = num1 < num2 ? 1 : 0;
							ip += 4;
							break;
						}

					case 8:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							instructions[instructions[ip + 3]] = num1 == num2 ? 1 : 0;
							ip += 4;
							break;
						}
				}
			}
		}
	}
}
