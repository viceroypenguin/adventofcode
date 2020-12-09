using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_09_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 9;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var numbers = input.GetLines()
				.Select(long.Parse)
				.ToArray();

			var invalidNumber = 0L;
			var queue = new Queue<long>(numbers.Take(25));
			foreach (var n in numbers.Skip(25))
			{
				if (queue.Subsets(2)
						.Where(l => l[0] != l[1])
						.Any(l => l[0] + l[1] == n))
				{
					queue.Dequeue();
					queue.Enqueue(n);
				}
				else
				{
					PartA = (invalidNumber = n).ToString();
					break;
				}
			}

			Array.Reverse(numbers);
			for (int i = 0; i < numbers.Length; i++)
			{
				if (numbers[i] > invalidNumber)
					continue;

				var x = numbers[i];
				var (sum, min, max) = (x, x, x);
				for (int j = i + 1; j < numbers.Length; j++)
				{
					sum += numbers[j];
					min = Math.Min(min, numbers[j]); 
					max = Math.Max(max, numbers[j]);
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
