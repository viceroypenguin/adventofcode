using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
	public class Day_2017_03_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 3;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var number = 0;
			for (int i = 0; i < input.Length; i++)
				if (input[i] >= '0')
					number = number * 10 + input[i] - '0';

			{
				var x = number - 1;
				var ring = (int)(Math.Sqrt((uint)x) + 1) / 2;
				Dump('A', ring + Math.Abs(x % (ring * 2) - ring));
			}

			{
				var arr = new long[512];
				arr[0] = 1; arr[1] = 1; arr[2] = 1;
				arr[3] = 1; arr[4] = 1; arr[5] = 1;
				arr[6] = 1; arr[7] = 2; arr[8] = 2;

				long p = 1, q = 9;
				for (int length = 2; ; length += 2)
				{
					for (int side = 0; side < 4; side++)
					{
						for (int i = length; i-- > 0;)
						{
							if (arr[p] > number)
							{
								Dump('B', arr[p]);
								return;
							}

							arr[q++] += arr[p];
							arr[q] += arr[p];
							arr[p + 1] += arr[p];
							arr[q + 1] += arr[p++];
						}

						if (side == 3) break;

						// Turn the corner
						arr[(++q) + 1] += arr[p - 1];
						arr[(++q) + 1] += arr[p - 1];
						arr[p] += arr[p - 2];

					}

					// Advance to the next ring
					arr[q++] += arr[p];
					arr[q++] += arr[p];
					arr[p + 1] += arr[p - 1];
				}
			}
		}
	}
}
