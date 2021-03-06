﻿using System;
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

			var span = new ReadOnlySpan<byte>(input);
			int part1 = 0, part2 = 0;
			Span<int> arr = stackalloc int[16];

			var j = 0;
			for (int i = 0; i < input.Length;)
			{
				var (x, y) = span[i..].AtoI();
				arr[j++] = x;
				i += y;

				if (span[i] == '\n')
				{
					int min = arr[0], max = arr[0];
					for (j = 1; j < 16; j++)
					{
						var n = arr[j];
						if (n < min) min = n;
						if (n > max) max = n;
					}

					part1 += max - min;

					var flag = false;
					for (j = 0; !flag && j < 16; j++)
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
				}

				i++;
			}

			PartA = part1.ToString();
			PartB = part2.ToString();
		}
	}
}
