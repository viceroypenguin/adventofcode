using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_09_Fastest : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 9;
		public override CodeType CodeType => CodeType.Fastest;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var span = new ReadOnlySpan<byte>(input);
			Span<long> arr = stackalloc long[input.Length / 8];
			int maxIndex = 0;

			for (int i = 0; i < span.Length;)
			{
				var (x, y) = span[i..].AtoL();
				arr[maxIndex++] = x;
				i += y + 1; // plus 1 to ignore next char
			}

			var invalidNumber = 0L;
			for (int i = 25; i < maxIndex; i++)
			{
				for (int j = i - 25; j < i; j++)
					for (int k = j + 1; k < i; k++)
						if (arr[j] + arr[k] == arr[i])
							goto found_match;

				PartA = (invalidNumber = arr[i]).ToString();
				break;

found_match:
				;
			}

			for (int i = maxIndex - 1; i >= 0; i--)
			{
				var x = arr[i];
				if (x > invalidNumber)
					continue;

				var (sum, min, max) = (x, x, x);
				for (int j = i - 1; j >= 0; j++)
				{
					sum += arr[j];
					min = Math.Min(min, arr[j]);
					max = Math.Max(max, arr[j]);
					if (sum == invalidNumber)
					{
						PartB = (min + max).ToString();
						return;
					}

					if (sum > invalidNumber)
						break;
				}
			}
		}
	}
}
