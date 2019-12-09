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
				.Select(long.Parse)
				.ToList();

			DoPartA(instructions).GetAwaiter().GetResult();
			DoPartB(instructions).GetAwaiter().GetResult();
		}

		async Task DoPartA(List<long> instructions)
		{
			var max = 0L;
			for (int a = 0; a <= 4; a++)
			{
				var bufferIn = new BufferBlock<long>();
				bufferIn.Post(a); bufferIn.Post(0);

				var bufferOut = new BufferBlock<long>();
				await new IntCodeComputer(instructions.ToArray(), bufferIn, bufferOut).RunProgram();
				var aOut = bufferOut.Receive();

				for (int b = 0; b <= 4; b++)
				{
					if (b == a) continue;

					bufferIn.Post(b); bufferIn.Post(aOut);
					await new IntCodeComputer(instructions.ToArray(), bufferIn, bufferOut).RunProgram();
					var bOut = bufferOut.Receive();

					for (int c = 0; c <= 4; c++)
					{
						if (c == a || c == b) continue;

						bufferIn.Post(c); bufferIn.Post(bOut);
						await new IntCodeComputer(instructions.ToArray(), bufferIn, bufferOut).RunProgram();
						var cOut = bufferOut.Receive();

						for (int d = 0; d <= 4; d++)
						{
							if (d == c || d == b || d == a) continue;

							bufferIn.Post(d); bufferIn.Post(cOut);
							await new IntCodeComputer(instructions.ToArray(), bufferIn, bufferOut).RunProgram();
							var dOut = bufferOut.Receive();

							for (int e = 0; e <= 4; e++)
							{
								if (e == a || e == b || e == c || e == d) continue;

								bufferIn.Post(e); bufferIn.Post(dOut);
								await new IntCodeComputer(instructions.ToArray(), bufferIn, bufferOut).RunProgram();
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

		async Task DoPartB(List<long> instructions)
		{
			var max = 0L;
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

								var buffers = Enumerable.Range(0, 5).Select(i => new BufferBlock<long>()).ToList();
								buffers[0].Post(a);
								buffers[0].Post(0);
								buffers[1].Post(b);
								buffers[2].Post(c);
								buffers[3].Post(d);
								buffers[4].Post(e);

								await Task.WhenAll(
									new IntCodeComputer(instructions.ToArray(), buffers[0], buffers[1]).RunProgram(),
									new IntCodeComputer(instructions.ToArray(), buffers[1], buffers[2]).RunProgram(),
									new IntCodeComputer(instructions.ToArray(), buffers[2], buffers[3]).RunProgram(),
									new IntCodeComputer(instructions.ToArray(), buffers[3], buffers[4]).RunProgram(),
									new IntCodeComputer(instructions.ToArray(), buffers[4], buffers[0]).RunProgram());

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
	}
}
