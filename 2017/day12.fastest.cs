using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2017_12_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 12;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override unsafe void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var parents = stackalloc int[input.Length / 16];
			var rowNumbers = stackalloc int[16];
			int entryCount = 0, rowCount = 0, n = 0;

			foreach (var c in input)
			{
				if (c >= '0' && c <= '9')
					n = n * 10 + c - '0';
				else if (c == ',' || c == '<' || c == '\r')
				{
					// 0 is our key value, so shift everything up one
					rowNumbers[rowCount++] = n + 1;
					n = 0;
				}
				else if (c == '\n')
				{
					var min = int.MaxValue;
					for (int i = 0; i < rowCount; i++)
						if (rowNumbers[i] < min)
							min = rowNumbers[i];

					var parent = min;
					var tmp = rowCount;
					for (int i = 0; i < tmp; i++)
					{
						n = parents[rowNumbers[i]];
						if (n != 0)
						{
							while (parents[n] != 0 && parents[n] != n)
								n = parents[n];
							// reset the root of each tree as well
							rowNumbers[rowCount++] = n;

							if (n < parent)
								parent = n;
						}
					}

					for (int i = 0; i < rowCount; i++)
						parents[rowNumbers[i]] = parent;
					entryCount++;
					rowCount = 0;
					n = 0;
				}
			}

			int numGroups = 0, zeroGroupSize = 1;
			var zeroGroup = stackalloc int[entryCount];
			zeroGroup[0] = 1;
			for (int i = 1; i < entryCount; i++)
			{
				if (parents[i] == i)
					numGroups++;
				else
				{
					for (int j = zeroGroupSize - 1; j >= 0; j--)
					{
						if (zeroGroup[j] == parents[i])
						{
							zeroGroup[zeroGroupSize++] = i;
							break;
						}
					}
				}
			}

			PartA = zeroGroupSize.ToString();
			PartB = numGroups.ToString();
		}
	}
}
