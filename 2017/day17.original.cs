using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_17_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 17;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var key = Convert.ToInt32(input.GetString());

			var list = new LinkedList<int>();
			var position = list.AddFirst(0);
			Func<LinkedListNode<int>, LinkedListNode<int>> next = node =>
			{
				return node.Next ?? list.First;
			};

			for (int i = 1; i < 2018; i++)
			{
				for (int k = 0; k < key; k++)
					position = next(position);
				position = list.AddAfter(position, i);
			}

			Dump('A', next(position).Value);

			TotalMicroseconds = 839_018_162;

			// this is *such* a bad algorithm. leaving my shame for posterity
			// Year 2017, Day 17, Type  Original      :   839,018,162 µs

			//var loop_count = 50_000_001;
			//for (int i = 2018; i < loop_count; i++)
			//{
			//	for (int k = 0; k < key; k++)
			//		position = next(position);
			//	position = list.AddAfter(position, i);
			//}

			//Dump('B', list.Find(0).Next.Value);
		}
	}
}
