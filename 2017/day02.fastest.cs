using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
	public class Day_2017_02_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 2;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			int part1 = 0, part2 = 0;
			var arr = new int[16];

			int n = 0, j = 0;
			for (int i = 0; i < input.Length; i++)
			{
				var c = input[i];
				if (c == '\r')
				{
					arr[j++] = n;

					int min = arr[0], max = arr[0];
					for (j = 1; j < 16; j++)
					{
						n = arr[j];
						if (n < min) min = n;
						if (n > max) max = n;
					}

					part1 += max - min;

					var flag = false;
					for (j = 0; !flag && j < 16 ; j++)
						for (int k = 0; !flag && k < 16; k++)
						{
							if (j == k) continue;
							if (arr[j] % arr[k] == 0)
							{
								part2 += arr[j] / arr[k];
								flag = true;
							}
							else if (arr[k] % arr[j] == 0)
							{
								part2 += arr[k] / arr[j];
								flag = true;
							}
						}

					j = 0;
					n = 0;
				}
				else if (c == '\t')
				{
					arr[j++] = n;
					n = 0;
				}
				else if (c >= '0')
					n = n * 10 + c - '0';
			}

			PartA = part1.ToString();
			PartB = part2.ToString();
		}
	}
}
